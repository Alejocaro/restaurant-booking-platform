using Microsoft.Extensions.Logging;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Interfaces.Repositories;
using RestaurantReservation.Domain.Interfaces.Services;

namespace RestaurantReservation.Domain.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(ICustomerRepository repository, ILogger<CustomerService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<Customer?> GetByIdAsync(int id)
        => await _repository.GetByIdWithReservationsAsync(id);

    public async Task<Customer> CreateAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Email))
            throw new InvalidOperationException("El email del cliente es obligatorio.");
        if (await _repository.EmailExistsAsync(customer.Email))
            throw new InvalidOperationException($"Ya existe un cliente con el email '{customer.Email}'.");

        return await _repository.CreateAsync(customer);
    }

    public async Task UpdateAsync(Customer customer)
    {
        if (!await _repository.ExistsAsync(customer.Id))
            throw new KeyNotFoundException($"No se encontró el cliente con Id {customer.Id}.");
        if (await _repository.EmailExistsAsync(customer.Email, customer.Id))
            throw new InvalidOperationException($"Ya existe otro cliente con el email '{customer.Email}'.");

        await _repository.UpdateAsync(customer);
    }

    public async Task DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
            throw new KeyNotFoundException($"No se encontró el cliente con Id {id}.");

        await _repository.DeleteAsync(id);
    }
}
