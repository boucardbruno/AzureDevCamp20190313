using System.Collections.Generic;
using System.Text;

namespace TrainReservationOffice;

public static class SeatsReservationAdapter
{
    public static string AdaptReservation(Reservation reservation)
    {
        return
            $"{{\"train_id\": \"{reservation.TrainId.Id}\", \"booking_reference\": \"{reservation.BookingReference.Id}\", \"seats\": {DumpSeats(reservation.Seats)}}}";
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
}