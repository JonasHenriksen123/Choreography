using Orders.Model;
using Orders.ServiceAPI;

namespace Orders.Services
{
    public class EventHandlerService : IEventHandlerService
    {
        private readonly IBackgroundTaskQueue taskQueue;
        private readonly IEventService eventService;

        public EventHandlerService(IBackgroundTaskQueue taskQueue, IEventService eventService)
        {
            this.taskQueue = taskQueue;
            this.eventService = eventService;
        }
        public async Task EnqueueEvent(Event @event)
        {
            // add event to queue
            await taskQueue.QueueBackgroundWorkItemAsync((CancellationToken cancellationToken) =>
            {
                return proccessEvent(@event, cancellationToken);
            });
        }

        private async ValueTask proccessEvent(Event @event, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                //let event service handle event
               await this.eventService.HandleEvent(@event);
            }
        }
    }
}
