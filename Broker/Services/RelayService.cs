using Broker.Model;
using Broker.ServiceAPI;

namespace Broker.Services
{
    public class RelayService : IRelayService
    {
        private readonly IBackgroundTaskQueue taskQueue;
        private readonly IWebClient webClient;

        private readonly List<EventAction> events;

        public RelayService(IBackgroundTaskQueue taskQueue, IWebClient webClient)
        {
            this.taskQueue = taskQueue;
            this.webClient = webClient;
            this.events = new List<EventAction>();

            #region event subscriptions
            events.AddRange(new[] {
                new EventAction { Queue = Event.QueueEnum.OrderQueue, Name = "OrderCreated", Recievers = new[] { IWebClient.Reciever.Stock } },
                new EventAction { Queue = Event.QueueEnum.StockQueue, Name = "ItemsReserved", Recievers = new [] { IWebClient.Reciever.Orders, IWebClient.Reciever.Accounts } },
                new EventAction { Queue = Event.QueueEnum.StockQueue, Name = "ItemValidationFailed", Recievers = new [] { IWebClient.Reciever.Orders } },
                new EventAction { Queue = Event.QueueEnum.StockQueue, Name = "ItemReservationFailed", Recievers = new [] { IWebClient.Reciever.Orders } },
                new EventAction { Queue = Event.QueueEnum.OrderQueue, Name = "OrderDeleted", Recievers = Array.Empty<IWebClient.Reciever>() },
                new EventAction { Queue = Event.QueueEnum.OrderQueue, Name = "OrderPendingPayment", Recievers = Array.Empty<IWebClient.Reciever>() },
                new EventAction { Queue = Event.QueueEnum.OrderQueue, Name = "OrderConfirmed", Recievers = Array.Empty<IWebClient.Reciever>() },
                new EventAction { Queue = Event.QueueEnum.AccountsQueue, Name = "AccountCreditReserved", Recievers = new[] { IWebClient.Reciever.Orders } },
                new EventAction { Queue = Event.QueueEnum.AccountsQueue, Name = "AccountCreditReservationFailed", Recievers = new[] { IWebClient.Reciever.Stock } },
                new EventAction { Queue = Event.QueueEnum.AccountsQueue, Name = "AccountValidationFailed", Recievers = new[] { IWebClient.Reciever.Stock } },
                new EventAction { Queue = Event.QueueEnum.StockQueue, Name = "ItemsReservationRemoved", Recievers = new[] { IWebClient.Reciever.Orders } },
            });
            #endregion
        }
        public async Task ScheduleMessage(Event @event)
        {
            // add event to queue of outgoing events
            await taskQueue.QueueBackgroundWorkItemAsync((CancellationToken cancellationToken) =>
            {
                return proccessEvent(@event, cancellationToken);
            });
        }

        private async ValueTask proccessEvent(Event @event, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                //find subscribtions to event
                var subscriptions = this.events.SingleOrDefault(ea => ea.Name == @event.EventName && ea.Queue == @event.EventQueue);

                if (subscriptions != null) 
                {
                    //notify each subscriber
                    foreach (var sub in subscriptions.Recievers!) {
                        var resp = await this.webClient.SendEvent(@event, sub);
                    }
                }

                //this event is not cataloged, we might want to log for diagnostics
            }
        }

        private class EventAction
        {
            public Event.QueueEnum Queue { get; set; }

            public string? Name { get; set; }

            public IEnumerable<IWebClient.Reciever>? Recievers { get; set; }
        }
    }
}
