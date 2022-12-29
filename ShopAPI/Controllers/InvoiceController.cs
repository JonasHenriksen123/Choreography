using Microsoft.AspNetCore.Mvc;
using ShopAPI.Model;
using ShopAPI.ServiceAPI;

namespace ShopAPI.Controllers
{
    [ApiController]
    public class InvoiceController :ControllerBase
    {
        private readonly IWebClient webClient;

        public InvoiceController(IWebClient webClient)
        {
            this.webClient = webClient;
        }

        [Route("Invoices/{id:Guid}")]
        public async Task<ActionResult<Invoice>> Get(Guid id)
        {
            var resp = await this.webClient.GetInvoice(id);

            if (resp == null) return this.NotFound();

            return resp;
        }
    }
}
