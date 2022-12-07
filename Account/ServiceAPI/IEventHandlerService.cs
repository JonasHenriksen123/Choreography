using Accounts.Model;

namespace Accounts.ServiceAPI
{
    public interface IEventHandlerService
    {
        Task EnqueueEvent(Event @event);
    }
}
