
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TrainTrain.Api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    public class ReservationsController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get([FromQuery(Name = "trainId")] string trainId,
            [FromQuery(Name = "numberOfSeats")] int numberOfSeats)
        {
            var manager = new WebTicketManager();
            return await manager.Reserve(trainId, numberOfSeats);
        }
    }
}
