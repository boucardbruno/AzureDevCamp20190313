using System;
using System.Collections.Generic;
using Value;

namespace TrainTrain.Domain
{
    public class TrainId : ValueType<TrainId>
    {
        public string TrainNumber { get; }
        public DateTime Date { get; }
        public string Id => $"{TrainNumber}-{Date:yyyy-MM-dd}";

        public TrainId(string id)
        {
            var tokens = id.Split("-");

            if (tokens[0].Length == 4)
            {
                TrainNumber = tokens[0];
            }

            Date = new DateTime(int.Parse(tokens[1]), int.Parse(tokens[2]), int.Parse(tokens[3]));
        }

        public override string ToString()
        {
            return Id;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] {TrainNumber, Date};
        }
    }
}