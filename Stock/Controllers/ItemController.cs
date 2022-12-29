using Microsoft.AspNetCore.Mvc;
using Stock.Model;
using Stock.ServiceAPI;

namespace Stock.Controllers
{
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly DatabaseContext dbContext;
        private readonly IItemService itemService;

        public ItemController(DatabaseContext dbContext, IItemService itemService)
        {
            this.dbContext = dbContext;
            this.itemService = itemService;
        }

        [Route("Item")]
        public ActionResult<IPartialItem[]> Get()
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

        [Route("Item")]
        [HttpPost]
        public ActionResult<Item> Post([FromBody] Item item) 
        {
            var resp = this.itemService.CreateItem(item);

            if (resp == null) return this.BadRequest();

            return resp;
        }
    }
}
