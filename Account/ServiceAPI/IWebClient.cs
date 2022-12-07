using Accounts.Model;
    
namespace Accounts.ServiceAPI
{
    public interface IWebClient
    {
        Task<bool> PostEvent(Event @event);

        Task<Order?> GetOrder(Guid id);
    }
}
