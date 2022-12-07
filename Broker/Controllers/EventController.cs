using Broker.Model;
using Broker.ServiceAPI;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Controllers
{
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [Route("Event/{id:Guid}")]
        public ActionResult<IEnumerable<Event>> Get(Guid id)
        {
            var resp = eventService.GetEvents(id);

            if (resp == null) return this.NotFound();

            return resp.ToArray();
        }

        [Route("Event")]
        [HttpPost]
        public ActionResult Post([FromBody] Event @event) 
        {
            var res = this.eventService.AddEvent(@event);

            if (res)
            {
                return Ok();
            }

            return this.BadRequest();
        }
    }
}
