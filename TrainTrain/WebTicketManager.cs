using System.Threading.Tasks;

namespace TrainTrain
{
    public class WebTicketManager
    {
        private const string UriTrainDataService = "http://localhost:50680";
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private readonly IBookingReferenceService _bookingReferenceService;
        private readonly ITrainDataService _trainDataService;

        public WebTicketManager() : this(new TrainDataServiceAdapter(UriTrainDataService),
            new BookingReferenceServiceAdapter(UriBookingReferenceService))
        {
        }

        public WebTicketManager(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {
            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
        }

        public async Task<Reservation> Reserve(string trainId, int seatsRequestedCount)
        {
            var train = await _trainDataService.GetTrain(trainId);

            if (train.DoesNotExceedOverallTrainCapacity(seatsRequestedCount))
            {
                var reservationAttempt = train.BuildReservationAttempt(seatsRequestedCount);

                if (reservationAttempt.IsFulFilled)
                {
                    var bookingReference = await _bookingReferenceService.GetBookingReference();
                    var assignBookingReference = reservationAttempt.AssignBookingReference(bookingReference);
                    await _trainDataService.BookSeats(assignBookingReference);
                    return assignBookingReference.Confirm();
                }
            }

            return new ReservationFailure(trainId);
        }
    }
}