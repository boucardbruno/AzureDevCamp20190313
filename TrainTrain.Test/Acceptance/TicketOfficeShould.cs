using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace TrainTrain.Test.Acceptance
{
    public class TicketOfficeShould
    {
        private const string TrainId = "9043-2019-03-13";
        private const string BookingReference = "75bcd15";

        [Test]
        public async Task Reserve_seats_when_train_is_empty()
        {
            const int seatsRequestedCount = 3;

            var trainDataService = BuildTrainDataService(TrainId, TrainTopologyGenerator
                .With_10_available_seats());
            var bookingReferenceService = BuildBookingReferenceService(BookingReference);

            var reservation = await new TicketOffice(trainDataService, bookingReferenceService)
                .Reserve(TrainId, seatsRequestedCount);

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation))
                .IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1A\", \"2A\", \"3A\"]}}");
        }

        [Test]
        public async Task Not_reserve_seats_when_it_exceed_max_capacity_threshold()
        {
            const int seatsRequestedCount = 3;

            var trainDataService = BuildTrainDataService(TrainId, TrainTopologyGenerator
                .With_10_seats_and_6_already_reserved());
            var bookingReferenceService = BuildBookingReferenceService(BookingReference);

            var reservation = await new TicketOffice(trainDataService, bookingReferenceService)
                .Reserve(TrainId, seatsRequestedCount);

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation))
                .IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"\", \"seats\": []}}");
        }


        [Test]
        [Ignore("Separated party")]
        public async Task Reserve_all_seats_in_the_same_coach()
        {
            const int seatsRequestedCount = 2;

            var trainDataService = BuildTrainDataService(TrainId, TrainTopologyGenerator
                .With_2_coaches_and_9_seats_already_reserved_in_the_first_coach());
            var bookingReferenceService = BuildBookingReferenceService(BookingReference);

            var reservation = await new TicketOffice(trainDataService, bookingReferenceService)
                .Reserve(TrainId, seatsRequestedCount);

            Check.That(SeatsReservationAdapter.AdaptReservation(reservation))
                .IsEqualTo($"{{\"train_id\": \"{TrainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1B\", \"2B\"]}}");
        }
        
        private static IBookingReferenceService BuildBookingReferenceService(string bookingReference)
        {
            var bookingReferenceService = Substitute.For<IBookingReferenceService>();
            bookingReferenceService.GetBookingReference().Returns(Task.FromResult(bookingReference));
            return bookingReferenceService;
        }

        private static ITrainDataService BuildTrainDataService(string trainId, string trainTopology)
        {
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrainId(trainId)
                .Returns(Task.FromResult(new Train(trainId, TrainDataServiceAdapter.AdaptTrainTopology(trainTopology))));
            return trainDataService;
        }
    }
}
