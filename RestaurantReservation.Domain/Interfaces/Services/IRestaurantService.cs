using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Domain.Interfaces.Services;

public interface IRestaurantService
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<Restaurant?> GetByIdAsync(int id);
    Task<Restaurant> CreateAsync(Restaurant restaurant);
    Task UpdateAsync(Restaurant restaurant);
    Task DeleteAsync(int id);
}
