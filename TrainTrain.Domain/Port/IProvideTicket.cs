using System.Threading.Tasks;

namespace TrainTrain.Domain.Port
{
    public interface IProvideTicket
    {
        Task<Reservation> Reserve(TrainId trainId, SeatsRequested seatsRequested);
    }
}