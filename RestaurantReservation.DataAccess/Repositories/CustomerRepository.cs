using Microsoft.EntityFrameworkCore;
using RestaurantReservation.DataAccess.Context;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Interfaces.Repositories;

namespace RestaurantReservation.DataAccess.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(RestaurantDbContext context) : base(context) { }

    public async Task<Customer?> GetByEmailAsync(string email)
        => await _dbSet.FirstOrDefaultAsync(c => c.Email == email);

    public async Task<Customer?> GetByIdWithReservationsAsync(int id)
        => await _dbSet
            .Include(c => c.Reservations)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        => await _dbSet.AnyAsync(c =>
            c.Email == email &&
            (excludeId == null || c.Id != excludeId));
}
