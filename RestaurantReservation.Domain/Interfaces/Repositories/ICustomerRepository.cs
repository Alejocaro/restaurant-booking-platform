using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Domain.Interfaces.Repositories;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByIdWithReservationsAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
}
