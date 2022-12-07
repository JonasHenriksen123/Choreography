
using Accounts.Model;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Controllers
{
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly DatabaseContext dbContext;

        public InvoiceController(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Route("Invoice")]
        public ActionResult<Invoice[]> Get()
        {
            var resp = this.dbContext.Invoices
                .OrderBy(i => i.InvoiceId)
                .ToArray();

            if (resp == null || !resp.Any()) return this.NotFound();

            return resp;
        }

        [Route("Invoice/{id:Guid}")]
        public ActionResult<Invoice> Get(Guid id)
        {
            var resp = this.dbContext.Invoices
                .SingleOrDefault(i => i.InvoiceId== id);

            if (resp == null) return this.NotFound();

            return resp;
        }
    }
}
