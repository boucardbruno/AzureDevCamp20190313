using System;
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

        [Test]
        public void Raise_exception_when_seats_requested_count_is_invalid()
        {
            Check.That(SeatsRequested.MinRequested).IsEqualTo(1);
            Check.That(SeatsRequested.MaxRequested).IsEqualTo(20);

            Check.ThatCode(() => new SeatsRequested(SeatsRequested.MinRequested -1)).Throws<ArgumentException>();
            Check.ThatCode(() => new SeatsRequested(SeatsRequested.MaxRequested +1)).Throws<ArgumentException>();
        }
    }
}