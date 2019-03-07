using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;
using TrainTrain.Domain.Port;

namespace TrainTrain.Infra.Adapter
{
    public class SeatsReservationAdapter
    {
        private readonly IProvideTicket _ticketOffice;

        public SeatsReservationAdapter(IProvideTicket ticketOffice)
        {
            _ticketOffice = ticketOffice;
        }

        public async Task<string> ReserveAsync(string trainId, int seatsRequestedCount)
        {
           
            return AdaptReservation(await _ticketOffice.Reserve(new TrainId(trainId), new SeatsRequested(seatsRequestedCount)));
        }

        public static string AdaptReservation(Reservation reservation)
        {
            return $"{{\"train_id\": \"{reservation.TrainId}\", \"booking_reference\": \"{reservation.BookingReference}\", \"seats\": {AdaptSeats(reservation.Seats)}}}";
        }

        private static string AdaptSeats(IEnumerable<Seat> seats)
        {
            var sb = new StringBuilder("[");
            sb.Append(string.Join(", ", seats));
            sb.Append("]");
            return sb.ToString();
        }
    }
}