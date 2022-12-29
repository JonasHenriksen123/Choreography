
namespace ShopAPI.Model
{
    public class Account
    {
        public Guid AccountId { get; set; }

        public string? Name { get; set; }

        public decimal CreditAmount { get; set; }

        public decimal Credit { get; set; }

        public List<Invoice>? Invoices { get; set; }
    }
}
