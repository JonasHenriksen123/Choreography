using Broker.Model;

namespace Broker.ServiceAPI
{
    public interface IRelayService
    {
        Task ScheduleMessage(Event @event);
    }
}
