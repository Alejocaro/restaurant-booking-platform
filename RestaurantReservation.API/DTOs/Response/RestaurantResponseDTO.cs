namespace RestaurantReservation.API.DTOs.Response;

public class RestaurantResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public int TablesCount { get; set; }
    public int MenuItemsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
