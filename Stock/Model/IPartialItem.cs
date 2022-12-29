namespace Stock.Model
{
    public interface IPartialItem
    {
        public Guid ItemId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}
