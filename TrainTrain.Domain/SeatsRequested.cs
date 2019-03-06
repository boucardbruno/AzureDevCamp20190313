using System.Collections.Generic;
using Value;

namespace TrainTrain.Domain
{
    public class SeatsRequested : ValueType<SeatsRequested>
    {
        public int Count { get; }

        public SeatsRequested(int count)
        {
            Count = count;
        }

        public bool IsMatch(IReadOnlyCollection<Seat> seats) => seats.Count == Count;

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {Count};
        }
    }
}