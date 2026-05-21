using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DataAccess.Context;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Domain.Interfaces.Repositories;

namespace RestaurantReservation.DataAccess.Repositories;

public class TableRepository : GenericRepository<Table>, ITableRepository
{
    public TableRepository(RestaurantDbContext context) : base(context) { }

    public async Task<IEnumerable<Table>> GetByRestaurantAsync(int restaurantId)
        => await _dbSet.Where(t => t.RestaurantId == restaurantId).ToListAsync();

    public async Task<IEnumerable<Table>> GetAvailableByRestaurantAsync(int restaurantId)
        => await _dbSet
            .Where(t => t.RestaurantId == restaurantId && t.Status == TableStatus.Available)
            .ToListAsync();

    public async Task<Table?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(t => t.Restaurant)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<bool> TableNumberExistsInRestaurantAsync(int restaurantId, int tableNumber, int? excludeId = null)
        => await _dbSet.AnyAsync(t =>
            t.RestaurantId == restaurantId &&
            t.TableNumber == tableNumber &&
            (excludeId == null || t.Id != excludeId));
}
