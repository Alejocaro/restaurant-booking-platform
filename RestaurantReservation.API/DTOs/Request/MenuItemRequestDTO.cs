using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.API.DTOs.Request;

public class MenuItemRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public MenuItemCategory Category { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int RestaurantId { get; set; }
}
