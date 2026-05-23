namespace RestaurantReservation.API.DTOs.Request;

public class ReservationRequestDTO
{
    public int CustomerId { get; set; }
    public int TableId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }
    public string? SpecialRequests { get; set; }
}
