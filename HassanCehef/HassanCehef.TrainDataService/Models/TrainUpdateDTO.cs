using System.Collections.Generic;

namespace HassanCehef.TrainDataService.Models
{
    public class TrainUpdateDTO
    {
        public TrainUpdateDTO(string trainId, List<string> seats, string bookingReference)
        {
            train_id = trainId;
            this.seats = seats;
            booking_reference = bookingReference;
        }
        public string train_id { get; set; }
        public List<string> seats { get; set; }
        public string booking_reference { get; set; }
    }
}