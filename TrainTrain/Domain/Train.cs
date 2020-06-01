using System;
using System.Collections.Generic;
using System.Linq;
using Value;
using Value.Shared;

namespace TrainTrain.Domain
{
    public class Train : ValueType<Train>
    {
        public TrainId TrainId { get; }
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


        public Train(TrainId trainId, IEnumerable<Seat> seats)
        {
            TrainId = trainId;
            foreach (var seat in seats)
            {
                if (!Coaches.ContainsKey(seat.CoachName)) Coaches[seat.CoachName] = new Coach(trainId, seat.CoachName);
                Coaches[seat.CoachName] = Coaches[seat.CoachName].AddSeat(seat);
            }
        }

        public bool DoesNotExceedOverallTrainCapacity(SeatsRequested seatsRequested)
        {
            return ReservedSeats + seatsRequested.Count <= Math.Floor(ThresholdReservation.MaxCapacity * MaxSeats);
        }

        public ReservationAttempt BuildReservationAttempt(SeatsRequested seatsRequested)
        {
            foreach (var coachName in Coaches.Keys)
            {
                var reservationAttempt = Coaches[coachName].BuildReservationAttempt(seatsRequested);
                if (reservationAttempt.IsFulFilled) return reservationAttempt;
            }

            return new ReservationAttemptFailure(TrainId, seatsRequested);
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainId.Id, new DictionaryByValue<string, Coach>(Coaches)};
        }
    }
}