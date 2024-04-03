using System.Threading.Tasks;
using NFluent;
using NSubstitute;
using NUnit.Framework;

namespace TrainReservationOffice.Test.Acceptance;

public class TrainTrainShould
{
    private readonly BookingReference _bookingReference = new("75bcd15");
    private readonly TrainId _trainId = new("9043-2019-03-13");

    [Test]
    public async Task Reserve_seat_when_train_is_empty()
    {
        var requestForSeats = new RequestForSeats(3);

        var trainDataService = BuildTrainDataService(_trainId, TrainTopologyGenerator
            .With_10_available_seats());
        
        var bookingReferenceService = BuildBookingReferenceService(_bookingReference);

        var ticketOffice = new TicketOffice(trainDataService, bookingReferenceService);
        var jsonReservation = await ticketOffice.Reserve(_trainId, requestForSeats);

        Check.That(SeatsReservationAdapter.AdaptReservation(jsonReservation))
            .IsEqualTo(
                $"{{\"train_id\": \"{_trainId.Id}\", \"booking_reference\": \"{_bookingReference.Id}\", \"seats\": [\"1A\", \"2A\", \"3A\"]}}");
    }

    [Test]
    public async Task Not_reserve_seats_when_it_exceed_max_capacity_threshold()
    {
        var requestForSeats = new RequestForSeats(2);

        var trainDataService =
            BuildTrainDataService(_trainId, TrainTopologyGenerator
                .With_10_seats_and_6_already_reserved());
        
        var bookingReferenceService = BuildBookingReferenceService(_bookingReference);

        var ticketOffice = new TicketOffice(trainDataService, bookingReferenceService);
        var jsonReservation = await ticketOffice.Reserve(_trainId, requestForSeats);

        Check.That(SeatsReservationAdapter.AdaptReservation(jsonReservation))
            .IsEqualTo($"{{\"train_id\": \"{_trainId.Id}\", \"booking_reference\": \"\", \"seats\": []}}");
    }

    [Test]
    [Ignore("Not fixed")]
    public async Task Reserve_all_seats_in_the_same_coach()
    {
        var requestForSeats = new RequestForSeats(2);

        var trainDataService = BuildTrainDataService(_trainId,
            TrainTopologyGenerator
                .With_2_coaches_and_9_seats_already_reserved_in_the_first_coach());
        
        var bookingReferenceService = BuildBookingReferenceService(_bookingReference);

        var ticketManager = new TicketOffice(trainDataService, bookingReferenceService);
        var jsonReservation = await ticketManager.Reserve(_trainId, requestForSeats);

        Check.That(SeatsReservationAdapter.AdaptReservation(jsonReservation))
            .IsEqualTo(
                $"{{\"train_id\": \"{_trainId.Id}\", \"booking_reference\": \"{_bookingReference.Id}\", \"seats\": [\"1B\", \"2B\"]}}");
    }

    private static IProvideBookingReference BuildBookingReferenceService(BookingReference bookingReference)
    {
        var bookingReferenceService = Substitute.For<IProvideBookingReference>();
        bookingReferenceService.GetBookingReference().Returns(Task.FromResult(bookingReference));
        return bookingReferenceService;
    }

    private static IProvideTrainData BuildTrainDataService(TrainId trainId, string trainTopology)
    {
        var trainDataService = Substitute.For<IProvideTrainData>();
        trainDataService.GetTrainById(trainId)
            .Returns(Task.FromResult(new Train(trainId, ProvideTrainDataAdapter.AdaptTrainTopology(trainTopology))));
        return trainDataService;
    }
}