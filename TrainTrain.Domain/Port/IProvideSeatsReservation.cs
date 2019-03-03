using System.Threading.Tasks;

namespace TrainTrain.Domain.Port
{
    public interface IProvideSeatsReservation
    {
        Task<Reservation> Reserve(TrainId trainId, SeatsRequested seatsRequested);
    }
}