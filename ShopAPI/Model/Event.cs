namespace ShopAPI.Model
{
    public class Event
    {
        public Guid EventId { get; set; }

        public string? EventName { get; set; }

        public DateTime PublishDate { get; set; }

        public int Queue { get; set; }

        public string? Params { get; set; }
    }
}
