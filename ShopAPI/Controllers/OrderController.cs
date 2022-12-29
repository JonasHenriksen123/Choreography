using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using ShopAPI.ServiceAPI;

namespace ShopAPI.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IWebClient webClient;

        public OrderController(IWebClient webClient)
        {
            this.webClient = webClient;
        }

        [Route("Orders")]
        public async Task<ActionResult<Order[]>> Get()
        {
            var resp = await this.webClient.GetOrders();

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("Orders/{id:Guid}")]
        public async Task<ActionResult<Order>> Get(Guid id)
        {
            var resp = await this.webClient.GetOrder(id);

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("Orders/{id:Guid}/events")]
        public async Task<ActionResult<Event[]>> GetEvents(Guid id)
        {
            var resp = await this.webClient.GetEventsForOrder(id);

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("Orders/{id:guid}/orderlines")]
        public async Task<ActionResult<OrderLine[]>> GetOrderLines(Guid id)
        {
            var resp = await this.webClient.GetOrderLinesForOrder(id);

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("Orders")]
        [HttpPost]
        public async Task<ActionResult<Order>> Post([FromBody] Order order)
        {
            var resp = await this.webClient.CreateOrder(order);

            if (resp == null) return this.BadRequest();

            return resp;
        }

        [Route("Orders/{id:Guid}/Confirm")]
        [HttpGet]
        public async Task<ActionResult> ConfirmOrder(Guid id)
        {
            var resp = await this.webClient.ConfirmOrder(id);

            if (resp == null) return this.NotFound();

            return Ok();
        }
    }
}
