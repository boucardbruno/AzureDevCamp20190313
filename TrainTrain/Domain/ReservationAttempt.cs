using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain.Domain
{
    public class ReservationAttempt : ValueType<ReservationAttempt>
    {
        public TrainId TrainId { get; }
        public List<Seat> Seats { get; }
        private SeatsRequested SeatsRequested { get; }
        public string BookingReference => Seats.First().BookingRef;

        public bool IsFulFilled => Seats.Count == SeatsRequested.Count;

        public ReservationAttempt(TrainId trainId, SeatsRequested seatsRequested, IEnumerable<Seat> seats)
        {
            TrainId = trainId;
            SeatsRequested = seatsRequested;
            Seats = seats.ToList();
        }

        public ReservationAttempt AssignBookingReference(string bookingReference)
        {
            var seats = Seats.Select(seat => new Seat(seat.CoachName, seat.SeatNumber, bookingReference)).ToList();
            return new ReservationAttempt(TrainId, SeatsRequested, seats);
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, Seats);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainId, SeatsRequested, new ListByValue<Seat>(Seats)};
        }
    }
}