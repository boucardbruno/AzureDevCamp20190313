using System.Threading.Tasks;
using TrainTrain.Domain.Port;

namespace TrainTrain.Domain
{
    public class TicketOffice : IProvideTicket
    {
        private readonly IProvideTrainTopology _provideTrainTopology;
        private readonly IProvideReservation _provideReservation;
        private readonly IProvideBookingReference _provideBookingReference;

        public TicketOffice(IProvideTrainTopology provideTrainTopology, IProvideReservation provideReservation,
            IProvideBookingReference provideBookingReference)
        {
            _provideTrainTopology = provideTrainTopology;
            _provideReservation = provideReservation;
            _provideBookingReference = provideBookingReference;
        }

        public async Task<Reservation> Reserve(TrainId trainId, SeatsRequested seatsRequested)
        {
            var train = await _provideTrainTopology.GetTrain(trainId);

            if (train.DoesNotExceedOverallCapacity(seatsRequested))
            {
                var reservationAttempt = train.BuildReservationAttempt(seatsRequested);

                if (reservationAttempt.IsFulFilled)
                {
                    var bookingReference = await _provideBookingReference.GetBookingReference();

                    return await _provideReservation.BookSeats(
                        reservationAttempt.AssignBookingReference(bookingReference));
                }
            }

            return new ReservationFailure(trainId);
        }
    }
}