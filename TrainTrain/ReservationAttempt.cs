using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain
{
    public class ReservationAttempt : ValueType<ReservationAttempt>
    {
        public string TrainId { get; }
        public List<Seat> Seats { get; }
        private int SeatsRequestedCount { get; }
        public string BookingReference => Seats.First().BookingRef;

        public bool IsFulFilled => Seats.Count == SeatsRequestedCount;

        public ReservationAttempt(string trainId, int seatsRequestedCount, IEnumerable<Seat> seats)
        {
            TrainId = trainId;
            SeatsRequestedCount = seatsRequestedCount;
            Seats = seats.ToList();
        }

        public ReservationAttempt AssignBookingReference(string bookingReference)
        {
            var seats = Seats.Select(seat => new Seat(seat.CoachName, seat.SeatNumber, bookingReference)).ToList();
            return new ReservationAttempt(TrainId, SeatsRequestedCount, seats);
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, Seats);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainId, SeatsRequestedCount, new ListByValue<Seat>(Seats)};
        }
    }
}