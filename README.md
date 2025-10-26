# Tipos de Inyección de Dependencias en .NET

## Introducción

En este proyecto implementé conforme decia la tarea un servicio de gestión de pedidos esto con el proposito de entender cómo funcionan los ciclos de vida de servicios en **.NET**: **Transient**, **Scoped** y **Singleton**.

-----

## Cómo Implementé Cada Tipo de Servicio

### 1\. La Interfaz y el Servicio Base

Primero tuve que crear una interfaz **`IOrderService`** con los métodos que necesitaba:

```csharp
public interface IOrderService
{
    Guid GetInstanceId();
    void AddOrder(Order order);
    List<Order> GetOrders();
    int GetOrdersCount();
}
```

Luego implementé el **`OrderService`** que hace lo siguiente:

  * Genera un **Guid único** cuando se crea.
  * Mantiener una lista interna de pedidos.
  * Agrege los métodos para agregar, consultar y contar pedidos.

### 2\. Registro en `Program.cs`

Registré los servicios con sus diferentes ciclos de vida:

```csharp
builder.Services.AddTransient<IOrderServiceTransient, OrderServiceTransient>();
builder.Services.AddScoped<IOrderServiceScoped, OrderServiceScoped>();
builder.Services.AddSingleton<IOrderServiceSingleton, OrderServiceSingleton>();
```

### 3\. El Controlador

En el controlador llame cada tipo de servicio para probarlos:

```csharp
private readonly IOrderServiceTransient _transient;
private readonly IOrderServiceScoped _scoped;
private readonly IOrderServiceSingleton _singleton;
```

Luego tuve que crear *endpoints* **GET** y **POST** para cada uno, para poder ver el **`InstanceId`** y cómo es el comportamiento de los pedidos en memoria.

-----

## Lo Que Observé despues de las Pruebas

| Tipo | Observación Principal | Conclusión Personal |
| :--- | :--- | :--- |
| **TRANSIENT** | Cada vez que llamaba al *endpoint*, las instancias tenían **IDs distintos**. La lista de pedidos **nunca** se mantenía entre llamadas. | **Cada request es completamente nuevo**. Es como pedir un café en otra cafetería cada vez: nadie recuerda nada de lo que pediste antes. |
| **SCOPED** | Dentro del **mismo *request***, las instancias compartían el **mismo ID**. Entre *requests* distintos, el ID **cambiaba**. | **Scoped dura lo que dura un request**. Perfecto para operaciones temporales que solo tienen sentido durante esa ejecución. |
| **SINGLETON** | La instancia **nunca cambia**. Los pedidos se mantienen entre *requests*. | **Singleton es como una nevera común**: todo el mundo ve lo mismo y los datos permanecen hasta que alguien los elimina. |

-----

## Cuándo Usaría Cada Uno

| Tipo | Uso principal | Ejemplo |
| :--- | :--- | :--- |
| **TRANSIENT** | Servicios **sin estado**. | Validadores, generadores de reportes. |
| **SCOPED** | **Estado por *request***. | `DbContext`, `Unit of Work`. |
| **SINGLETON** | **Estado global compartido**. | Configuración, caché en memoria, `HttpClient`. |

-----

## Diagramas del Ciclo de Vida

### Transient - Siempre nueva instancia

```
Request 1:
┌─────────────┐
│ Controller  │
└──────┬──────┘
       │
       ├──> [Instancia A] Nueva
       │
       └──> [Instancia B] Nueva

Request 2:
┌─────────────┐
│ Controller  │
└──────┬──────┘
       │
       ├──> [Instancia C] Nueva
       │
       └──> [Instancia D] Nueva
```

### Scoped - Una instancia por request

```
Request 1:
┌─────────────┐
│ Controller  │
└──────┬──────┘
       │
       ├──> [Instancia X] Misma
       │
       └──> [Instancia X] Misma

Request 2:
┌─────────────┐
│ Controller  │
└──────┬──────┘
       │
       ├──> [Instancia Y] Nueva
       │
       └──> [Instancia Y] Nueva
```

### Singleton - Siempre la misma instancia

```
Request 1:
┌─────────────┐
│ Controller  │
└──────┬──────┘
       │
       ├──> [Instancia Z] Siempre
       │
Request 2:
┌─────────────┐
│ Controller  │
└──────┬──────┘
       │
       ├──> [Instancia Z] Siempre
```

-----

## Conclusión

  * **Transient** → si no necesito conservar nada.
  * **Scoped** → si quiero mantener estado durante un *request*.
  * **Singleton** → si necesito un estado global que todos compartan.
