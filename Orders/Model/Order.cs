using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Orders.Model
{
    public class Order
    {

        public enum StateEnum
        {
            New = -1,
            Pending = 0,
            PendingPayment = 1,
            Created = 2
        }

        public Guid OrderId { get; set; }

        public decimal Amount { get; set; }

        public int ItemCount { get; set; }

        public Guid? AccountId { get; set; }

        public int State { get; set; }

        [NotMapped]
        public StateEnum OrderState
        {
            get
            {
                return State switch
                {
                    (int)StateEnum.Pending => StateEnum.Pending,
                    (int)StateEnum.Created => StateEnum.Created,
                    (int)StateEnum.PendingPayment => StateEnum.PendingPayment,
                    (int)StateEnum.New => StateEnum.New,
                    _ => throw new Exception("Unknown"),
                };
            }
            set
            {
                this.State = value switch
                {
                    StateEnum.Pending => (int)StateEnum.Pending,
                    StateEnum.Created => (int)StateEnum.Created,
                    StateEnum.PendingPayment => (int)StateEnum.PendingPayment,
                    StateEnum.New => throw new Exception("Not allowed"),
                    _ => throw new Exception("Unknown"),
                };
            }
        }

        public List<OrderLine>? Lines { get; set; }

        public string? GenerateOrderCreatedParams()
        {
            if (Lines == null || !Lines.Any()) return null;

            var res = new
            {
                items = Lines.Select(l => new { itemId = l.ItemId, amount = l.Count })
            };

            return JsonSerializer.Serialize(res);
        }
    }
}
