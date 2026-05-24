using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.DTOs.Request;
using RestaurantReservation.API.DTOs.Response;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Interfaces.Services;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantService _service;
    private readonly IMapper _mapper;

    public RestaurantsController(IRestaurantService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<RestaurantResponseDTO>>(restaurants));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var restaurant = await _service.GetByIdAsync(id);
        if (restaurant == null) return NotFound(new { message = $"No se encontró el restaurante con Id {id}." });
        return Ok(_mapper.Map<RestaurantResponseDTO>(restaurant));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RestaurantRequestDTO dto)
    {
        try
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            var created = await _service.CreateAsync(restaurant);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<RestaurantResponseDTO>(created));
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RestaurantRequestDTO dto)
    {
        try
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.Id = id;
            await _service.UpdateAsync(restaurant);
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
