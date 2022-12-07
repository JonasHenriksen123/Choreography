using Orders.Model;

namespace Orders.ServiceAPI
{
    public interface IEventHandlerService
    {
        Task EnqueueEvent(Event @event);
    }
}
