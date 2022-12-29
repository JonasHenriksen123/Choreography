using Broker.Model;
using Broker.ServiceAPI;
using Microsoft.EntityFrameworkCore;

namespace Broker.Services
{
    public class EventService : IEventService
    {
        private readonly DatabaseContext dbContext;
        private readonly IRelayService relayService;

        public EventService(DatabaseContext dbContext, IRelayService relayService)
        {
            this.dbContext = dbContext;
            this.relayService = relayService;
        }

        public bool AddEvent(Event @event)
        {
            //check payload
            if (!verifyAddEvent(@event)) { return false; }

            //set publish date
            @event.PublishDate= DateTime.Now;

            //add to event store
            try
            {
                this.dbContext.Events.Add(@event);
                this.dbContext.SaveChanges();
            }
            catch (Exception) 
            {
                return false;
            }

            this.dbContext.ChangeTracker.Clear();
            this.relayService.ScheduleMessage(@event).Wait();

            return true;
        }

        public IEnumerable<Event> GetEvents(Guid EventId)
        {
            //get events for Id
            var events = this.dbContext.Events.AsNoTracking()
                .Where(e => e.EventId == EventId)
                .OrderBy(e => e.PublishDate)
                .ToArray();

            return events;
        }

        #region private helpers
        private static bool verifyAddEvent(Event @event) 
        {
            if (@event == null) { return false; }

            if (String.IsNullOrEmpty(@event.EventName)) { return false; }

            if (@event.Queue < 0 || @event.Queue > 2) { return false; }

            return true;
        }
        #endregion
    }
}
