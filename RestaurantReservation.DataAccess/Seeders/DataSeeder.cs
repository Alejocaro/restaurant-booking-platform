using RestaurantReservation.DataAccess.Context;
using RestaurantReservation.Domain.Entities;
using RestaurantReservation.Domain.Enums;

namespace RestaurantReservation.DataAccess.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(RestaurantDbContext context)
    {
        if (context.Restaurants.Any()) return;

        var restaurants = new List<Restaurant>
        {
            new() { Name = "La Palma", Address = "Calle 10 #23-45, Medellín", Phone = "604-111-2222", Email = "lapalma@resto.com", Description = "Cocina tradicional colombiana", Capacity = 60 },
            new() { Name = "El Fogón", Address = "Carrera 70 #45-10, Medellín", Phone = "604-333-4444", Email = "elfogon@resto.com", Description = "Parrilla y asados", Capacity = 80 },
            new() { Name = "Mar y Tierra", Address = "Av. El Poblado #18-30, Medellín", Phone = "604-555-6666", Email = "marytierra@resto.com", Description = "Mariscos y carnes", Capacity = 50 }
        };
        context.Restaurants.AddRange(restaurants);
        await context.SaveChangesAsync();

        var tables = new List<Table>
        {
            new() { TableNumber = 1, Capacity = 2, Status = TableStatus.Available, RestaurantId = restaurants[0].Id },
            new() { TableNumber = 2, Capacity = 4, Status = TableStatus.Available, RestaurantId = restaurants[0].Id },
            new() { TableNumber = 3, Capacity = 6, Status = TableStatus.Available, RestaurantId = restaurants[0].Id },
            new() { TableNumber = 4, Capacity = 8, Status = TableStatus.Available, RestaurantId = restaurants[0].Id },
            new() { TableNumber = 1, Capacity = 2, Status = TableStatus.Available, RestaurantId = restaurants[1].Id },
            new() { TableNumber = 2, Capacity = 4, Status = TableStatus.Available, RestaurantId = restaurants[1].Id },
            new() { TableNumber = 3, Capacity = 6, Status = TableStatus.Reserved, RestaurantId = restaurants[1].Id },
            new() { TableNumber = 1, Capacity = 4, Status = TableStatus.Available, RestaurantId = restaurants[2].Id },
            new() { TableNumber = 2, Capacity = 6, Status = TableStatus.Available, RestaurantId = restaurants[2].Id },
        };
        context.Tables.AddRange(tables);
        await context.SaveChangesAsync();

        var menuItems = new List<MenuItem>
        {
            new() { Name = "Bandeja Paisa", Description = "Plato típico completo", Price = 28000, Category = MenuItemCategory.MainCourse, IsAvailable = true, RestaurantId = restaurants[0].Id },
            new() { Name = "Sancocho de gallina", Description = "Sopa tradicional con gallina", Price = 22000, Category = MenuItemCategory.Soup, IsAvailable = true, RestaurantId = restaurants[0].Id },
            new() { Name = "Empanadas", Description = "3 unidades con ají", Price = 9000, Category = MenuItemCategory.Starter, IsAvailable = true, RestaurantId = restaurants[0].Id },
            new() { Name = "Limonada de coco", Description = "Refrescante bebida tropical", Price = 8000, Category = MenuItemCategory.Beverage, IsAvailable = true, RestaurantId = restaurants[0].Id },
            new() { Name = "Buñuelos con arequipe", Description = "Postre tradicional", Price = 7000, Category = MenuItemCategory.Dessert, IsAvailable = true, RestaurantId = restaurants[0].Id },
            new() { Name = "Churrasco 300g", Description = "Carne a la parrilla con papas", Price = 45000, Category = MenuItemCategory.MainCourse, IsAvailable = true, RestaurantId = restaurants[1].Id },
            new() { Name = "Chorizo santarrosano", Description = "Chorizo asado con arepa", Price = 18000, Category = MenuItemCategory.Starter, IsAvailable = true, RestaurantId = restaurants[1].Id },
            new() { Name = "Costillas BBQ", Description = "Costillas de cerdo con salsa BBQ", Price = 38000, Category = MenuItemCategory.MainCourse, IsAvailable = true, RestaurantId = restaurants[1].Id },
            new() { Name = "Cerveza artesanal", Description = "Variedad local 330ml", Price = 12000, Category = MenuItemCategory.Beverage, IsAvailable = true, RestaurantId = restaurants[1].Id },
            new() { Name = "Cazuela de mariscos", Description = "Mezcla de camarones, calamar y mejillones", Price = 52000, Category = MenuItemCategory.MainCourse, IsAvailable = true, RestaurantId = restaurants[2].Id },
            new() { Name = "Ceviche mixto", Description = "Ceviche frío con limón", Price = 29000, Category = MenuItemCategory.Starter, IsAvailable = true, RestaurantId = restaurants[2].Id },
            new() { Name = "Lomo de res", Description = "250g con guarnición", Price = 49000, Category = MenuItemCategory.MainCourse, IsAvailable = true, RestaurantId = restaurants[2].Id },
            new() { Name = "Ensalada caprese", Description = "Tomate, mozzarella y albahaca", Price = 19000, Category = MenuItemCategory.Salad, IsAvailable = true, RestaurantId = restaurants[2].Id },
        };
        context.MenuItems.AddRange(menuItems);
        await context.SaveChangesAsync();

        var customers = new List<Customer>
        {
            new() { FirstName = "Carlos", LastName = "Gómez", Email = "carlos.gomez@email.com", Phone = "300-111-2222" },
            new() { FirstName = "María", LastName = "Rodríguez", Email = "maria.rodriguez@email.com", Phone = "301-333-4444" },
            new() { FirstName = "Andrés", LastName = "Martínez", Email = "andres.martinez@email.com", Phone = "302-555-6666" },
            new() { FirstName = "Luisa", LastName = "Fernández", Email = "luisa.fernandez@email.com", Phone = "303-777-8888" },
            new() { FirstName = "Pedro", LastName = "López", Email = "pedro.lopez@email.com", Phone = "304-999-0000" },
        };
        context.Customers.AddRange(customers);
        await context.SaveChangesAsync();

        var futureDate1 = DateTime.UtcNow.AddDays(1).Date.AddHours(19);
        var futureDate2 = DateTime.UtcNow.AddDays(2).Date.AddHours(20);
        var futureDate3 = DateTime.UtcNow.AddDays(3).Date.AddHours(13);

        var reservations = new List<Reservation>
        {
            new() { CustomerId = customers[0].Id, TableId = tables[1].Id, ReservationDate = futureDate1, PartySize = 4, Status = ReservationStatus.Confirmed, SpecialRequests = "Silla alta para bebé" },
            new() { CustomerId = customers[1].Id, TableId = tables[2].Id, ReservationDate = futureDate2, PartySize = 5, Status = ReservationStatus.Pending },
            new() { CustomerId = customers[2].Id, TableId = tables[6].Id, ReservationDate = futureDate1, PartySize = 3, Status = ReservationStatus.Confirmed, SpecialRequests = "Celebración de cumpleaños" },
            new() { CustomerId = customers[3].Id, TableId = tables[7].Id, ReservationDate = futureDate3, PartySize = 4, Status = ReservationStatus.Pending },
        };
        context.Reservations.AddRange(reservations);
        await context.SaveChangesAsync();

        var orders = new List<Order>
        {
            new() { ReservationId = reservations[0].Id, Status = OrderStatus.Completed, TotalAmount = 0 },
        };
        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();

        var orderItems = new List<OrderItem>
        {
            new() { OrderId = orders[0].Id, MenuItemId = menuItems[0].Id, Quantity = 2, UnitPrice = menuItems[0].Price },
            new() { OrderId = orders[0].Id, MenuItemId = menuItems[3].Id, Quantity = 2, UnitPrice = menuItems[3].Price },
        };
        context.OrderItems.AddRange(orderItems);

        orders[0].TotalAmount = orderItems.Sum(oi => oi.UnitPrice * oi.Quantity);
        context.Orders.Update(orders[0]);

        await context.SaveChangesAsync();
    }
}
