using Microsoft.AspNetCore.Mvc;
using Stock.Model;

namespace Stock.Controllers
{
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly DatabaseContext dbContext;

        public ItemController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Route("Item")]
        public ActionResult<Item[]> Get()
        {
            var resp = this.dbContext.Items
                .OrderBy(i => i.ItemId)
                .ToArray();

            if (resp == null || !resp.Any()) return this.NotFound();

            return resp;
        }

        [Route("Item/{id:Guid}")]
        public ActionResult<Item> Get(Guid id)
        {
            var resp = this.dbContext.Items
                .SingleOrDefault(i => i.ItemId == id);

            if (resp == null) return this.NotFound();

            return resp;
        }
    }
}
