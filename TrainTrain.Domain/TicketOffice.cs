using System.Threading.Tasks;
using TrainTrain.Domain.Port;

namespace TrainTrain.Domain
{
    public class TicketOffice : IProvideReservation
    {
        private readonly IProvideTrainTopology _provideTrainTopology;
        private readonly IBuildReservation _buildReservation;
        private readonly IProvideBookingReference _bookingReferenceService;

        public TicketOffice(IProvideTrainTopology provideTrainTopology, IBuildReservation buildReservation,
            IProvideBookingReference bookingReferenceService)
        {
            _provideTrainTopology = provideTrainTopology;
            _buildReservation = buildReservation;
            _bookingReferenceService = bookingReferenceService;
        }

        public async Task<Reservation> Reserve(TrainId trainId, SeatsRequested seatsRequested)
        {
            var train = await _provideTrainTopology.GetTrain(trainId);

            if (train.DoesNotExceedOverallCapacity(seatsRequested))
            {
                var reservationAttempt = train.BuildReservationAttempt(seatsRequested);

                if (reservationAttempt.IsFulFilled)
                {
                    var bookingReference = await _bookingReferenceService.GetBookingReference();

                    return await _buildReservation.BookSeats(
                        reservationAttempt.AssignBookingReference(bookingReference));
                }
            }

            return new ReservationFailure(trainId);
        }
    }
}