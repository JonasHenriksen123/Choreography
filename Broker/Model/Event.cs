using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Broker.Model
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

        public DateTime PublishDate { get; set; }

        public int Queue { get; set; }

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
    }
}
