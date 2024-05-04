using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace TrainTrain
{
    public static class SeatsReservationAdapter
    {
        public static string AdaptReservation(Reservation reservation)
        {
            return
                $"{{\"train_id\": \"{reservation.TrainId}\", \"booking_reference\": \"{reservation.BookingReference}\", \"seats\": {DumpSeats(reservation.Seats)}}}";
        }

        private static string DumpSeats(IEnumerable<Seat> seats)
        {
            var seatsAString = new StringBuilder("[");
            seatsAString.Append(string.Join(", ", seats));
            seatsAString.Append(']');
            return seatsAString.ToString();
        }
    }
}