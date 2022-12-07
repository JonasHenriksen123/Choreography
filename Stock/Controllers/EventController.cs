﻿using Microsoft.AspNetCore.Mvc;
using Stock.Model;
using Stock.ServiceAPI;

namespace Stock.Controllers
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
