using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrainTrain.Infra.Adapter;

namespace TrainTrain.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReservationsController(SeatsReservationAdapter seatsReservationAdapter) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get([FromQuery(Name = "trainId")] string trainId,
            [FromQuery(Name = "numberOfSeats")] int numberOfSeats)
        {
            return await seatsReservationAdapter.ReserveAsync(trainId, numberOfSeats);
        }
    }
}