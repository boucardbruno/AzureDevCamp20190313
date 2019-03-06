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
            const string trainPoco = "9043-2019-03-13";
            var trainId = new TrainId(trainPoco);
            var sameTrainId = new TrainId(trainPoco);

            Check.That(trainId).IsEqualTo(sameTrainId);
            Check.That(trainId.TrainNumber).IsEqualTo(sameTrainId.TrainNumber);
            Check.That(trainId.Date).IsEqualTo(sameTrainId.Date);
        }
    }
}