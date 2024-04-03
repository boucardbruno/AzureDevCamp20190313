namespace TrainReservationOffice.Api.Models;

public class ReservationRequestDto
{
    public ReservationRequestDto(string trainId, int numberOfSeats)
    {
        train_id = trainId;
        number_of_seats = numberOfSeats;
    }

    public string train_id { get; set; }
    public int number_of_seats { get; set; }
}