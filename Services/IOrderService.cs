using TiposDeInyecciónDeDependencias.Models;

namespace TiposDeInyecciónDeDependencias.Services
{
    public interface IOrderService
    {
        Guid GetInstanceId();
        void AddOrder(Order order);
        List<Order> GetOrders();
        int GetOrdersCount();
    }
}   