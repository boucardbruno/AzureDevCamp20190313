using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;
using TrainTrain.Domain.Port;

namespace TrainTrain.Infra.Adapter
{
    public class SeatsReservationAdapter
    {
        private readonly IProvideSeatsReservation _ticketOffice;

        public SeatsReservationAdapter(IProvideSeatsReservation ticketOffice)
        {
            _ticketOffice = ticketOffice;
        }

        public async Task<string> ReserveAsync(string trainId, int seatsRequestedCount)
        {
           
            return AdaptReservation(await _ticketOffice.Reserve(new TrainId(trainId), new SeatsRequested(seatsRequestedCount)));
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
                {
                    sb.Append(", ");
                }
                else
                {
                    firstTime = false;
                }

                sb.Append($"\"{seat.SeatNumber}{seat.CoachName}\"");
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}