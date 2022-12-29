using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using ShopAPI.ServiceAPI;

namespace ShopAPI.Controllers
{
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IWebClient webClient;

        public ItemController(IWebClient webClient)
        {
            this.webClient = webClient;
        }

        [Route("Items")]
        public async Task<ActionResult<IPartialItem[]>> Get()
        {
            var resp = await this.webClient.GetAllItems();

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("Items/{id:Guid}")]
        public async Task<ActionResult<Item>> Get(Guid id)
        {
            var resp = await this.webClient.GetItem(id);

            if (resp == null)  return this.NotFound();

            return resp;
        }

        [Route("Items")]
        [HttpPost]
        public async Task<ActionResult<IPartialItem>> Post([FromBody] Item item)
        {
            var resp = await this.webClient.CreateItem(item);

            if (resp == null) return this.BadRequest();

            return resp;
        }
    }
}
