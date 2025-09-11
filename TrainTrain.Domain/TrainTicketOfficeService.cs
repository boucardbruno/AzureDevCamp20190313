using System.Threading.Tasks;
using TrainTrain.Domain.Port;

namespace TrainTrain.Domain
{
    public class TrainTicketOfficeService(
        IProvideTrainTopology trainRepository,
        IProvideReservation reservationRepository,
        IProvideBookingReference bookingReferenceRepository)
        : IProvideTrainTicket
    {
        public async Task<Reservation> Reserve(TrainId trainId, SeatsRequested seatsRequested)
        {
            var train = await trainRepository.GetTrainBy(trainId);

            if (train.DoesNotExceedOverallCapacity(seatsRequested))
            {
                var reservationAttempt = train.BuildReservationAttempt(seatsRequested);

                if (reservationAttempt.IsFulFilled)
                {
                    return await reservationRepository.BookSeats(
                        reservationAttempt
                            .AssignBookingReference(await bookingReferenceRepository.GetBookingReference()));
                }
            }
            return new ReservationFailure(trainId);
        }
    }
}