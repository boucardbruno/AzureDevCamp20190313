using System.Collections.Generic;
using Value;

namespace TrainTrain.Domain
{
    public class Seat : ValueType<Seat>
    {
        public string CoachName { get; }
        public int SeatNumber { get; }
        public string BookingRef { get; }

        public bool IsAvailable => BookingRef == string.Empty;

        public Seat(string coachName, int seatNumber, string bookingRef)
        {
            CoachName = coachName;
            SeatNumber = seatNumber;
            BookingRef = bookingRef;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {CoachName, SeatNumber, BookingRef};
        }
    }
}