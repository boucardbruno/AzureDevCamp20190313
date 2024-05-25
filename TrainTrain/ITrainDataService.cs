using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrainTrain
{
    public interface ITrainDataService
    {
        Task<Train> FindTrainById(string trainId);
        Task BookSeats(ReservationAttempt reservationAttempt);
    }
}