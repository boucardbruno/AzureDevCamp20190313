using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainReservationOffice;

public class Reservation(TrainId trainId, BookingReference bookingReference, IEnumerable<Seat> seats)
    : ValueType<Reservation>
{
    public TrainId TrainId { get; } = trainId;
    public BookingReference BookingReference { get; } = bookingReference;
    public List<Seat> Seats { get; } = seats.ToList();

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
        return new object[] { TrainId, BookingReference, new ListByValue<Seat>(Seats) };
    }
}