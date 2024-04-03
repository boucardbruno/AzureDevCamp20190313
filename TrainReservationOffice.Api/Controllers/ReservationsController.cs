using Microsoft.AspNetCore.Mvc;

namespace TrainReservationOffice.Api.Controllers;

[Route("api/[controller]")]
public class ReservationsController : Controller
{
    private const string UriTrainDataService = "http://localhost:50680";
    private const string UriBookingReferenceService = "http://localhost:51691/";

    [HttpGet]
    public async Task<ActionResult<string>> Get([FromQuery(Name = "trainId")] string trainId,
        [FromQuery(Name = "numberOfSeats")] int numberOfSeats)
    {
        var ticketOffice = new TicketOffice(
            new ProvideTrainDataAdapter(UriTrainDataService),
            new ProvideBookingReferenceAdapter(UriBookingReferenceService));

        return SeatsReservationAdapter.AdaptReservation(
            await ticketOffice.Reserve(
                new TrainId(trainId),
                new RequestForSeats(numberOfSeats)));
    }
}