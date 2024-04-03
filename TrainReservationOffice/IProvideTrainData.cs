using System.Threading.Tasks;

namespace TrainReservationOffice;

public interface IProvideTrainData
{
    Task<Train> GetTrainById(TrainId trainId);
    Task BookSeats(ReservationAttempt reservationAttempt);
}