using System.Text;

namespace HassanCehef.TrainDataService.Models
{
    public class  Seat
    {
        public string booking_reference { get; set; }
        public string seat_number { get; set; }
        public string coach { get; set; }

        public Seat(string coach, string seatNumber, string bookingReference)
        {
            booking_reference = bookingReference;
            seat_number = seatNumber;
            this.coach = coach;
        }
        
    }
}