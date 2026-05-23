using AutoMapper;
using RestaurantReservation.API.DTOs.Request;
using RestaurantReservation.API.DTOs.Response;
using RestaurantReservation.Domain.Entities;

namespace RestaurantReservation.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RestaurantRequestDTO, Restaurant>();
        CreateMap<Restaurant, RestaurantResponseDTO>()
            .ForMember(d => d.TablesCount, o => o.MapFrom(s => s.Tables.Count))
            .ForMember(d => d.MenuItemsCount, o => o.MapFrom(s => s.MenuItems.Count));

        CreateMap<TableRequestDTO, Table>();
        CreateMap<Table, TableResponseDTO>()
            .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.RestaurantName, o => o.MapFrom(s => s.Restaurant != null ? s.Restaurant.Name : string.Empty));

        CreateMap<MenuItemRequestDTO, MenuItem>();
        CreateMap<MenuItem, MenuItemResponseDTO>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.ToString()))
            .ForMember(d => d.RestaurantName, o => o.MapFrom(s => s.Restaurant != null ? s.Restaurant.Name : string.Empty));

        CreateMap<CustomerRequestDTO, Customer>();
        CreateMap<Customer, CustomerResponseDTO>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.FirstName + " " + s.LastName));

        CreateMap<ReservationRequestDTO, Reservation>();
        CreateMap<Reservation, ReservationResponseDTO>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer != null ? s.Customer.FirstName + " " + s.Customer.LastName : string.Empty))
            .ForMember(d => d.TableNumber, o => o.MapFrom(s => s.Table != null ? s.Table.TableNumber : 0))
            .ForMember(d => d.RestaurantName, o => o.MapFrom(s => s.Table != null && s.Table.Restaurant != null ? s.Table.Restaurant.Name : string.Empty))
            .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<OrderItemRequestDTO, OrderItem>();
        CreateMap<OrderItem, OrderItemResponseDTO>()
            .ForMember(d => d.MenuItemName, o => o.MapFrom(s => s.MenuItem != null ? s.MenuItem.Name : string.Empty))
            .ForMember(d => d.Subtotal, o => o.MapFrom(s => s.UnitPrice * s.Quantity));

        CreateMap<Order, OrderResponseDTO>()
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Reservation != null && s.Reservation.Customer != null
                ? s.Reservation.Customer.FirstName + " " + s.Reservation.Customer.LastName
                : string.Empty))
            .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.OrderItems));
    }
}
