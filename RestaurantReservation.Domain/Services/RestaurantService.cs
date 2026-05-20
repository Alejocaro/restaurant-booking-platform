using Microsoft.Extensions.Logging;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Interfaces.Repositories;
using RestaurantReservation.Domain.Interfaces.Services;

namespace RestaurantReservation.Domain.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _repository;
    private readonly ILogger<RestaurantService> _logger;

    public RestaurantService(IRestaurantRepository repository, ILogger<RestaurantService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
        => await _repository.GetAllWithDetailsAsync();

    public async Task<Restaurant?> GetByIdAsync(int id)
        => await _repository.GetByIdWithDetailsAsync(id);

    public async Task<Restaurant> CreateAsync(Restaurant restaurant)
    {
        if (string.IsNullOrWhiteSpace(restaurant.Name))
            throw new InvalidOperationException("El nombre del restaurante es obligatorio.");
        if (restaurant.Capacity <= 0)
            throw new InvalidOperationException("La capacidad del restaurante debe ser mayor a 0.");

        return await _repository.CreateAsync(restaurant);
    }

    public async Task UpdateAsync(Restaurant restaurant)
    {
        if (!await _repository.ExistsAsync(restaurant.Id))
            throw new KeyNotFoundException($"No se encontró el restaurante con Id {restaurant.Id}.");
        if (string.IsNullOrWhiteSpace(restaurant.Name))
            throw new InvalidOperationException("El nombre del restaurante es obligatorio.");

        await _repository.UpdateAsync(restaurant);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            throw new KeyNotFoundException($"No se encontró el restaurante con Id {id}.");

        await _repository.DeleteAsync(id);
    }
}
