using System;
using System.Collections.Generic;
using Value;

namespace TrainTrain.Domain
{
    public class SeatsRequested : ValueType<SeatsRequested>
    {
        public const int MinRequested = 1;
        public const int MaxRequested = 20;

        public int Count { get; }

        public SeatsRequested(int seatRequestCount)
        {
            if (seatRequestCount <= MinRequested || seatRequestCount > MaxRequested)
                throw new ArgumentException(
                    $"{nameof(seatRequestCount)}({seatRequestCount}) should be between {MinRequested} and {MaxRequested}");

            Count = seatRequestCount;
        }

        public bool IsMatch(IReadOnlyCollection<Seat> seats)
        {
            return seats.Count == Count;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {Count};
        }
    }
}