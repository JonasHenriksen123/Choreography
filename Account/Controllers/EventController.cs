using Microsoft.AspNetCore.Mvc;
using Accounts.Model;
using Accounts.ServiceAPI;

namespace Accounts.Controllers
{
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventHandlerService eventHandlerService;

        public EventController(IEventHandlerService eventHandlerService)
        {
            this.eventHandlerService = eventHandlerService;
        }

        [Route("Event")]
        [HttpPost]
        public ActionResult Post([FromBody] Event @event)
        {
            this.eventHandlerService.EnqueueEvent(@event).Wait();

            return Ok();
        }
    }
}
