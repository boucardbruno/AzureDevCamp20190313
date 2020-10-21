using System.Linq;
using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using TechTalk.SpecFlow;
using TrainTrain.Domain;
using TrainTrain.Domain.Port;
using TrainTrain.Infra.Adapter;
using TrainTrain.Test.Acceptance;

namespace TrainTrain.Test.StepDefinition
{
    [Binding]
    public class TrainReservationSteps
    {
        private readonly TrainId _trainId = new TrainId("9043-2019-03-13");
        private readonly BookingReference _bookingReference = new BookingReference("341RTFA");
        private IProvideTicket _ticketOffice;
        private Reservation _reservation;

        [Given(@"one coach empty made of (.*) seats")]
        public void GivenOneCoachEmptyMadeOfSeats(int availableSeats)
        {
            var withNAvailableSeats = TrainTopologyGenerator.With_n_seats_and_m_already_reserved(availableSeats, 1);
            var provideTrainTopology = BuildTrainTopology(_trainId, withNAvailableSeats);
            var bookingReferenceService = BuildBookingReferenceService(_bookingReference);
            var provideReservation = BuildMakeReservation(_trainId, _bookingReference, new Seat("A", 1), new Seat("A", 2), new Seat("A", 3));

            _ticketOffice = new TicketOfficeService(provideTrainTopology, provideReservation, bookingReferenceService);

        }

        [Given(@"(.*) coaches of (.*) seats and (.*) already reserved")]
        public void GivenCoachesOfSeatsAndAlreadyReserved(int coachesCount, int seatsCount, int alreadyReservedSeats)
        {
            var provideTrainTopology = BuildTrainTopology(_trainId, TrainTopologyGenerator.With_n_seats_and_m_already_reserved(seatsCount, coachesCount, alreadyReservedSeats, _bookingReference.Id));
            var bookingReferenceService = BuildBookingReferenceService(_bookingReference);
            var provideReservation = BuildMakeReservation(_trainId, _bookingReference, new Seat("A", 1), new Seat("A", 2), new Seat("A", 3));

            _ticketOffice = new TicketOfficeService(provideTrainTopology, provideReservation, bookingReferenceService);
        }

        [Given(@"(.*) coaches of (.*) seats and (.*) seats already reserved in the coach (.*)")]
        public void GivenCoachesOfSeatsAndSeatsAlreadyReservedInTheCoach(int coachesCount, int seatsCount, int alreadyReservedSeats, int coachNumber)
        {
            var withNSeatsAndMAlreadyReserved = TrainTopologyGenerator.With_n_seats_and_m_already_reserved(seatsCount, coachesCount, alreadyReservedSeats, _bookingReference.Id, coachNumber);   
            var provideTrainTopology = BuildTrainTopology(_trainId,
                withNSeatsAndMAlreadyReserved);
            var provideReservation = BuildMakeReservation(_trainId, _bookingReference, new Seat("B", 1), new Seat("B", 2));
            var bookingReferenceService = BuildBookingReferenceService(_bookingReference);

            _ticketOffice = new TicketOfficeService(provideTrainTopology, provideReservation, bookingReferenceService);

        }

        [When(@"(.*) seats are requested")]
        public async Task WhenSeatsAreRequested(int seatsCountRequested)
        {
            _reservation = await _ticketOffice.Reserve(_trainId, new SeatsRequested(seatsCountRequested));
        }

        [Then(@"the reservation should be assigned these seats ""(.*)""")]
        public void ThenTheReservationShouldBeAssignedTheseSeats(string expectedSeats)
        {
            var seatsAsString = string.Join(", ", expectedSeats.Split(",").Select(s => $"\"{s.Trim()}\""));
            Check.That(SeatsReservationAdapter.AdaptReservation(_reservation))
                .IsEqualTo(
                    $"{{\"train_id\": \"{_trainId}\", \"booking_reference\": \"{_bookingReference}\", \"seats\": [{ seatsAsString}]}}");
        }

        [Then(@"the reservation should be failed")]
        public void ThenTheReservationShouldBeFailed()
        {
            Check.That(SeatsReservationAdapter.AdaptReservation(_reservation))
                .IsEqualTo($"{{\"train_id\": \"{_trainId}\", \"booking_reference\": \"\", \"seats\": []}}");
        }

        private static IProvideBookingReference BuildBookingReferenceService(BookingReference bookingReference)
        {
            var bookingReferenceService = Substitute.For<IProvideBookingReference>();
            bookingReferenceService.GetBookingReference().Returns(Task.FromResult(bookingReference));
            return bookingReferenceService;
        }

        private static IProvideTrainTopology BuildTrainTopology(TrainId trainId, string trainTopology)
        {
            var trainDataService = Substitute.For<IProvideTrainTopology>();
            trainDataService.GetTrain(trainId)
                .Returns(Task.FromResult(new Train(trainId,
                    TrainDataServiceAdapter.AdaptTrainTopology(trainTopology))));

            return trainDataService;
        }

        private static IProvideReservation BuildMakeReservation(TrainId trainId,
            BookingReference bookingReference, params Seat[] seats)
        {
            var trainDataService = Substitute.For<IProvideReservation>();

            trainDataService.BookSeats(Arg.Any<ReservationAttempt>())
                .Returns(Task.FromResult(new Reservation(trainId, bookingReference, seats)));

            return trainDataService;
        }
    }
}
