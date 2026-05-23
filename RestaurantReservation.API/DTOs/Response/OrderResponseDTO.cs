using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.API.DTOs.Response;

public class OrderResponseDTO
{
    public int Id { get; set; }
    public int ReservationId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItemResponseDTO> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class OrderItemResponseDTO
{
    public int Id { get; set; }
    public int MenuItemId { get; set; }
    public string MenuItemName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}
