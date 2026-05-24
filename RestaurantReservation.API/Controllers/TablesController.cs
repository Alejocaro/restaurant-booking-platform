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
public class TablesController : ControllerBase
{
    private readonly ITableService _service;
    private readonly IMapper _mapper;

    public TablesController(ITableService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tables = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TableResponseDTO>>(tables));
    }

    [HttpGet("restaurant/{restaurantId}")]
    public async Task<IActionResult> GetByRestaurant(int restaurantId)
    {
        var tables = await _service.GetByRestaurantAsync(restaurantId);
        return Ok(_mapper.Map<IEnumerable<TableResponseDTO>>(tables));
    }

    [HttpGet("restaurant/{restaurantId}/available")]
    public async Task<IActionResult> GetAvailableByRestaurant(int restaurantId)
    {
        var tables = await _service.GetAvailableByRestaurantAsync(restaurantId);
        return Ok(_mapper.Map<IEnumerable<TableResponseDTO>>(tables));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var table = await _service.GetByIdAsync(id);
        if (table == null) return NotFound(new { message = $"No se encontró la mesa con Id {id}." });
        return Ok(_mapper.Map<TableResponseDTO>(table));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TableRequestDTO dto)
    {
        try
        {
            var table = _mapper.Map<Table>(dto);
            var created = await _service.CreateAsync(table);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<TableResponseDTO>(created));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TableRequestDTO dto)
    {
        try
        {
            var table = _mapper.Map<Table>(dto);
            table.Id = id;
            await _service.UpdateAsync(table);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TableStatus status)
    {
        try
        {
            await _service.UpdateStatusAsync(id, status);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
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
