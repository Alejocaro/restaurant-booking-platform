using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.Domain.Interfaces.Repositories;

public interface IRestaurantRepository : IGenericRepository<Restaurant>
{
    Task<Restaurant?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Restaurant>> GetAllWithDetailsAsync();
}
