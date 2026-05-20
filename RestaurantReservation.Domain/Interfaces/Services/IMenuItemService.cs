using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Interfaces.Services;

public interface IMenuItemService
{
    Task<IEnumerable<MenuItem>> GetAllAsync();
    Task<IEnumerable<MenuItem>> GetByRestaurantAsync(int restaurantId);
    Task<IEnumerable<MenuItem>> GetByRestaurantAndCategoryAsync(int restaurantId, MenuItemCategory category);
    Task<MenuItem?> GetByIdAsync(int id);
    Task<MenuItem> CreateAsync(MenuItem menuItem);
    Task UpdateAsync(MenuItem menuItem);
    Task DeleteAsync(int id);
}
