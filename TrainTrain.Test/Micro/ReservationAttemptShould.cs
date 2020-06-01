using System.Collections.Generic;
using NFluent;
using NUnit.Framework;
using TrainTrain.Domain;

namespace TrainTrain.Test.Micro
{
    internal class ReservationAttemptShould
    {
        private const string TrainId = "9043-2020-11-13";
        private const string BookingReference = "75bcd15";

        [Test]
        public void Be_equal_by_value()
        {
            var reservationAttempt1 = new ReservationAttempt(new TrainId(TrainId), new SeatsRequested(3), new List<Seat>()
            {
                new Seat("A", 1, BookingReference),
                new Seat("A", 2, BookingReference),
                new Seat("A", 3, BookingReference)
            });

            var reservationAttempt2 = new ReservationAttempt(new TrainId(TrainId), new SeatsRequested(3), new List<Seat>()
            {
                new Seat("A", 1, BookingReference),
                new Seat("A", 2, BookingReference),
                new Seat("A", 3, BookingReference)
            });

            Check.That(reservationAttempt1).IsEqualTo(reservationAttempt2);

            var reservationAttemptFailure1 = new ReservationAttemptFailure(new TrainId(TrainId), new SeatsRequested(3));

            var reservationAttemptFailure2 = new ReservationAttemptFailure(new TrainId(TrainId), new SeatsRequested(3));

            Check.That(reservationAttemptFailure1).IsEqualTo(reservationAttemptFailure2);
        }
    }
}