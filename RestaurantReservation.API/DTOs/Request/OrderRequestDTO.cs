namespace RestaurantReservation.API.DTOs.Request;

public class OrderRequestDTO
{
    public int ReservationId { get; set; }
    public string? Notes { get; set; }
    public List<OrderItemRequestDTO> Items { get; set; } = new();
}

public class OrderItemRequestDTO
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}
