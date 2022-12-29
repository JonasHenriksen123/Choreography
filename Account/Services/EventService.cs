using Accounts.Model;
using Accounts.ServiceAPI;

namespace Accounts.Services
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
            handlers.AddRange(new[] { new Handler { Queue = Event.QueueEnum.StockQueue, Event = "ItemsReserved", HandleEvent = reserveCredit } });
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

        private async Task reserveCredit(Event @event)
        {
            //TODO implement

            //get order details from order service
            var order = await this.webClient.GetOrder(@event.EventId);
            if (order == null)
            {
                //Order was not found, we might want to retry this event, no logging
                return;
            }

            if (order.AccountId == null)
            {
                //this order is not for an account, no action for this service, log and move on
                this.dbContext.Events.Add(@event);
                this.dbContext.SaveChanges();
                return;
            }

            if (order.OrderState != Order.StateEnum.Pending)
            {
                //wrong state on order, we might want to retry this action, no logging
                return;
            }

            //start transaction
            using var transaction = this.dbContext.Database.BeginTransaction();

            try
            {
                //fetch account
                var account = this.dbContext.Accounts
                .Where(a => a.AccountId == order.AccountId!)
                .SingleOrDefault();

                if (account == null)
                {
                    //account does not exist, we may want to retry this transaction, no logging
                    transaction.Rollback();
                    throw new AccountValidationFailedException();
                }

                if (account.Credit < order.Amount)
                {
                    //account does not have credit enough for this order, send credit reservation failed
                    transaction.Rollback();
                    throw new CreditReservationFailedException();
                }

                account.Credit -= order.Amount;

                var invoice = order.GenerateInvoice();

                if (invoice == null)
                {
                    //we probably want to retry this, no logging
                    return;
                }

                //update account credit
                this.dbContext.Accounts.Update(account);
                this.dbContext.SaveChanges();

                //save invoice
                this.dbContext.Invoices.Add(invoice);
                this.dbContext.SaveChanges();

                //publish sucess event
                var resp = await this.webClient.PostEvent(new Event { EventId = @event.EventId, EventName = "AccountCreditReserved", EventQueue = Event.QueueEnum.AccountsQueue, });
                if (!resp)
                {
                    transaction.Rollback();
                    throw new Exception("Unable to send sucess event");
                }

                transaction.Commit();
            }
            catch(AccountValidationFailedException)
            {
                //send fail event
                var resp = await this.webClient.PostEvent(new Event { EventId = @event.EventId, EventName = "AccountValidationFailed", EventQueue = Event.QueueEnum.AccountsQueue });
                if (!resp)
                {
                    //we may want to retry, dont log
                    return;
                }
            }
            catch(CreditReservationFailedException)
            {
                //send fail event
                var resp = await this.webClient.PostEvent(new Event { EventId= @event.EventId, EventName = "AccountCreditReservationFailed", EventQueue = Event.QueueEnum.AccountsQueue });
                if (!resp)
                {
                    //we may want to retry, dont log
                    return;
                }
            }
            catch(Exception) 
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

        private class CreditReservationFailedException : Exception 
        {
            public CreditReservationFailedException() { }
        }

        private class AccountValidationFailedException : Exception
        {
            public AccountValidationFailedException() { }
        }
        #endregion
    }
}
