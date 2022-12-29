namespace ShopAPI.Model
{
    public class OrderLine
    {
        public Guid OrderLineId { get; set; }

        public Guid OrderId { get; set; }

        public Guid ItemId { get; set; }

        public int Count { get; set; }

        public decimal Amount { get; set; }

        public decimal Total { get; set; }

        public Order? Order { get; set; }
    }
}
