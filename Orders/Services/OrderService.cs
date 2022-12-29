using Orders.Model;
using Orders.ServiceAPI;

namespace Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseContext dbContext;
        private readonly IWebClient webClient;

        public OrderService(DatabaseContext dbContext, IWebClient webClient)
        {
            this.dbContext = dbContext;
            this.webClient = webClient;
        }

        public Order? CreateOrder(Order order)
        {
            //check payload
            if (!verifyCreateOrder(order)) { return null; }

            //set state and create ids
            order.OrderState = Order.StateEnum.Pending;
            order.OrderId = Guid.NewGuid();
            order.Lines!.ForEach(line => { line.OrderId = order.OrderId; line.OrderLineId = Guid.NewGuid(); });
            
            //start transaction
            using var transaction = this.dbContext.Database.BeginTransaction();
            try
            {
                //created order
                this.dbContext.Orders.Add(order);
                this.dbContext.SaveChanges();

                //publish event
                var resp = this.webClient.PostEvent(new Event { EventId = order.OrderId, EventName = "OrderCreated", EventQueue = Event.QueueEnum.OrderQueue, Params = order.GenerateOrderCreatedParams() }).Result;
                if (!resp)
                {
                    transaction.Rollback();
                    return null;
                }

                //commit transaction
                transaction.Commit();
            }
            catch (Exception)
            {
                //unable to create order
                return null;
            }
            return order;
        }

        public async Task<bool> ConfirmOrder(Guid id)
        {
            //check if order exists
            var possibleOrder = this.dbContext.Orders
                .Where(o => o.OrderId == id)
                .SingleOrDefault();

            //we cant do much about this
            if (possibleOrder == null) { return false; }

            //update order
            if (possibleOrder.OrderState == Order.StateEnum.PendingPayment)
            {
                var transaction = this.dbContext.Database.BeginTransaction();
                try
                {
                    //updating order
                    possibleOrder.OrderState = Order.StateEnum.Created;
                    this.dbContext.Orders.Update(possibleOrder);
                    this.dbContext.SaveChanges();

                    //post sucess event
                    var resp = await this.webClient.PostEvent(new Event { EventId = id, EventName = "OrderConfirmed", EventQueue = Event.QueueEnum.OrderQueue });
                    if (!resp)
                    {
                        transaction.Rollback();
                        throw new Exception("Unable to send sucess event");
                    }

                    transaction.Commit();
                }
                catch(Exception) 
                { 
                    //this should be retried
                    return false; 
                }
                return true;
            }

            return false;
        }

        #region private helpers
        private static bool verifyCreateOrder(Order order) 
        {
            if (order == null) { return false; }

            if (order.Lines == null || !order.Lines.Any()) { return false; }

            foreach (var line in order.Lines)
            {
                if (line == null) { return false; }
                if (line.Amount == 0) { return false; }
                if (line.Count == 0) { return false; }
                if (line.Total == 0) { return false; }
            }

            if (order.ItemCount == 0) { return false; }

            if (order.Amount == 0) { return false; }

            return true;
        }

        #endregion
    }
}
