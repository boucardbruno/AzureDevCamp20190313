using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;

namespace TrainTrain.Infra
{
    public class SeatsReservationAdapter
    {
        private readonly IProvideReservation _hexagon;

        public SeatsReservationAdapter(IProvideReservation hexagon)
        {
            _hexagon = hexagon;
        }

        public static string AdaptReservation(Reservation reservation)
        {
            return
                $"{{\"train_id\": \"{reservation.TrainId}\", \"booking_reference\": \"{reservation.BookingReference}\", \"seats\": {DumpSeats(reservation.Seats)}}}";
        }

        private static string DumpSeats(IEnumerable<Seat> seats)
        {
            var sb = new StringBuilder("[");

            var firstTime = true;
            foreach (var seat in seats)
            {
                if (!firstTime)
                    sb.Append(", ");
                else
                    firstTime = false;

                sb.Append($"\"{seat.SeatNumber}{seat.CoachName}\"");
            }

            sb.Append("]");

            return sb.ToString();
        }

        public async Task<string> Reserve(string trainId, int seatsRequestedCount)
        {
            var id = new TrainId(trainId);
            var seatsRequested = new SeatsRequested(seatsRequestedCount);
            var reservation = await _hexagon.Reserve(id, seatsRequested);
            return AdaptReservation(reservation);
        }
    }
}