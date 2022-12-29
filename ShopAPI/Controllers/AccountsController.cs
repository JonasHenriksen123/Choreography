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

        [Route("accounts/{id:Guid}")]
        public async Task<ActionResult<Account>> Get(Guid id)
        {
            var resp = await this.webClient.GetAccount(id);

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("accounts/{id:Guid}/invoices")]
        public async Task<ActionResult<Invoice[]>> GetInvoices(Guid id)
        {
            var resp = await this.webClient.GetInvoicesForAccount(id);

            if (resp == null) return this.NotFound();

            return resp;
        }

        [Route("Accounts")]
        [HttpPost]
        public async Task<ActionResult<Account>> Post([FromBody] Account account)
        {
            var resp = await this.webClient.CreateAccount(account);

            if (resp == null) return this.BadRequest();

            return resp;
        }
    }
}
