using System.ComponentModel.DataAnnotations.Schema;

namespace Accounts.Model
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

        public StateEnum OrderState
        {
            get
            {
                return State switch
                {
                    (int)StateEnum.Pending => StateEnum.Pending,
                    (int)StateEnum.Created => StateEnum.Created,
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
                    StateEnum.New => throw new Exception("Not allowed"),
                    _ => throw new Exception("Unknown"),
                };
            }
        }

        public Invoice? GenerateInvoice()
        {
            if (this.Amount < 0)
            {
                return null;
            }

            return new Invoice
            {
                AccountId = this.AccountId ?? throw new Exception("This order has no attached account, not allowed"),
                InvoiceId = Guid.NewGuid(),
                OrderId = this.OrderId,
                InvoiceState = Invoice.StateEnum.New,
                Amount = this.Amount,
            };
        }
    }
}
