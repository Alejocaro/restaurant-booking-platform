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
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _service;
    private readonly IMapper _mapper;

    public MenuItemsController(IMenuItemService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _service.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<MenuItemResponseDTO>>(items));
    }

    [HttpGet("restaurant/{restaurantId}")]
    public async Task<IActionResult> GetByRestaurant(int restaurantId, [FromQuery] MenuItemCategory? category = null)
    {
        IEnumerable<MenuItem> items;
        if (category.HasValue)
            items = await _service.GetByRestaurantAndCategoryAsync(restaurantId, category.Value);
        else
            items = await _service.GetByRestaurantAsync(restaurantId);

        return Ok(_mapper.Map<IEnumerable<MenuItemResponseDTO>>(items));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound(new { message = $"No se encontró el ítem con Id {id}." });
        return Ok(_mapper.Map<MenuItemResponseDTO>(item));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MenuItemRequestDTO dto)
    {
        try
        {
            var item = _mapper.Map<MenuItem>(dto);
            var created = await _service.CreateAsync(item);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<MenuItemResponseDTO>(created));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MenuItemRequestDTO dto)
    {
        try
        {
            var item = _mapper.Map<MenuItem>(dto);
            item.Id = id;
            await _service.UpdateAsync(item);
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
