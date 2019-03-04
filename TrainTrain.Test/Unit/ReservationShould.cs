using System.Collections.Generic;
using NFluent;
using NUnit.Framework;
using TrainTrain.Domain;

namespace TrainTrain.Test.Unit
{
    internal class ReservationShould
    {
        [Test]
        public void Be_value_object()
        {
            var reservation = new Reservation(new TrainId("Express-2000"), new BookingReference("1BW80"),
                new List<Seat> {new Seat("A", 1)});
            var sameReservation = new Reservation(new TrainId("Express-2000"), new BookingReference("1BW80"),
                new List<Seat> {new Seat("A", 1)});

            Check.That(sameReservation).IsEqualTo(reservation);
        }
    }
}