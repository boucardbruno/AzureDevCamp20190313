using System.Threading.Tasks;

namespace TrainTrain.Domain.Port
{
    public interface IBuildReservation
    {
        Task<Reservation> BookSeats(ReservationAttempt reservationAttempt);
    }
}