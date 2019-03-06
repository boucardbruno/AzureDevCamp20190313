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
            var trainIds = id.Split("-");
            if (trainIds[0].Length == 4)
            {
                TrainNumber = trainIds[0];
            }
            Date = new DateTime(int.Parse(trainIds[1]), int.Parse(trainIds[2]), int.Parse(trainIds[3]));
        }

        public override string ToString()
        {
            return Id;
        }

        protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
        {
            return new object[] { TrainNumber, Date};
        }
    }
}