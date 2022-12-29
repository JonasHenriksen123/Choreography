using Stock.Model;
using Stock.ServiceAPI;

namespace Stock.Services
{
    public class EventService : IEventService
    {
        private readonly DatabaseContext dbContext;
        private readonly IWebClient client;

        private readonly List<Handler> handlers;

        public EventService(DatabaseContext dbContext, IWebClient client)
        {
            this.dbContext = dbContext;
            this.client = client;
            handlers = new List<Handler>();

            #region mapping of events and actions
            handlers.AddRange(new[] { 
                new Handler { Queue = Event.QueueEnum.OrderQueue, Event = "OrderCreated", HandleEvent = reserveItems },
                new Handler { Queue = Event.QueueEnum.AccountsQueue, Event = "OrderCreated", HandleEvent = removeReservation },
                new Handler { Queue = Event.QueueEnum.AccountsQueue, Event = "OrderCreated", HandleEvent = removeReservation },
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
            this.dbContext.ChangeTracker.Clear();
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
        private async Task reserveItems(Event @event)
        {
            //get the parameters from the event
            var param = @event.FetchOrderCreatedParams();

            if (param == null || param.Items == null || !param.Items.Any()) return;

            //start transaction
            var transaction = this.dbContext.Database.BeginTransaction();

            try
            {
                //reserve stock for each item
                foreach (var item in param.Items) {
                    var res = this.dbContext.Items
                        .Where(i => i.ItemId == item.ItemId)
                        .SingleOrDefault();

                    if (res == null)
                    {
                        //item does no exist, abort
                        transaction.Rollback();
                        throw new ItemValidationFailedException();
                    }

                    if (res.Amount < item.Amount) 
                    {
                        //not enough stock, abort
                        transaction.Rollback();
                        throw new ItemReservationFailedEvent();
                    }

                    res.Amount -= item.Amount;

                    this.dbContext.Items.Update(res);
                    this.dbContext.SaveChanges();
                }

                //publish succes event
                var resp = await this.client.PostEvent(new Event { EventId = @event.EventId, EventName = "ItemsReserved", EventQueue = Event.QueueEnum.StockQueue, });
                if (!resp) 
                {
                    transaction.Rollback();
                    throw new Exception("Unable to send sucess event");
                }

                //commit transaction
                transaction.Commit();
            }
            catch (ItemValidationFailedException)
            {
                //send a fail event
                var resp = await this.client.PostEvent(new Event { EventId= @event.EventId, EventName = "ItemValidationFailed", EventQueue= Event.QueueEnum.StockQueue, });
                if (!resp)
                {
                    //we might want to retry this transaction, dont log
                    return;
                }
            }
            catch (ItemReservationFailedEvent)
            {
                //send fail event
                var resp = await this.client.PostEvent(new Event { EventId = @event.EventId, EventName = "ItemReservationFailed", EventQueue = Event.QueueEnum.StockQueue });
                if (!resp)
                {
                    //we might want to retry this transaction, dont log
                    return;
                }
            }
            catch (Exception) 
            {
                //we might want to retry this transaction, dont publish fail event or log
                return;
            }

            this.dbContext.Events.Add(@event);
            this.dbContext.SaveChanges();
        }

        private async Task removeReservation(Event @event)
        {
            //fetch orderLines
            var orderLines = await this.client.GetOrder(@event.EventId);

            if (orderLines == null || !orderLines.Any())
            {
                //no orderlines found, we might want to retry, no logging
                return;
            }

            //start transaction
            using var transaction = this.dbContext.Database.BeginTransaction();
            try
            {
                foreach (var line in orderLines)
                {
                    var res = this.dbContext.Items
                        .Where(i => i.ItemId == line.ItemId)
                        .SingleOrDefault();

                    if (res == null)
                    {
                        //this scenario should never happen, we have to abort and try again
                        transaction.Rollback();
                        throw new Exception($"Unable to find item with id: { line.ItemId }");
                    }

                    res.Amount += line.Count;

                    this.dbContext.Items.Update(res);
                    this.dbContext.SaveChanges();
                }

                //publish sucess event
                var resp = await this.client.PostEvent(new Event { EventId = @event.EventId, EventName = "ItemsReservationRemoved", EventQueue = Event.QueueEnum.StockQueue, });
                if (!resp)
                {
                    transaction.Rollback();
                    throw new Exception("Unable to send sucess event");
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                //we may want to retry, no logging
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

        private class ItemValidationFailedException : Exception
        {
            public ItemValidationFailedException() { }
        }

        private class ItemReservationFailedEvent : Exception 
        {
            public ItemReservationFailedEvent() { }
        }
        #endregion
    }
}
