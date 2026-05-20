using Microsoft.Extensions.Logging;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Domain.Interfaces.Repositories;
using RestaurantReservation.Domain.Interfaces.Services;

namespace RestaurantReservation.Domain.Services;

public class TableService : ITableService
{
    private readonly ITableRepository _repository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ILogger<TableService> _logger;

    public TableService(ITableRepository repository, IRestaurantRepository restaurantRepository, ILogger<TableService> logger)
    {
        _repository = repository;
        _restaurantRepository = restaurantRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Table>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<IEnumerable<Table>> GetByRestaurantAsync(int restaurantId)
        => await _repository.GetByRestaurantAsync(restaurantId);

    public async Task<IEnumerable<Table>> GetAvailableByRestaurantAsync(int restaurantId)
        => await _repository.GetAvailableByRestaurantAsync(restaurantId);

    public async Task<Table?> GetByIdAsync(int id)
        => await _repository.GetByIdWithDetailsAsync(id);

    public async Task<Table> CreateAsync(Table table)
    {
        if (!await _restaurantRepository.ExistsAsync(table.RestaurantId))
            throw new KeyNotFoundException($"No se encontró el restaurante con Id {table.RestaurantId}.");
        if (table.Capacity <= 0)
            throw new InvalidOperationException("La capacidad de la mesa debe ser mayor a 0.");
        if (await _repository.TableNumberExistsInRestaurantAsync(table.RestaurantId, table.TableNumber))
            throw new InvalidOperationException($"Ya existe una mesa con el número {table.TableNumber} en este restaurante.");

        return await _repository.CreateAsync(table);
    }

    public async Task UpdateAsync(Table table)
    {
        if (!await _repository.ExistsAsync(table.Id))
            throw new KeyNotFoundException($"No se encontró la mesa con Id {table.Id}.");
        if (table.Capacity <= 0)
            throw new InvalidOperationException("La capacidad de la mesa debe ser mayor a 0.");
        if (await _repository.TableNumberExistsInRestaurantAsync(table.RestaurantId, table.TableNumber, table.Id))
            throw new InvalidOperationException($"Ya existe otra mesa con el número {table.TableNumber} en este restaurante.");

        await _repository.UpdateAsync(table);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            throw new KeyNotFoundException($"No se encontró la mesa con Id {id}.");

        await _repository.DeleteAsync(id);
    }

    public async Task UpdateStatusAsync(int id, TableStatus status)
    {
        var table = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró la mesa con Id {id}.");

        table.Status = status;
        await _repository.UpdateAsync(table);
    }
}
