using NFluent;
using NUnit.Framework;
using TrainTrain.Domain;

namespace TrainTrain.Test.Unit
{
    internal class SeatRequestedShould
    {
        [Test]
        public void Be_value_object()
        {
            var seatsRequested = new SeatsRequested(3);
            var sameSeatsRequested = new SeatsRequested(3);

            Check.That(seatsRequested).IsEqualTo(sameSeatsRequested);
        }
    }
}