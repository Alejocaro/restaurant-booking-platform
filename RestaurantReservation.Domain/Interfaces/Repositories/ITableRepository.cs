using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Interfaces.Repositories;

public interface ITableRepository : IGenericRepository<Table>
{
    Task<IEnumerable<Table>> GetByRestaurantAsync(int restaurantId);
    Task<IEnumerable<Table>> GetAvailableByRestaurantAsync(int restaurantId);
    Task<Table?> GetByIdWithDetailsAsync(int id);
    Task<bool> TableNumberExistsInRestaurantAsync(int restaurantId, int tableNumber, int? excludeId = null);
}
