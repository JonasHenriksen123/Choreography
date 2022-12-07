using Broker.Model;

namespace Broker.ServiceAPI
{
    public interface IEventService
    {
        IEnumerable<Event> GetEvents(Guid EventId);

        bool AddEvent(Event @event);
    }
}
