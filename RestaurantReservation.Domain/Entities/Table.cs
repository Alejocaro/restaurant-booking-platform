using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.Domain.Entities;

public class Table : AuditBase
{
    public int TableNumber { get; set; }
    public int Capacity { get; set; }
    public TableStatus Status { get; set; } = TableStatus.Available;
    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
