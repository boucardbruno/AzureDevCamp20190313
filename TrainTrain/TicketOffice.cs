using System.Threading.Tasks;

namespace TrainTrain
{
    public class TicketOffice(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
    {
        private const string UriTrainDataService = "http://localhost:50680";
        private const string UriBookingReferenceService = "http://localhost:51691/";

        public TicketOffice():this(new TrainDataServiceAdapter(UriTrainDataService), new BookingReferenceServiceAdapter(UriBookingReferenceService))
        {
            
        }

        public async Task<Reservation> Reserve(string trainId, int seatsRequestedCount)
        {
            var train = await trainDataService.GetTrainId(trainId);

            if (train.DoesNotExceedOverallTrainCapacity(seatsRequestedCount))
            {
                var reservationAttempt = train.BuildReservationAttempt(seatsRequestedCount);

                if (reservationAttempt.IsFulfilled())
                {
                    var bookingReference = await bookingReferenceService.GetBookingReference();

                    reservationAttempt.AssignBookingReference(bookingReference);

                    await trainDataService.BookSeats(reservationAttempt);

                    return reservationAttempt.Confirm();
                }
            }
            return new ReservationFailure(trainId);
        }
    }
}