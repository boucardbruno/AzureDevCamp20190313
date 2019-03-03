using System.Collections.Generic;
using Value;

namespace TrainTrain.Domain
{
    public class Reservation : ValueType<Reservation>
    {
        private readonly List<Seat> _seats;
        public TrainId TrainId { get; }
        public BookingReference BookingReference { get; }
        public IReadOnlyCollection<Seat> Seats => _seats;

        public Reservation(TrainId trainId, BookingReference bookingReference, List<Seat> seats)
        {
            TrainId = trainId;
            BookingReference = bookingReference;
            _seats = seats;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainId, BookingReference, new ListByValue<Seat>(_seats)};
        }
    }
}