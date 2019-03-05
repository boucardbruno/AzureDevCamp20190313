using System.Threading.Tasks;
using TrainTrain.Domain.Port;

namespace TrainTrain.Domain
{
    public class TicketOffice : IProvideSeatsReservation
    {
        private readonly ITrainDataService _trainDataService;
        private readonly IProvideBookingReference _bookingReferenceService;

        public TicketOffice(ITrainDataService trainDataService, IProvideBookingReference bookingReferenceService)
        {
            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
        }

        public async Task<Reservation> Reserve(TrainId trainId, SeatsRequested seatsRequested)
        {
            var train = await _trainDataService.GetTrain(trainId);

            if (train.DoesNotExceedOverallTrainCapacity(seatsRequested))
            {
                var reservationAttempt = train.BuildReservationAttempt(seatsRequested);

                if (reservationAttempt.IsFulFilled)
                {
                    var bookingReference = await _bookingReferenceService.GetBookingReference();

                    return await _trainDataService.BookSeats(
                        reservationAttempt.AssignBookingReference(bookingReference));
                }
            }

            return new ReservationFailure(trainId);
        }
    }
}