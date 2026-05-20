using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Domain.Interfaces.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order?> GetByReservationAsync(int reservationId);
    Task<Order?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Order>> GetAllWithDetailsAsync();
}
