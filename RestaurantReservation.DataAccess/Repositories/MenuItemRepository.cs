using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DataAccess.Context;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Domain.Interfaces.Repositories;

namespace RestaurantReservation.DataAccess.Repositories;

public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
{
    public MenuItemRepository(RestaurantDbContext context) : base(context) { }

    public async Task<IEnumerable<MenuItem>> GetByRestaurantAsync(int restaurantId)
        => await _dbSet.Where(m => m.RestaurantId == restaurantId).ToListAsync();

    public async Task<IEnumerable<MenuItem>> GetByRestaurantAndCategoryAsync(int restaurantId, MenuItemCategory category)
        => await _dbSet
            .Where(m => m.RestaurantId == restaurantId && m.Category == category)
            .ToListAsync();

    public async Task<IEnumerable<MenuItem>> GetAvailableByRestaurantAsync(int restaurantId)
        => await _dbSet
            .Where(m => m.RestaurantId == restaurantId && m.IsAvailable)
            .ToListAsync();
}
