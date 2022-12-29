namespace ShopAPI.Model
{
    public class Item : IPartialItem
    {
        public Guid ItemId { get; set; }

        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public int Amount { get; set; }
    }
}
