using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Interfaces.Services;

public interface ITableService
{
    Task<IEnumerable<Table>> GetAllAsync();
    Task<IEnumerable<Table>> GetByRestaurantAsync(int restaurantId);
    Task<IEnumerable<Table>> GetAvailableByRestaurantAsync(int restaurantId);
    Task<Table?> GetByIdAsync(int id);
    Task<Table> CreateAsync(Table table);
    Task UpdateAsync(Table table);
    Task DeleteAsync(int id);
    Task UpdateStatusAsync(int id, TableStatus status);
}
