namespace TrainReservationOffice;

public record Seat(string CoachName, int SeatNumber, BookingReference bookingReference)
{
    public bool IsAvailable => bookingReference.Id == "";
}