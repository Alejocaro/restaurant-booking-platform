using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DataAccess.Context;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Interfaces.Repositories;

namespace RestaurantReservation.DataAccess.Repositories;

public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(RestaurantDbContext context) : base(context) { }

    public async Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId)
        => await _dbSet.Where(oi => oi.OrderId == orderId).ToListAsync();

    public async Task<IEnumerable<OrderItem>> GetByOrderWithDetailsAsync(int orderId)
        => await _dbSet
            .Include(oi => oi.MenuItem)
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync();
}
