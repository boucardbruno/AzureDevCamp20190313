﻿using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TrainTrain.Domain;

namespace TrainTrain.Infra
{
    public class TrainDataServiceAdapter : ITrainDataService
    {
        private readonly string _uriTrainDataService;

        public async Task<Train> GetTrain(TrainId trainId)
        {
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_uriTrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);

                // HTTP GET
                var response = await client.GetAsync($"api/data_for_train/{trainId}");
                response.EnsureSuccessStatusCode();
                return new Train(trainId, AdaptTrainTopology(await response.Content.ReadAsStringAsync()));
            }
        }

        public async Task BookSeats(ReservationAttempt reservationAttempt)
        {
            using (var client = new HttpClient())
            {
                var value = new MediaTypeWithQualityHeaderValue("application/json");
                client.BaseAddress = new Uri(_uriTrainDataService);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(value);

                // HTTP POST
                HttpContent resJson = new StringContent(
                    BuildPostContent(reservationAttempt.TrainId.Id, reservationAttempt.BookingReference,
                        reservationAttempt.Seats), Encoding.UTF8,
                    "application/json");
                var response = await client.PostAsync("reserve", resJson);

                response.EnsureSuccessStatusCode();
            }
        }

        public TrainDataServiceAdapter(string uriTrainDataService)
        {
            _uriTrainDataService = uriTrainDataService;
        }

        private static string BuildPostContent(string trainId, string bookingRef, IEnumerable<Seat> availableSeats)
        {
            var seats = new StringBuilder("[");
            var firstTime = true;

            foreach (var s in availableSeats)
            {
                if (!firstTime)
                    seats.Append(", ");
                else
                    firstTime = false;

                seats.Append($"\"{s.SeatNumber}{s.CoachName}\"");
            }

            seats.Append("]");

            var result =
                $"{{\r\n\t\"train_id\": \"{trainId}\",\r\n\t\"seats\": {seats},\r\n\t\"booking_reference\": \"{bookingRef}\"\r\n}}";

            return result;
        }

        public static List<Seat> AdaptTrainTopology(string trainTopology)
        {
            var seats = new List<Seat>();
            //var sample =
            //"{\"seats\": {\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, \"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}}}";

            // Forced to workaround with dynamic parsing since the received JSON is invalid format ;-(
            dynamic parsed = JsonConvert.DeserializeObject(trainTopology)!;

            foreach (var token in (JContainer) parsed)
            {
                var allStuffs = (JObject) ((JContainer) token).First!;

                foreach (var stuff in allStuffs)
                {
                    var seatPoco = stuff.Value!.ToObject<SeatJsonPoco>();
                    seats.Add(new Seat(seatPoco!.coach, int.Parse(seatPoco.seat_number), seatPoco.booking_reference));
                }
            }

            return seats;
        }
    }
}