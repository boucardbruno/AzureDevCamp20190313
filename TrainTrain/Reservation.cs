using System.Collections.Generic;

namespace TrainTrain
{
    public class Reservation(string trainId, string bookingReference, IEnumerable<Seat> seats)
    {
        public string TrainId { get; } = trainId;
        public string BookingReference { get; } = bookingReference;
        public IEnumerable<Seat> Seats { get; } = seats;
    }
}