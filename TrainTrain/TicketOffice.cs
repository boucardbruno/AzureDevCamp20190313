using System.Threading.Tasks;

namespace TrainTrain
{
    public class TicketOffice(ITrainDataService trainRepository, IBookingReferenceService bookingReferenceRepository)
    {
        private const string UriTrainDataService = "http://localhost:50680";
        private const string UriBookingReferenceService = "http://localhost:51691/";

        public TicketOffice():this(new TrainDataServiceAdapter(UriTrainDataService), new BookingReferenceServiceAdapter(UriBookingReferenceService))
        {
        }

        public async Task<Reservation> Reserve(string trainId, int seatsRequestedCount)
        {
            var train = await trainRepository.FindTrainById(trainId);

            if (train.DoesNotExceedOverallTrainCapacity(seatsRequestedCount))
            {
                var reservationAttempt = train.BuildReservationAttempt(seatsRequestedCount);

                if (reservationAttempt.IsFulfilled())
                {
                    var bookingReference = await bookingReferenceRepository.GetBookingReference();

                    reservationAttempt.AssignBookingReference(bookingReference);

                    await trainRepository.BookSeats(reservationAttempt);

                    return reservationAttempt.Confirm();
                }
            }
            return new ReservationFailure(trainId);
        }
    }
}