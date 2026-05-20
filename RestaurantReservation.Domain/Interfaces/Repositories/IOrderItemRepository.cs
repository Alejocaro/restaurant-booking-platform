using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Domain.Interfaces.Repositories;

public interface IOrderItemRepository : IGenericRepository<OrderItem>
{
    Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId);
    Task<IEnumerable<OrderItem>> GetByOrderWithDetailsAsync(int orderId);
}
