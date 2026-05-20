using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Entities;

public class MenuItem : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public MenuItemCategory Category { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
