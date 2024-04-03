using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainReservationOffice;

public class ReservationAttempt(
    TrainId trainId,
    RequestForSeats requestForSeats,
    IEnumerable<Seat> seats,
    BookingReference bookingReference)
    : ValueType<ReservationAttempt>
{
    public TrainId TrainId { get; } = trainId;
    public List<Seat> Seats { get; } = seats.ToList();
    public BookingReference BookingReference { get; } = bookingReference;
    private RequestForSeats RequestForSeats { get; } = requestForSeats;

    public bool IsFulFilled()
    {
        return Seats.Count == RequestForSeats.NumberOfSeats;
    }

    public ReservationAttempt AssignBookingReference(BookingReference bookingReference)
    {
        return new ReservationAttempt(
            TrainId,
            RequestForSeats,
            seats.Select(s => new Seat(s.CoachName, s.SeatNumber, bookingReference)),
            bookingReference);
    }

    public Reservation Confirm()
    {
        return new Reservation(TrainId, BookingReference, Seats);
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
        return new object[]
            { TrainId, BookingReference, new ListByValue<Seat>(Seats) };
    }
}