using Stock.Model;

namespace Stock.ServiceAPI
{
    public interface IWebClient
    {
        public Task<bool> PostEvent(Event @event);

        public Task<IEnumerable<OrderLine>?> GetOrder(Guid id);
    }
}
