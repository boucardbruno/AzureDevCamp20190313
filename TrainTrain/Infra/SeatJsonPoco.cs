namespace TrainTrain.Infra
{
    public class SeatJsonPoco
    {
        public SeatJsonPoco(string bookingReference, string seatNumber, string coach)
        {
            booking_reference = bookingReference;
            seat_number = seatNumber;
            this.coach = coach;
        }
        public string booking_reference { get; set; }
        public string seat_number { get; set; }
        public string coach { get; set; }
    }
}