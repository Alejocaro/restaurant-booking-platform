using Microsoft.Extensions.Logging;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Domain.Interfaces.Repositories;
using RestaurantReservation.Domain.Interfaces.Services;

namespace RestaurantReservation.Domain.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _repository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ILogger<MenuItemService> _logger;

    public MenuItemService(IMenuItemRepository repository, IRestaurantRepository restaurantRepository, ILogger<MenuItemService> logger)
    {
        _repository = repository;
        _restaurantRepository = restaurantRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<IEnumerable<MenuItem>> GetByRestaurantAsync(int restaurantId)
        => await _repository.GetByRestaurantAsync(restaurantId);

    public async Task<IEnumerable<MenuItem>> GetByRestaurantAndCategoryAsync(int restaurantId, MenuItemCategory category)
        => await _repository.GetByRestaurantAndCategoryAsync(restaurantId, category);

    public async Task<MenuItem?> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id);

    public async Task<MenuItem> CreateAsync(MenuItem menuItem)
    {
        if (!await _restaurantRepository.ExistsAsync(menuItem.RestaurantId))
            throw new KeyNotFoundException($"No se encontró el restaurante con Id {menuItem.RestaurantId}.");
        if (string.IsNullOrWhiteSpace(menuItem.Name))
            throw new InvalidOperationException("El nombre del ítem del menú es obligatorio.");
        if (menuItem.Price <= 0)
            throw new InvalidOperationException("El precio debe ser mayor a 0.");

        return await _repository.CreateAsync(menuItem);
    }

    public async Task UpdateAsync(MenuItem menuItem)
    {
        if (!await _repository.ExistsAsync(menuItem.Id))
            throw new KeyNotFoundException($"No se encontró el ítem del menú con Id {menuItem.Id}.");
        if (menuItem.Price <= 0)
            throw new InvalidOperationException("El precio debe ser mayor a 0.");

        await _repository.UpdateAsync(menuItem);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            throw new KeyNotFoundException($"No se encontró el ítem del menú con Id {id}.");

        await _repository.DeleteAsync(id);
    }
}
