namespace TrainTrain
{
    public class Seat(string coachName, int seatNumber, string bookingRef)
    {
        public string CoachName { get; } = coachName;
        public int SeatNumber { get; } = seatNumber;
        public string BookingRef { get; set;  } = bookingRef;

        public bool IsAvailable()
        {
            return BookingRef == "";
        }

        public override string ToString()
        {
            return $"\"{SeatNumber}{CoachName}\"";
        }
    }
}