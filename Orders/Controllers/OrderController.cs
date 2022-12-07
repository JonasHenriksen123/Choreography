using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Model;
using Orders.ServiceAPI;

namespace Orders.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DatabaseContext dbContext;
        private readonly IOrderService orderService;

        public OrderController(DatabaseContext dbContext, IOrderService orderService)
        {
            this.dbContext = dbContext;
            this.orderService = orderService;
        }

        [Route("Order")]
        public ActionResult<Order[]> Get() 
        {
            var resp = dbContext.Orders
                .OrderBy(o => o.OrderId)
                .ToArray();

            if (resp == null || !resp.Any()) return this.NotFound();

            return resp;
        }

        [Route("Order/{id:Guid}")]
        public ActionResult<Order> Get(Guid id)
        {
            var resp = dbContext.Orders
                .SingleOrDefault(o => o.OrderId== id);

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("Order/{id:Guid/full}")]
        [HttpGet]
        public ActionResult<Order> GetFull(Guid id)
        {
            var resp = dbContext.Orders
                .Include(o => o.Lines)
                .Where(o => o.OrderId == id)
                .SingleOrDefault();

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("Order")]
        [HttpPost]
        public ActionResult<Order> Post([FromBody] Order order) {
            var res = this.orderService.CreateOrder(order);

            if (res == null) return this.BadRequest();

            return res;
        }

        [Route("Order/{id:Guid}/Confirm")]
        [HttpPost]
        public async Task<ActionResult> PostConfirm(Guid id)
        {
            var resp = await this.orderService.ConfirmOrder(id);

            if (!resp) return this.BadRequest();

            return this.Ok();
        }
    }
}
