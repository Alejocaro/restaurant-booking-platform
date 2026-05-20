using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Interfaces.Repositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetByCustomerAsync(int customerId);
    Task<IEnumerable<Reservation>> GetByTableAsync(int tableId);
    Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status);
    Task<Reservation?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Reservation>> GetAllWithDetailsAsync();
    Task<bool> TableHasConflictAsync(int tableId, DateTime reservationDate, int? excludeId = null);
}
