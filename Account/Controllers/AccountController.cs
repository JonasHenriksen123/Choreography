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
    }
}
