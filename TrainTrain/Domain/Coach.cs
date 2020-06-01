using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain.Domain
{
    public class Coach : ValueType<Coach>
    {
        public List<Seat> Seats { get; }
        private TrainId TrainId { get; }
        private string Name { get; }

        public Coach(TrainId trainId, string name) : this(trainId, name, new List<Seat>())
        {
        }

        private Coach(TrainId trainId, string name, List<Seat> seats)
        {
            TrainId = trainId;
            Name = name;
            Seats = seats;
        }

        public Coach AddSeat(Seat seat)
        {
            return new Coach(TrainId, Name, new List<Seat>(Seats) {seat});
        }

        public ReservationAttempt BuildReservationAttempt(SeatsRequested seatsRequested)
        {
            var availableSeats = Seats.Where(s => s.IsAvailable).Take(seatsRequested.Count);
            return new ReservationAttempt(TrainId, seatsRequested, availableSeats);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainId, Name, new ListByValue<Seat>(Seats)};
        }
    }
}