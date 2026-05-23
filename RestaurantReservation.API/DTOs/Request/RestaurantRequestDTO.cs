namespace RestaurantReservation.API.DTOs.Request;

public class RestaurantRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
}
