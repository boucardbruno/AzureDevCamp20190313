using System.Threading.Tasks;

namespace TrainTrain.Domain.Port
{
    public interface IProvideTrainTicket
    {
        Task<Reservation> Reserve(TrainId trainId, SeatsRequested seatsRequested);
    }
}