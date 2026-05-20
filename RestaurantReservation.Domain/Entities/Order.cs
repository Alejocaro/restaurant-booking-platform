using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Entities;

public class Order : AuditBase
{
    public int ReservationId { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? Notes { get; set; }
    public decimal TotalAmount { get; set; }

    public Reservation Reservation { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
