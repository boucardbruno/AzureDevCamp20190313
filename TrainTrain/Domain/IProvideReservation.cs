using System.Threading.Tasks;

namespace TrainTrain.Domain
{
    public interface IProvideReservation
    {
        Task<Reservation> Reserve(TrainId trainId, SeatsRequested seatsRequestedCount);
    }
}