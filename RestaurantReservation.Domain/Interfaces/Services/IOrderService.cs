using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Interfaces.Services;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<Order?> GetByReservationAsync(int reservationId);
    Task<Order> CreateAsync(Order order, IEnumerable<OrderItem> items);
    Task UpdateStatusAsync(int id, OrderStatus status);
    Task DeleteAsync(int id);
    Task AddItemAsync(int orderId, OrderItem item);
    Task RemoveItemAsync(int orderId, int itemId);
}
