using Accounts.Model;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DatabaseContext dbContext;

        public AccountController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Route("Account")]
        public ActionResult<Account[]> Get() 
        {
            var resp = this.dbContext.Accounts
                .OrderBy(a => a.Name)
                .ToArray();

            if (resp == null || !resp.Any()) return this.NotFound();

            return resp;
        }

        [Route("Account/{id:Guid}")]
        public ActionResult<Account> Get(Guid id)
        {
            var resp = this.dbContext.Accounts
                .SingleOrDefault(a => a.AccountId== id);

            if (resp == null) return this.NotFound();
            
            return resp;
        }

        [Route("Account")]
        [HttpPost]
        public ActionResult<Account> Post([FromBody] Account account)
        {
            if (account == null) return this.BadRequest();

            account.Credit = account.CreditAmount;
            account.AccountId = Guid.NewGuid();

            this.dbContext.Accounts.Add(account);
            this.dbContext.SaveChanges();

            return account;
        }


        [Route("Account/{id:Guid}/invoices")]
        public ActionResult<Invoice[]> GetInvoices(Guid id)
        {
            var resp = this.dbContext.Invoices
                .Where(i => i.AccountId == id)
                .ToArray();

            if (resp == null || !resp.Any()) return this.NotFound();

            return resp;
        }
    }
}
