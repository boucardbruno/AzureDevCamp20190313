using System.Collections.Generic;
using Value;

namespace TrainTrain
{
    public class Reservation : ValueType<Reservation>
    {
        public string TrainId { get; }
        public string BookingReference { get; }
        public List<Seat> Seats { get; }

        public Reservation(string trainId, string bookingReference, List<Seat> seats)
        {
            TrainId = trainId;
            BookingReference = bookingReference;
            Seats = seats;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainId, BookingReference, new ListByValue<Seat>(Seats)};
        }
    }
}