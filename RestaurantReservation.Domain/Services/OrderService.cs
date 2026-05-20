using Microsoft.Extensions.Logging;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Domain.Interfaces.Repositories;
using RestaurantReservation.Domain.Interfaces.Services;

namespace RestaurantReservation.Domain.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IReservationRepository reservationRepository,
        IMenuItemRepository menuItemRepository,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _orderItemRepository = orderItemRepository;
        _reservationRepository = reservationRepository;
        _menuItemRepository = menuItemRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
        => await _orderRepository.GetAllWithDetailsAsync();

    public async Task<Order?> GetByIdAsync(int id)
        => await _orderRepository.GetByIdWithDetailsAsync(id);

    public async Task<Order?> GetByReservationAsync(int reservationId)
        => await _orderRepository.GetByReservationAsync(reservationId);

    public async Task<Order> CreateAsync(Order order, IEnumerable<OrderItem> items)
    {
        var reservation = await _reservationRepository.GetByIdAsync(order.ReservationId)
            ?? throw new KeyNotFoundException($"No se encontró la reserva con Id {order.ReservationId}.");

        if (reservation.Status == ReservationStatus.Cancelled)
            throw new InvalidOperationException("No se puede crear una orden para una reserva cancelada.");

        if (await _orderRepository.GetByReservationAsync(order.ReservationId) != null)
            throw new InvalidOperationException("Esta reserva ya tiene una orden asociada.");

        var itemList = items.ToList();
        if (!itemList.Any())
            throw new InvalidOperationException("La orden debe tener al menos un ítem.");

        decimal total = 0;
        foreach (var item in itemList)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(item.MenuItemId)
                ?? throw new KeyNotFoundException($"No se encontró el ítem del menú con Id {item.MenuItemId}.");
            if (!menuItem.IsAvailable)
                throw new InvalidOperationException($"El ítem '{menuItem.Name}' no está disponible.");
            if (item.Quantity <= 0)
                throw new InvalidOperationException("La cantidad de cada ítem debe ser mayor a 0.");

            item.UnitPrice = menuItem.Price;
            total += menuItem.Price * item.Quantity;
        }

        order.TotalAmount = total;
        var createdOrder = await _orderRepository.CreateAsync(order);

        foreach (var item in itemList)
        {
            item.OrderId = createdOrder.Id;
            await _orderItemRepository.CreateAsync(item);
        }

        return (await _orderRepository.GetByIdWithDetailsAsync(createdOrder.Id))!;
    }

    public async Task UpdateStatusAsync(int id, OrderStatus status)
    {
        var order = await _orderRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró la orden con Id {id}.");

        if (order.Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("No se puede cambiar el estado de una orden cancelada.");

        order.Status = status;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _orderRepository.ExistsAsync(id))
            throw new KeyNotFoundException($"No se encontró la orden con Id {id}.");

        await _orderRepository.DeleteAsync(id);
    }

    public async Task AddItemAsync(int orderId, OrderItem item)
    {
        var order = await _orderRepository.GetByIdWithDetailsAsync(orderId)
            ?? throw new KeyNotFoundException($"No se encontró la orden con Id {orderId}.");

        if (order.Status != OrderStatus.Pending)
            throw new InvalidOperationException("Solo se pueden agregar ítems a órdenes en estado Pendiente.");

        var menuItem = await _menuItemRepository.GetByIdAsync(item.MenuItemId)
            ?? throw new KeyNotFoundException($"No se encontró el ítem del menú con Id {item.MenuItemId}.");
        if (!menuItem.IsAvailable)
            throw new InvalidOperationException($"El ítem '{menuItem.Name}' no está disponible.");

        item.OrderId = orderId;
        item.UnitPrice = menuItem.Price;
        await _orderItemRepository.CreateAsync(item);

        order.TotalAmount += menuItem.Price * item.Quantity;
        await _orderRepository.UpdateAsync(order);
    }

    public async Task RemoveItemAsync(int orderId, int itemId)
    {
        var order = await _orderRepository.GetByIdWithDetailsAsync(orderId)
            ?? throw new KeyNotFoundException($"No se encontró la orden con Id {orderId}.");

        if (order.Status != OrderStatus.Pending)
            throw new InvalidOperationException("Solo se pueden eliminar ítems de órdenes en estado Pendiente.");

        var item = await _orderItemRepository.GetByIdAsync(itemId)
            ?? throw new KeyNotFoundException($"No se encontró el ítem con Id {itemId}.");

        if (item.OrderId != orderId)
            throw new InvalidOperationException("El ítem no pertenece a esta orden.");

        order.TotalAmount -= item.UnitPrice * item.Quantity;
        await _orderItemRepository.DeleteAsync(itemId);
        await _orderRepository.UpdateAsync(order);
    }
}
