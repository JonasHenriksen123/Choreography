using Orders.Model;

namespace Orders.ServiceAPI
{
    public interface IOrderService
    {
        Order? CreateOrder(Order order);

        Task<bool> ConfirmOrder(Guid id);
    }
}
