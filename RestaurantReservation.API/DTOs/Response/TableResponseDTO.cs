using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.API.DTOs.Response;

public class TableResponseDTO
{
    public int Id { get; set; }
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public TableStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
