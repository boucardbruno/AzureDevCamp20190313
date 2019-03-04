using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain.Domain
{
    public class ReservationAttempt : ValueType<ReservationAttempt>
    {
        private readonly List<Seat> _seats;
        public TrainId TrainId { get; }
        public IReadOnlyCollection<Seat> Seats => _seats;
        public BookingReference BookingReference { get; }
        private SeatsRequested SeatsRequested { get; }

        public ReservationAttempt(TrainId trainId, SeatsRequested seatsRequestedCount, IEnumerable<Seat> seats)
            : this(trainId, new BookingReference(), seatsRequestedCount, seats)
        {
        }

        private ReservationAttempt(TrainId trainId, BookingReference bookingReference,
            SeatsRequested seatsRequestedCount, IEnumerable<Seat> seats)
        {
            TrainId = trainId;
            BookingReference = bookingReference;
            SeatsRequested = seatsRequestedCount;
            _seats = seats.ToList();
        }

        public bool IsFulFilled => Seats.Count == SeatsRequested.Count;

        // DDD Pattern: Closure Of Operation
        public ReservationAttempt AssignBookingReference(BookingReference bookingReference)
        {
            var seats = Seats.Select(seat => new Seat(seat.CoachName, seat.SeatNumber, BookingReference));

            return new ReservationAttempt(TrainId, bookingReference, SeatsRequested, seats);
        }

        public Reservation Confirm()
        {
            return new Reservation(TrainId, BookingReference, _seats);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainId, BookingReference, SeatsRequested, new ListByValue<Seat>(_seats)};
        }
    }
}