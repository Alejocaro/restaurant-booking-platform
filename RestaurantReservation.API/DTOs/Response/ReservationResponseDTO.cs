using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.API.DTOs.Response;

public class ReservationResponseDTO
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int TableId { get; set; }
    public int TableNumber { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }
    public ReservationStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? SpecialRequests { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
