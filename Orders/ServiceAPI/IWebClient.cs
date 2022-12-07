using Orders.Model;

namespace Orders.ServiceAPI
{
    public interface IWebClient
    {
        Task<bool> PostEvent(Event @event);
    }
}
