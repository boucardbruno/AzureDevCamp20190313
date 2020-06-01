using System.Collections.Generic;
using NFluent;
using NUnit.Framework;

namespace TrainTrain.Test.Micro
{
    internal class ReservationShould
    {
        private const string TrainId = "9043-2020-11-13";
        private const string BookingReference = "75bcd15";


        [Test]
        public void Be_equal_by_value()
        {
            var reservation1 = new Reservation(TrainId, BookingReference, new List<Seat>()
            {
                new Seat("A", 1, BookingReference),
                new Seat("A", 2, BookingReference),
                new Seat("A", 3, BookingReference)
            });

            var reservation2 = new Reservation(TrainId, BookingReference, new List<Seat>()
            {
                new Seat("A", 1, BookingReference),
                new Seat("A", 2, BookingReference),
                new Seat("A", 3, BookingReference)
            });

            Check.That(reservation1).IsEqualTo(reservation2);
        }
    }
}