namespace ShopAPI.Model
{
    public class Invoice
    {
        public enum StateEnum
        {
            New = 0,
            Payed = 1
        }

        public Guid InvoiceId { get; set; }

        public Guid AccountId { get; set; }

        public Account? Account { get; set; }

        public decimal? Amount { get; set; }

        public Guid OrderId { get; set; }

        public int State { get; set; }

        public StateEnum InvoiceState
        {
            get
            {
                return State switch
                {
                    (int)StateEnum.New => StateEnum.New,
                    (int)StateEnum.Payed => StateEnum.Payed,
                    _ => throw new Exception("Unknown")
                };
            }
            set
            {
                this.State = value switch
                {
                    StateEnum.New => (int)StateEnum.New,
                    StateEnum.Payed => (int)StateEnum.Payed,
                    _ => throw new Exception("Unknown")
                };
            }
        }
    }
}
