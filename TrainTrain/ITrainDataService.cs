using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrainTrain
{
    public interface ITrainDataService
    {
        Task<Train> GetTrainId(string trainId);
        Task BookSeats(ReservationAttempt reservationAttempt);
    }
}