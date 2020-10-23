using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using TechTalk.SpecFlow;
using TrainTrain.Domain;
using TrainTrain.Domain.Port;
using TrainTrain.Infra.Adapter;
using TrainTrain.Test.Acceptance;

namespace TrainTrain.Test.Specification.StepDefinition
{
    [Binding]
    public class TrainReservationSteps
    {
        private readonly BookingReference _bookingReference = new BookingReference("341RTFA");
        private readonly TrainId _trainId = new TrainId("9043-2019-03-13");

        private IProvideTicket TicketOffice { get; set; }
        private Reservation Reservation { get; set; }

        [Given(@"one coach empty made of (.*) seats")]
        public void GivenOneCoachEmptyMadeOfSeats(int availableSeats)
        {
            var expectedSeats = "A1, A2, A3";
            var provideTrainTopology = BuildTrainTopology(_trainId,
                TrainTopologyGenerator.With_n_seats_and_m_already_reserved(availableSeats, 1));
            var bookingReferenceService = BuildBookingReferenceService(_bookingReference);
            var provideReservationService = BuildMakeReservation(_trainId, _bookingReference, expectedSeats);

            TicketOffice =
                new TicketOfficeService(provideTrainTopology, provideReservationService, bookingReferenceService);
        }

        [Given(@"(.*) coaches of (.*) seats and (.*) already reserved")]
        public void GivenCoachesOfSeatsAndAlreadyReserved(int coachesCount, int seatsCount, int alreadyReservedSeats)
        {
            var trainTopology = BuildTrainTopology(_trainId,
                TrainTopologyGenerator.With_n_seats_and_m_already_reserved(seatsCount, coachesCount,
                    alreadyReservedSeats, _bookingReference.Id));
            var bookingReferenceService = BuildBookingReferenceService(_bookingReference);
            var provideReservationService = BuildMakeReservation(_trainId, _bookingReference, "A1, A2, A3");

            TicketOffice = new TicketOfficeService(trainTopology, provideReservationService, bookingReferenceService);
        }

        [Given(@"(.*) coaches of (.*) seats and (.*) seats already reserved in the coach (.*)")]
        public void GivenCoachesOfSeatsAndSeatsAlreadyReservedInTheCoach(int coachesCount, int seatsCount,
            int alreadyReservedSeats, int coachNumber)
        {
            var provideTrainTopology = BuildTrainTopology(_trainId,
                TrainTopologyGenerator.With_n_seats_and_m_already_reserved(seatsCount, coachesCount,
                    alreadyReservedSeats, _bookingReference.Id, coachNumber));
            var provideReservationService =
                BuildMakeReservation(_trainId, _bookingReference, "B1, B2");
            var bookingReferenceService = BuildBookingReferenceService(_bookingReference);

            TicketOffice =
                new TicketOfficeService(provideTrainTopology, provideReservationService, bookingReferenceService);
        }

        [When(@"(.*) seats are requested")]
        public async Task WhenSeatsAreRequested(int seatsCountRequested)
        {
            Reservation = await TicketOffice.Reserve(_trainId, new SeatsRequested(seatsCountRequested));
        }

        [Then(@"the reservation should be assigned these seats ""(.*)""")]
        public void ThenTheReservationShouldBeAssignedTheseSeats(string expectedSeats)
        {
            var seatsAsString = string.Join(", ", expectedSeats.Split(",").Select(s => $"\"{s.Trim()}\""));
            Check.That(SeatsReservationAdapter.AdaptReservation(Reservation))
                .IsEqualTo(
                    $"{{\"train_id\": \"{_trainId}\", \"booking_reference\": \"{_bookingReference}\", \"seats\": [{seatsAsString}]}}");
        }

        [Then(@"the reservation should be failed")]
        public void ThenTheReservationShouldBeFailed()
        {
            Check.That(SeatsReservationAdapter.AdaptReservation(Reservation))
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
            BookingReference bookingReference, string expectedSeats)
        {
            var trainDataService = Substitute.For<IProvideReservation>();

            trainDataService.BookSeats(Arg.Any<ReservationAttempt>())
                .Returns(new Reservation(trainId, bookingReference, ToSeats(expectedSeats)));

            return trainDataService;
        }

        private static IEnumerable<Seat> ToSeats(string expectedSeats)
        {
            var seats = new List<Seat>();
            foreach (var seatName in expectedSeats.Split(','))
            {
                var (coachName, seatNumber) = ToSeat(seatName.Trim());
                seats.Add(new Seat(coachName, seatNumber));
            }
            return seats;
        }

        private static (string coachName, int seatNumber) ToSeat(string seatAsString)
        {
            return (seatAsString[0].ToString(), Convert.ToInt32(seatAsString.Substring(1)));
        }
    }
}