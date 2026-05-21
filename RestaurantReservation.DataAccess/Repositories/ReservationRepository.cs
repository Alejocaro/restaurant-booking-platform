using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DataAccess.Context;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Domain.Interfaces.Repositories;

namespace RestaurantReservation.DataAccess.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(RestaurantDbContext context) : base(context) { }

    public async Task<IEnumerable<Reservation>> GetByCustomerAsync(int customerId)
        => await _dbSet
            .Include(r => r.Table)
            .Where(r => r.CustomerId == customerId)
            .ToListAsync();

    public async Task<IEnumerable<Reservation>> GetByTableAsync(int tableId)
        => await _dbSet.Where(r => r.TableId == tableId).ToListAsync();

    public async Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status)
        => await _dbSet.Where(r => r.Status == status).ToListAsync();

    public async Task<Reservation?> GetByIdWithDetailsAsync(int id)
        => await _dbSet
            .Include(r => r.Customer)
            .Include(r => r.Table)
                .ThenInclude(t => t.Restaurant)
            .Include(r => r.Order)
                .ThenInclude(o => o!.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Reservation>> GetAllWithDetailsAsync()
        => await _dbSet
            .Include(r => r.Customer)
            .Include(r => r.Table)
                .ThenInclude(t => t.Restaurant)
            .ToListAsync();

    public async Task<bool> TableHasConflictAsync(int tableId, DateTime reservationDate, int? excludeId = null)
    {
        var from = reservationDate.AddHours(-2);
        var to = reservationDate.AddHours(2);
        return await _dbSet.AnyAsync(r =>
            r.TableId == tableId &&
            r.Status != ReservationStatus.Cancelled &&
            r.Status != ReservationStatus.NoShow &&
            r.ReservationDate >= from &&
            r.ReservationDate <= to &&
            (excludeId == null || r.Id != excludeId));
    }
}
