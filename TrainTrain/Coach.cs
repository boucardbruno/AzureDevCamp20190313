using System.Collections.Generic;
using System.Linq;
using Value;

namespace TrainTrain
{
    public class Coach : ValueType<Coach>
    {
        public List<Seat> Seats { get; }
        private string TrainId { get; }
        private string Name { get; }

        public Coach(string trainId, string name) : this(trainId, name, new List<Seat>())
        {
        }

        private Coach(string trainId, string name, List<Seat> seats)
        {
            TrainId = trainId;
            Name = name;
            Seats = seats;
        }

        public Coach AddSeat(Seat seat)
        {
            return new Coach(TrainId, Name, new List<Seat>(Seats) {seat});
        }

        public ReservationAttempt BuildReservationAttempt(int seatsRequestedCount)
        {
            var availableSeats = Seats.Where(s => s.IsAvailable).Take(seatsRequestedCount);
            return new ReservationAttempt(TrainId, seatsRequestedCount, availableSeats);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainId, Name, new ListByValue<Seat>(Seats)};
        }
    }
}