using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DataAccess.Context;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Interfaces.Repositories;

namespace RestaurantReservation.DataAccess.Repositories;

public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
{
    public RestaurantRepository(RestaurantDbContext context) : base(context) { }

    public async Task<Restaurant?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(r => r.Tables)
            .Include(r => r.MenuItems)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Restaurant>> GetAllWithDetailsAsync()
        => await _dbSet
            .Include(r => r.Tables)
            .Include(r => r.MenuItems)
            .ToListAsync();
}
