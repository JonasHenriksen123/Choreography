using Stock.Model;

namespace Stock.ServiceAPI
{
    public interface IEventHandlerService
    {
        Task EnqueueEvent(Event @event);
    }
}
