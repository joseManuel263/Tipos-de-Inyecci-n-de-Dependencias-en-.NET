using Microsoft.AspNetCore.Mvc;
using TiposDeInyecciónDeDependencias.Models;
using TiposDeInyecciónDeDependencias.Services;

namespace TiposDeInyecciónDeDependencias
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _transient1;
        private readonly IOrderService _transient2;
        private readonly IOrderService _scoped1;
        private readonly IOrderService _scoped2;
        private readonly IOrderService _singleton1;
        private readonly IOrderService _singleton2;

        public OrdersController(
            [FromKeyedServices("transient")] IOrderService transient1,
            [FromKeyedServices("transient")] IOrderService transient2,
            [FromKeyedServices("scoped")] IOrderService scoped1,
            [FromKeyedServices("scoped")] IOrderService scoped2,
            [FromKeyedServices("singleton")] IOrderService singleton1,
            [FromKeyedServices("singleton")] IOrderService singleton2)
        {
            _transient1 = transient1;
            _transient2 = transient2;
            _scoped1 = scoped1;
            _scoped2 = scoped2;
            _singleton1 = singleton1;
            _singleton2 = singleton2;
        }
        
        [HttpGet("transient/info")]
        public IActionResult GetTransientInfo()
        {
            return Ok(new
            {
                Tipo = "TRANSIENT",
                Instancia1 = _transient1.GetInstanceId(),
                Instancia2 = _transient2.GetInstanceId(),
                SonIguales = _transient1.GetInstanceId() == _transient2.GetInstanceId(),
                CantidadPedidos1 = _transient1.GetOrdersCount(),
                CantidadPedidos2 = _transient2.GetOrdersCount()
            });
        }

        [HttpGet("transient/orders")]
        public IActionResult GetTransientOrders()
        {
            return Ok(new
            {
                InstanceId = _transient1.GetInstanceId(),
                Pedidos = _transient1.GetOrders(),
                Total = _transient1.GetOrdersCount()
            });
        }

        [HttpPost("transient/orders")]
        public IActionResult AddTransientOrder([FromBody] Order order)
        {
            _transient1.AddOrder(order);
            return Ok(new
            {
                message = "Pedido agregado (Transient)",
                InstanceId = _transient1.GetInstanceId(),
                total = _transient1.GetOrdersCount()
            });
        }
        
        [HttpGet("scoped/info")]
        public IActionResult GetScopedInfo()
        {
            return Ok(new
            {
                Tipo = "SCOPED",
                Instancia1 = _scoped1.GetInstanceId(),
                Instancia2 = _scoped2.GetInstanceId(),
                SonIguales = _scoped1.GetInstanceId() == _scoped2.GetInstanceId(),
                CantidadPedidos1 = _scoped1.GetOrdersCount(),
                CantidadPedidos2 = _scoped2.GetOrdersCount()
            });
        }

        [HttpGet("scoped/orders")]
        public IActionResult GetScopedOrders()
        {
            return Ok(new
            {
                InstanceId = _scoped1.GetInstanceId(),
                Pedidos = _scoped1.GetOrders(),
                Total = _scoped1.GetOrdersCount()
            });
        }

        [HttpPost("scoped/orders")]
        public IActionResult AddScopedOrder([FromBody] Order order)
        {
            _scoped1.AddOrder(order);
            return Ok(new
            {
                message = "Pedido agregado (Scoped)",
                InstanceId = _scoped1.GetInstanceId(),
                total = _scoped1.GetOrdersCount()
            });
        }
        
        [HttpGet("singleton/info")]
        public IActionResult GetSingletonInfo()
        {
            return Ok(new
            {
                Tipo = "SINGLETON",
                Instancia1 = _singleton1.GetInstanceId(),
                Instancia2 = _singleton2.GetInstanceId(),
                SonIguales = _singleton1.GetInstanceId() == _singleton2.GetInstanceId(),
                CantidadPedidos1 = _singleton1.GetOrdersCount(),
                CantidadPedidos2 = _singleton2.GetOrdersCount()
            });
        }

        [HttpGet("singleton/orders")]
        public IActionResult GetSingletonOrders()
        {
            return Ok(new
            {
                InstanceId = _singleton1.GetInstanceId(),
                Pedidos = _singleton1.GetOrders(),
                Total = _singleton1.GetOrdersCount()
            });
        }

        [HttpPost("singleton/orders")]
        public IActionResult AddSingletonOrder([FromBody] Order order)
        {
            _singleton1.AddOrder(order);
            return Ok(new
            {
                message = "Pedido agregado (Singleton)",
                InstanceId = _singleton1.GetInstanceId(),
                total = _singleton1.GetOrdersCount()
            });
        }
        
        [HttpGet("comparar")]
        public IActionResult Comparar()
        {
            return Ok(new
            {
                Transient = new
                {
                    Id1 = _transient1.GetInstanceId(),
                    Id2 = _transient2.GetInstanceId(),
                    Iguales = _transient1.GetInstanceId() == _transient2.GetInstanceId(),
                    Pedidos = _transient1.GetOrdersCount()
                },
                Scoped = new
                {
                    Id1 = _scoped1.GetInstanceId(),
                    Id2 = _scoped2.GetInstanceId(),
                    Iguales = _scoped1.GetInstanceId() == _scoped2.GetInstanceId(),
                    Pedidos = _scoped1.GetOrdersCount()
                },
                Singleton = new
                {
                    Id1 = _singleton1.GetInstanceId(),
                    Id2 = _singleton2.GetInstanceId(),
                    Iguales = _singleton1.GetInstanceId() == _singleton2.GetInstanceId(),
                    Pedidos = _singleton1.GetOrdersCount()
                }
            });
        }
    }
}