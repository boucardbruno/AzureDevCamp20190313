using System.Collections.Generic;
using NFluent;
using NUnit.Framework;
using TrainTrain.Domain;

namespace TrainTrain.Test.Unit
{
    internal class ReservationAttemptShould
    {
        [Test]
        public void Be_value_object()
        {
            var reservationAttempt = new ReservationAttempt(new TrainId("express-2000"), new SeatsRequested(3),
                new List<Seat> {new Seat("A", 1)});
            var sameReservationAttempt = new ReservationAttempt(new TrainId("express-2000"), new SeatsRequested(3),
                new List<Seat> {new Seat("A", 1)});

            Check.That(reservationAttempt).IsEqualTo(sameReservationAttempt);
        }
    }
}