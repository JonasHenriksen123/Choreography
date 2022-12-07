using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using ShopAPI.ServiceAPI;

namespace ShopAPI.Controllers
{
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IWebClient webClient;

        public AccountsController(IWebClient webClient)
        {
            this.webClient = webClient;
        }

        [Route("accounts")]
        public async Task<ActionResult<Account[]>> Get()
        {
            var resp = await this.webClient.GetAllAccounts();

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("accounts/{id:int}")]
        public async Task<ActionResult<Account>> Get(int id)
        {
            var resp = await this.webClient.GetAccount(id);

            if (resp == null) return this.NotFound();

            return resp;
        }
    }
}
