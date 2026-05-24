using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.Request;
using RestaurantReservation.API.DTOs.Response;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;
using RestaurantReservation.Domain.Interfaces.Services;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<OrderResponseDTO>>(orders));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _service.GetByIdAsync(id);
        if (order == null) return NotFound(new { message = $"No se encontró la orden con Id {id}." });
        return Ok(_mapper.Map<OrderResponseDTO>(order));
    }

    [HttpGet("reservation/{reservationId}")]
    public async Task<IActionResult> GetByReservation(int reservationId)
    {
        var order = await _service.GetByReservationAsync(reservationId);
        if (order == null) return NotFound(new { message = $"No se encontró una orden para la reserva {reservationId}." });
        return Ok(_mapper.Map<OrderResponseDTO>(order));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderRequestDTO dto)
    {
        try
        {
            var order = new Order { ReservationId = dto.ReservationId, Notes = dto.Notes };
            var items = _mapper.Map<IEnumerable<OrderItem>>(dto.Items);
            var created = await _service.CreateAsync(order, items);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<OrderResponseDTO>(created));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatus status)
    {
        try
        {
            await _service.UpdateStatusAsync(id, status);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPost("{id}/items")]
    public async Task<IActionResult> AddItem(int id, [FromBody] OrderItemRequestDTO dto)
    {
        try
        {
            var item = _mapper.Map<OrderItem>(dto);
            await _service.AddItemAsync(id, item);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpDelete("{id}/items/{itemId}")]
    public async Task<IActionResult> RemoveItem(int id, int itemId)
    {
        try
        {
            await _service.RemoveItemAsync(id, itemId);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
