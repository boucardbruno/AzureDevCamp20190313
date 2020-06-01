using System;
using System.Collections.Generic;
using System.Linq;
using Value;
using Value.Shared;

namespace TrainTrain
{
    public class Train : ValueType<Train>
    {
        public string TrainId { get; }
        public Dictionary<string, Coach> Coaches { get; } = new Dictionary<string, Coach>();

        public List<Seat> Seats
        {
            get { return Coaches.Values.SelectMany(c => c.Seats).ToList(); }
        }

        private int ReservedSeats
        {
            get { return Seats.Count(s => !string.IsNullOrEmpty(s.BookingRef)); }
        }

        private int MaxSeats => Seats.Count;


        public Train(string trainId, IEnumerable<Seat> seats)
        {
            TrainId = trainId;
            foreach (var seat in seats)
            {
                if (!Coaches.ContainsKey(seat.CoachName)) Coaches[seat.CoachName] = new Coach(trainId, seat.CoachName);
                Coaches[seat.CoachName] = Coaches[seat.CoachName].AddSeat(seat);
            }
        }

        public bool DoesNotExceedOverallTrainCapacity(int seatsRequestedCount)
        {
            return ReservedSeats + seatsRequestedCount <= Math.Floor(ThresholdReservation.MaxCapacity * MaxSeats);
        }

        public ReservationAttempt BuildReservationAttempt(int seatsRequestedCount)
        {
            foreach (var coachName in Coaches.Keys)
            {
                var reservationAttempt = Coaches[coachName].BuildReservationAttempt(seatsRequestedCount);
                if (reservationAttempt.IsFulFilled) return reservationAttempt;
            }

            return new ReservationAttemptFailure(TrainId, seatsRequestedCount);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainId, new DictionaryByValue<string, Coach>(Coaches)};
        }
    }
}