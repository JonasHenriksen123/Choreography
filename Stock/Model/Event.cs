using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Stock.Model
{
    public class Event
    {
        public enum QueueEnum
        {
            AccountsQueue = 0,
            OrderQueue = 1,
            StockQueue = 2
        }

        public Guid EventId { get; set; }

        public string? EventName { get; set; }


        [NotMapped]
        public DateTime PublishDate { get; set; }

        public int Queue { get; set; }

        [NotMapped]
        public string? Params { get; set; }

        [NotMapped]
        public QueueEnum EventQueue
        {
            get
            {
                return Queue switch
                {
                    (int)QueueEnum.AccountsQueue => QueueEnum.AccountsQueue,
                    (int)QueueEnum.OrderQueue => QueueEnum.OrderQueue,
                    (int)QueueEnum.StockQueue => QueueEnum.StockQueue,
                    _ => throw new Exception("Unknown"),
                };
            }

            set
            {
                Queue = value switch
                {
                    QueueEnum.AccountsQueue => (int)QueueEnum.AccountsQueue,
                    QueueEnum.StockQueue => (int)QueueEnum.StockQueue,
                    QueueEnum.OrderQueue => (int)QueueEnum.OrderQueue,
                    _ => throw new Exception("Unknown"),
                };
            }
        }

        public OrderCreatedParams? FetchOrderCreatedParams()
        {
            if (String.IsNullOrEmpty(this.Params)) return null;

            return JsonSerializer.Deserialize<OrderCreatedParams>(this.Params, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
