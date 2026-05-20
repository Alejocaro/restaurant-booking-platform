using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Interfaces.Repositories;

public interface IMenuItemRepository : IGenericRepository<MenuItem>
{
    Task<IEnumerable<MenuItem>> GetByRestaurantAsync(int restaurantId);
    Task<IEnumerable<MenuItem>> GetByRestaurantAndCategoryAsync(int restaurantId, MenuItemCategory category);
    Task<IEnumerable<MenuItem>> GetAvailableByRestaurantAsync(int restaurantId);
}
