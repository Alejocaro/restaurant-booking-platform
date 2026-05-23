using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.API.DTOs.Request;

public class TableRequestDTO
{
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public TableStatus Status { get; set; } = TableStatus.Available;
    public int RestaurantId { get; set; }
}
