using Microsoft.Extensions.Logging;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Domain.Interfaces.Repositories;
using RestaurantReservation.Domain.Interfaces.Services;

namespace RestaurantReservation.Domain.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _repository;
    private readonly ITableRepository _tableRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        IReservationRepository repository,
        ITableRepository tableRepository,
        ICustomerRepository customerRepository,
        ILogger<ReservationService> logger)
    {
        _repository = repository;
        _tableRepository = tableRepository;
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
        => await _repository.GetAllWithDetailsAsync();

    public async Task<IEnumerable<Reservation>> GetByCustomerAsync(int customerId)
        => await _repository.GetByCustomerAsync(customerId);

    public async Task<Reservation?> GetByIdAsync(int id)
        => await _repository.GetByIdWithDetailsAsync(id);

    public async Task<Reservation> CreateAsync(Reservation reservation)
    {
        if (!await _customerRepository.ExistsAsync(reservation.CustomerId))
            throw new KeyNotFoundException($"No se encontró el cliente con Id {reservation.CustomerId}.");

        var table = await _tableRepository.GetByIdAsync(reservation.TableId)
            ?? throw new KeyNotFoundException($"No se encontró la mesa con Id {reservation.TableId}.");

        if (table.Status == TableStatus.OutOfService)
            throw new InvalidOperationException("La mesa está fuera de servicio y no puede ser reservada.");
        if (reservation.PartySize > table.Capacity)
            throw new InvalidOperationException($"El tamaño del grupo ({reservation.PartySize}) supera la capacidad de la mesa ({table.Capacity}).");
        if (reservation.ReservationDate <= DateTime.UtcNow)
            throw new InvalidOperationException("La fecha de reserva debe ser en el futuro.");
        if (await _repository.TableHasConflictAsync(reservation.TableId, reservation.ReservationDate))
            throw new InvalidOperationException("La mesa ya tiene una reserva confirmada en esa fecha y hora.");

        var created = await _repository.CreateAsync(reservation);

        table.Status = TableStatus.Reserved;
        await _tableRepository.UpdateAsync(table);

        return created;
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        if (!await _repository.ExistsAsync(reservation.Id))
            throw new KeyNotFoundException($"No se encontró la reserva con Id {reservation.Id}.");

        await _repository.UpdateAsync(reservation);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            throw new KeyNotFoundException($"No se encontró la reserva con Id {id}.");

        await _repository.DeleteAsync(id);
    }

    public async Task UpdateStatusAsync(int id, ReservationStatus status)
    {
        var reservation = await _repository.GetByIdWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró la reserva con Id {id}.");

        reservation.Status = status;
        await _repository.UpdateAsync(reservation);

        if (status == ReservationStatus.Cancelled || status == ReservationStatus.Completed || status == ReservationStatus.NoShow)
        {
            var table = await _tableRepository.GetByIdAsync(reservation.TableId);
            if (table != null)
            {
                table.Status = TableStatus.Available;
                await _tableRepository.UpdateAsync(table);
            }
        }
        else if (status == ReservationStatus.Confirmed)
        {
            var table = await _tableRepository.GetByIdAsync(reservation.TableId);
            if (table != null)
            {
                table.Status = TableStatus.Reserved;
                await _tableRepository.UpdateAsync(table);
            }
        }
    }
}
