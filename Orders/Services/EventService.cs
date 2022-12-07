using Orders.Model;
using Orders.ServiceAPI;
using System.Transactions;

namespace Orders.Services
{
    public class EventService : IEventService
    {
        private readonly DatabaseContext dbContext;
        private readonly IWebClient webClient;

        private readonly List<Handler> handlers;

        public EventService(DatabaseContext dbContext, IWebClient webClient)
        {
            this.dbContext = dbContext;
            this.webClient = webClient;
            handlers = new List<Handler>();

            #region mapping of events and actions
            handlers.AddRange(new[] { 
                new Handler { Queue = Event.QueueEnum.StockQueue, Event = "ItemsReserved", HandleEvent = confirmorder},
                new Handler { Queue = Event.QueueEnum.AccountsQueue, Event = "AccountCreditReserved", HandleEvent = confirmorder},
                new Handler { Queue = Event.QueueEnum.StockQueue, Event = "ItemValidationFailed", HandleEvent = rejectOrder},
                new Handler { Queue = Event.QueueEnum.StockQueue, Event = "ItemReservationFailed", HandleEvent = rejectOrder},
                new Handler { Queue = Event.QueueEnum.StockQueue, Event = "ItemsReservationRemoved", HandleEvent = rejectOrder},
            });
            #endregion
        }

        public async Task HandleEvent(Event @event)
        {
            if (@event == null)
            {
                //we can do nothing here
                return;
            }

            //we need to see if this event has already been handled
            var res = this.dbContext.Events
                .Where(e => e.EventId == @event.EventId && e.Queue == @event.Queue && e.EventName == @event.EventName)
                .SingleOrDefault();

            if (res != null)
            {
                //this event has already been handled, dismiss
                return;
            }

            //find handler for event
            var handler = handlers.SingleOrDefault(h => h.Queue == @event.EventQueue && h.Event == @event.EventName);

            if (handler == null) 
            {
                //we dont have a handler for this, consider logging
                return;
            }

            await handler.HandleEvent!(@event);
        }

        #region private helpers
        private async Task confirmorder(Event @event)
        {
            //check order
            var order = this.dbContext.Orders
                .Where(o => o.OrderId == @event.EventId)
                .SingleOrDefault();

            if (order == null) 
            {
                //Order does not exist, we may want to retry event, no logging
                return;
            }

            if (@event.EventQueue == Event.QueueEnum.StockQueue && order.AccountId != null)
            {
                //this order is for an account, no handling yet, log event
                this.dbContext.Events.Add(@event);
                this.dbContext.SaveChanges();
                return;
            }

            order.OrderState = Order.StateEnum.PendingPayment;

            //start transaction
            using var transaction = this.dbContext.Database.BeginTransaction();

            try
            {
                //update order
                this.dbContext.Orders.Update(order);
                this.dbContext.SaveChanges(true);

                //publish sucess event
                var resp = await this.webClient.PostEvent(new Event { EventId = @event.EventId, EventName = "OrderPendingPayment", EventQueue = Event.QueueEnum.OrderQueue });
                if (!resp)
                {
                    transaction.Rollback();
                    throw new Exception("Unable to send sucess event");
                }

                transaction.Commit();
            }
            catch(Exception) 
            {
                //we may want to retry this transaction dont publish or log
                return;
            }

            this.dbContext.Events.Add(@event);
            this.dbContext.SaveChanges();
        }

        private async Task rejectOrder(Event @event)
        {
            //start transaction
            using var transaction = this.dbContext.Database.BeginTransaction();

            try
            {
                //find order
                var oRes = this.dbContext.Orders
                    .Where(o => o.OrderId == @event.EventId)
                    .SingleOrDefault();

                if (oRes == null)
                {
                    //Order does not exist, throw
                    transaction.Rollback();
                    throw new Exception($"No order with id: {@event.EventId}");
                }

                    //find orderlines
                    var olRes = this.dbContext.OrderLines
                    .Where(ol => ol.OrderId == @event.EventId)
                    .ToList();

                if (olRes == null || !olRes.Any())
                {
                    //Order has no orderlines, throw 
                    transaction.Rollback();
                    throw new Exception($"No orderlines for order with id: {@event.EventId}");
                }

                //delete order from database
                this.dbContext.OrderLines.RemoveRange(olRes);
                this.dbContext.SaveChanges();

                this.dbContext.Orders.Remove(oRes);
                this.dbContext.SaveChanges();

                //publish success event
                var resp = await this.webClient.PostEvent(new Event { EventId = @event.EventId, EventName = "OrderDeleted", EventQueue = Event.QueueEnum.OrderQueue });
                if (!resp)
                {
                    transaction.Rollback();
                    throw new Exception("Unable to send sucess event");
                }

                //commit transaction
                transaction.Commit();
            }
            catch(Exception) 
            {
                //This event failed, but we might want to rerun later, no logging
                return;
            }

            this.dbContext.Events.Add(@event);
            this.dbContext.SaveChanges();
        }

        private class Handler
        {
            public Event.QueueEnum Queue { get; set; }

            public string? Event { get; set; }

            public Func<Event, Task>? HandleEvent { get; set; }
        }
        #endregion
    }
}
