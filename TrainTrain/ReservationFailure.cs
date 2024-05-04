using System.Collections.Generic;

namespace TrainTrain
{
    public class ReservationFailure(string trainId) : Reservation(trainId, string.Empty, new List<Seat>());
}