namespace Stock.Model
{
    public class OrderCreatedParams
    {
        public IEnumerable<OrderCreatedLines>? Items { get; set; }

        public class OrderCreatedLines
        {
            public Guid ItemId { get; set; }

            public int Amount { get; set; }
        }
    }
}
