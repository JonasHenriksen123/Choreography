using ShopAPI.Model;

namespace ShopAPI.ServiceAPI
{
    public interface IWebClient
    {
        Task<Account[]> GetAllAccounts();

        Task<Account?> GetAccount(Guid id);

        Task<Account?> CreateAccount(Account account);

        Task<Invoice?> GetInvoice(Guid id);

        Task<Invoice[]> GetInvoicesForAccount(Guid id);

        Task<IPartialItem[]> GetAllItems();

        Task<Item?> GetItem(Guid id);

        Task<Item?> CreateItem(Item item);

        Task<Order[]> GetOrders();

        Task<Order?> GetOrder(Guid id);

        Task<Order?> CreateOrder(Order order);

        Task<Event[]> GetEventsForOrder(Guid id);

        Task<OrderLine[]> GetOrderLinesForOrder(Guid id);

        Task<bool> ConfirmOrder(Guid id);
    }
}
