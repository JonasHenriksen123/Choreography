using Microsoft.AspNetCore.Mvc;
using Orders.Model;

namespace Orders.Controllers
{
    [ApiController]
    public class OrderLineController : ControllerBase
    {
        private readonly DatabaseContext dbContext;

        public OrderLineController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Route("OrderLine/Order/{id:Guid}")]
        public ActionResult<OrderLine[]> GetOrder(Guid id)
        {
            var resp = this.dbContext.OrderLines
                .Where(o => o.OrderId== id)
                .OrderBy(o => o.OrderLineId)
                .ToArray();

            if (resp == null || !resp.Any()) return this.NotFound();

            return resp;
        }

        [Route("OrderLine/{id:Guid}")]
        public ActionResult<OrderLine> Get(Guid id)
        {
            var resp = this.dbContext.OrderLines
                .SingleOrDefault(o => o.OrderLineId == id);
            
            if (resp == null) return this.NotFound();

            return resp;
        }
    }
}
