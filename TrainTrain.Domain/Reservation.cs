using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain.Domain
{
    public class Reservation(TrainId trainId, BookingReference bookingReference, IEnumerable<Seat> seats)
        : ValueType<Reservation>
    {
        private readonly List<Seat> _seats = seats.ToList();
        public TrainId TrainId { get; } = trainId;
        public BookingReference BookingReference { get; } = bookingReference;
        public IReadOnlyCollection<Seat> Seats => _seats;

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return [TrainId, BookingReference, new ListByValue<Seat>(_seats)];
        }
    }
}