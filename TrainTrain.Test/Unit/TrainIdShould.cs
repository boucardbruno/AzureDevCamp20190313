using NFluent;
using NUnit.Framework;
using TrainTrain.Domain;

namespace TrainTrain.Test.Unit
{
    internal class TrainIdShould
    {
        [Test]
        public void Be_value_object()
        {
            var trainId = new TrainId("Express-2000");
            var sameTrainId = new TrainId("Express-2000");

            Check.That(trainId).IsEqualTo(sameTrainId);
        }
    }
}