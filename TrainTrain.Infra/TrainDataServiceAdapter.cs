﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrainTrain.Domain;

namespace TrainTrain.Infra
{
    public class TrainDataServiceAdapter : ITrainDataService
    {
        private readonly string _uriTrainDataService;

        public TrainDataServiceAdapter(string uriTrainDataService)
        {
            _uriTrainDataService = uriTrainDataService;
        }

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
                    BuildPostContent(reservationAttempt.TrainId.Id, reservationAttempt.BookingReference.Id, reservationAttempt.Seats), Encoding.UTF8,
                    "application/json");
                var response = await client.PostAsync("reserve", resJson);

                response.EnsureSuccessStatusCode();
            }
        }

        private static string BuildPostContent(string trainId, string bookingRef, IEnumerable<Seat> availableSeats)
        {
            var seats = new StringBuilder("[");
            bool firstTime = true;

            foreach (var s in availableSeats)
            {
                if (!firstTime)
                {
                    seats.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                seats.Append($"\"{s.SeatNumber}{s.CoachName}\"");
            }
            seats.Append("]");

            var result = $"{{\r\n\t\"train_id\": \"{trainId}\",\r\n\t\"seats\": {seats},\r\n\t\"booking_reference\": \"{bookingRef}\"\r\n}}";

            return result;
        }

        public static Dictionary<string, Coach> AdaptTrainTopology(string trainTopology)
        {
            var coaches = new Dictionary<string, Coach>();
            //var sample =
            //"{\"seats\": {\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, \"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}}}";

            // Forced to workaround with dynamic parsing since the received JSON is invalid format ;-(
            dynamic parsed = JsonConvert.DeserializeObject(trainTopology);

            foreach (var token in ((Newtonsoft.Json.Linq.JContainer) parsed))
            {
                var allStuffs = ((Newtonsoft.Json.Linq.JObject) ((Newtonsoft.Json.Linq.JContainer) token).First);

                foreach (var stuff in allStuffs)
                {
                    var seatPoco = stuff.Value.ToObject<SeatJsonPoco>();
                    var seat = new Seat(seatPoco.coach, int.Parse(seatPoco.seat_number), new BookingReference(seatPoco.booking_reference));
                    if (!coaches.ContainsKey(seatPoco.coach))
                    {
                        coaches[seatPoco.coach] = new Coach(seatPoco.coach);
                    }
                    coaches[seatPoco.coach]  = coaches[seatPoco.coach].AddSeat(seat);
                }
            }
            return coaches;
        }
    }
}