using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DataAccess.Context;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Interfaces.Repositories;

namespace RestaurantReservation.DataAccess.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(RestaurantDbContext context) : base(context) { }

    public async Task<Order?> GetByReservationAsync(int reservationId)
        => await _dbSet
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.ReservationId == reservationId);

    public async Task<Order?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(o => o.Reservation)
                .ThenInclude(r => r.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        => await _dbSet
            .Include(o => o.Reservation)
                .ThenInclude(r => r.Customer)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .ToListAsync();
}
