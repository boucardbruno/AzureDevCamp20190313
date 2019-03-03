using NFluent;
using NUnit.Framework;
using TrainTrain.Domain;

namespace TrainTrain.Test.Unit
{
    internal class BookingReferenceShould
    {
        [Test]
        public void Be_value_object()
        {
            var bookingReference = new BookingReference("AK23H");
            var sameBookingReference = new BookingReference("AK23H");

            Check.That(bookingReference).IsEqualTo(sameBookingReference);
        }
    }
}