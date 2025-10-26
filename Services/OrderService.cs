using TiposDeInyecciónDeDependencias.Models;

namespace TiposDeInyecciónDeDependencias.Services
{
    public class OrderService : IOrderService
    {
        private readonly Guid _instanceId;
        private readonly List<Order> _orders;

        public OrderService()
        {
            _instanceId = Guid.NewGuid();
            _orders = new List<Order>();
            
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Nueva instancia creada: {_instanceId}...");
        }

        public Guid GetInstanceId() => _instanceId;

        public void AddOrder(Order order)
        {
            _orders.Add(order);
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Pedido agregado en instancia {_instanceId}. Total: {_orders.Count}...");
        }

        public List<Order> GetOrders() => _orders;

        public int GetOrdersCount() => _orders.Count;
    }
}