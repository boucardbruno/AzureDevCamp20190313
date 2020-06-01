using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using NUnit.Framework;
using TrainTrain.Domain;
using TrainTrain.Infra;

namespace TrainTrain.Test.Acceptance
{
    public class TrainTrainShould
    {
        private readonly TrainId _trainId = new TrainId("9043-2020-11-13");
        private const string BookingReference = "75bcd15";

        [Test]
        public async Task Reserve_seats_when_train_is_empty()
        {
            const int seatsRequestedCount = 3;

            var trainDataService = BuildTrainDataService(_trainId, TrainTopologyGenerator.With_10_available_seats());
            var bookingReferenceService = BuildBookingReferenceService(BookingReference);

            IProvideReservation hexagon = new TicketOffice(trainDataService, bookingReferenceService);
            var seatsReservationAdapter = new SeatsReservationAdapter(hexagon);
            var jsonReservation = await seatsReservationAdapter.Reserve(_trainId.Id, seatsRequestedCount);

            Check.That(jsonReservation)
                .IsEqualTo($"{{\"train_id\": \"{_trainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1A\", \"2A\", \"3A\"]}}");
        }

        [Test]
        public async Task Not_reserve_seats_when_it_exceed_max_capacity_threshold()
        {
            const int seatsRequestedCount = 3;

            var trainDataService = BuildTrainDataService(_trainId, TrainTopologyGenerator.With_10_seats_and_6_already_reserved());
            var bookingReferenceService = BuildBookingReferenceService(BookingReference);

            var ticketOffice = new TicketOffice(trainDataService, bookingReferenceService);
            var seatsReservationAdapter = new SeatsReservationAdapter(ticketOffice);
            var jsonReservation = await seatsReservationAdapter.Reserve(_trainId.Id, seatsRequestedCount);

            Check.That(jsonReservation)
                .IsEqualTo($"{{\"train_id\": \"{_trainId}\", \"booking_reference\": \"\", \"seats\": []}}");
        }

        [Test]
        public async Task Reserve_all_seats_in_the_same_coach()
        {
            const int seatsRequestedCount = 2;

            var trainDataService = BuildTrainDataService(_trainId, TrainTopologyGenerator.With_2_coaches_and_9_seats_already_reserved_in_the_first_coach());
            var bookingReferenceService = BuildBookingReferenceService(BookingReference);

            var ticketOffice = new TicketOffice(trainDataService, bookingReferenceService);
            var seatsReservationAdapter = new SeatsReservationAdapter(ticketOffice);
            var jsonReservation = await seatsReservationAdapter.Reserve(_trainId.Id, seatsRequestedCount);

            Check.That(jsonReservation)
                .IsEqualTo($"{{\"train_id\": \"{_trainId}\", \"booking_reference\": \"{BookingReference}\", \"seats\": [\"1B\", \"2B\"]}}");
        }

        private static IBookingReferenceService BuildBookingReferenceService(string bookingReference)
        {
            var bookingReferenceService = Substitute.For<IBookingReferenceService>();
            bookingReferenceService.GetBookingReference().Returns(Task.FromResult(bookingReference));
            return bookingReferenceService;
        }

        private static ITrainDataService BuildTrainDataService(TrainId trainId, string trainTopology)
        {
            var trainDataService = Substitute.For<ITrainDataService>();
            trainDataService.GetTrain(trainId)
                .Returns(Task.FromResult(new Train(trainId, TrainDataServiceAdapter.AdaptTrainTopology(trainTopology))));
            return trainDataService;
        }
    }
}
