using System.Threading.Tasks;

namespace TrainTrain.Domain.Port
{
    public interface IProvideReservation
    {
        Task<Reservation> BookSeats(ReservationAttempt reservationAttempt);
    }
}