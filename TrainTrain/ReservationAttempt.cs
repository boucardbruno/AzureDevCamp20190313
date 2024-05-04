using System.Collections.Generic;
using System.Linq;

namespace TrainTrain
{
    public class ReservationAttempt(string trainId, int seatsRequestedCount, IEnumerable<Seat> seats)
    {
        public string TrainId { get; } = trainId;
        public List<Seat> Seats { get; private set; } = seats.ToList();
        public string BookingReference { get; private set; }
        private int SeatsRequestedCount { get; } = seatsRequestedCount;

        public bool IsFulfilled()
        {
            return Seats.Count == SeatsRequestedCount;
        }

        public void AssignBookingReference(string bookingReference)
        {
            BookingReference = bookingReference;
            Seats = Seats
                .Select(s => new Seat(s.CoachName, s.SeatNumber, bookingReference))
                .ToList();
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, Seats);
        }
    }
}