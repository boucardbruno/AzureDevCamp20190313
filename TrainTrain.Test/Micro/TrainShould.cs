using NFluent;
using NUnit.Framework;
using TrainTrain.Domain;
using TrainTrain.Infra;
using TrainTrain.Test.Acceptance;

namespace TrainTrain.Test.Micro
{
    internal class TrainShould
    {
        private const string TrainId = "9043-2020-11-13";

        [Test]
        public void Be_equal_by_value()
        {
            var train1 = new Train(new TrainId(TrainId),
                TrainDataServiceAdapter.AdaptTrainTopology(TrainTopologyGenerator.With_10_available_seats()));
            var train2 = new Train(new TrainId(TrainId),
                TrainDataServiceAdapter.AdaptTrainTopology(TrainTopologyGenerator.With_10_available_seats()));

            Check.That(train1).IsEqualTo(train2);
        }
    }
}
