using System;
using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain.Domain
{
    public class Coach : ValueType<Coach>
    {
        private readonly List<Seat> _seats;
        public IReadOnlyCollection<Seat> Seats => _seats;
        private string Name { get; }

        private int NumberOfReservedSeats
        {
            get { return Seats.Count(s => !s.IsAvailable()); }
        }

        public Coach(string name) : this(name, new List<Seat>())
        {
        }

        public Coach(string name, List<Seat> seats)
        {
            Name = name;
            _seats = seats;
        }

        // DDD Pattern: Closure Of Operation
        public Coach AddSeat(Seat seat)
        {
            return new Coach(seat.CoachName, new List<Seat>(Seats) {seat});
        }

        public ReservationAttempt BuildReservationAttempt(TrainId trainId, SeatsRequested seatsRequested)
        {
            var availableSeats = Seats.Where(s => s.IsAvailable()).Take(seatsRequested.Count).ToList();
            return seatsRequested.IsMatch(availableSeats)
                ? new ReservationAttempt(trainId, seatsRequested, availableSeats)
                : new ReservationAttemptFailure(trainId, seatsRequested);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {Name, new ListByValue<Seat>(_seats)};
        }

        public bool DoesNotExceedOverallCapacity(SeatsRequested seatsRequested)
        {
            return NumberOfReservedSeats + seatsRequested.Count <=
                   Math.Floor(CapacityThresholdPolicy.ForCoach * Seats.Count);
        }
    }
}