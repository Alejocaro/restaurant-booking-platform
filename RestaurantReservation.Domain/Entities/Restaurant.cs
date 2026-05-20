namespace RestaurantReservation.Domain.Entities;

public class Restaurant : AuditBase
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }

    public ICollection<Table> Tables { get; set; } = new List<Table>();
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
