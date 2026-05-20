using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Entities;

public class Reservation : AuditBase
{
    public int CustomerId { get; set; }
    public int TableId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int PartySize { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public string? SpecialRequests { get; set; }

    public Customer Customer { get; set; } = null!;
    public Table Table { get; set; } = null!;
    public Order? Order { get; set; }
}
