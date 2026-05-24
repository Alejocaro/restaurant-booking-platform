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
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _service;
    private readonly IMapper _mapper;

    public ReservationsController(IReservationService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reservations = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ReservationResponseDTO>>(reservations));
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomer(int customerId)
    {
        var reservations = await _service.GetByCustomerAsync(customerId);
        return Ok(_mapper.Map<IEnumerable<ReservationResponseDTO>>(reservations));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reservation = await _service.GetByIdAsync(id);
        if (reservation == null) return NotFound(new { message = $"No se encontró la reserva con Id {id}." });
        return Ok(_mapper.Map<ReservationResponseDTO>(reservation));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservationRequestDTO dto)
    {
        try
        {
            var reservation = _mapper.Map<Reservation>(dto);
            var created = await _service.CreateAsync(reservation);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<ReservationResponseDTO>(created));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ReservationRequestDTO dto)
    {
        try
        {
            var reservation = _mapper.Map<Reservation>(dto);
            reservation.Id = id;
            await _service.UpdateAsync(reservation);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] ReservationStatus status)
    {
        try
        {
            await _service.UpdateStatusAsync(id, status);
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
