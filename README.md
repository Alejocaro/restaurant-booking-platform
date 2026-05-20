# Sistema de Reservas de Restaurante

Proyecto final de Programación Web — ITM 2026

## Integrantes
- Alejandro caro gómez

## Descripción

Sistema web full-stack para la gestión de reservas de restaurantes. Permite administrar restaurantes, mesas, menús, clientes, reservas y órdenes.

## Tecnologías

| Capa       | Tecnología                        |
|------------|-----------------------------------|
| Backend    | .NET 8 Web API                    |
| ORM        | Entity Framework Core 8 + SQLite  |
| Frontend   | React 19 + TypeScript + Tailwind  |
| Docs API   | Swagger / OpenAPI                 |

## Entidades

- **Restaurant** → tiene muchas **Tables** (1:N) y muchos **MenuItems** (1:N)
- **Customer** → tiene muchas **Reservations** (1:N)
- **Table** → tiene muchas **Reservations** (1:N)
- **Reservation** → tiene una **Order** (1:1)
- **Order** ↔ **MenuItem** a través de **OrderItem** (N:M)

## Instrucciones para ejecutar

### Backend

```bash
cd RestaurantReservation.API
dotnet run
```

La API arranca en `http://localhost:5158`.  
Swagger disponible en `http://localhost:5158/swagger`.

> La base de datos SQLite (`restaurant.db`) se crea y pobla automáticamente al primer inicio.

### Frontend

```bash
cd restaurant-frontend
npm install
npm run dev
```

La app arranca en `http://localhost:5173`.

## Arquitectura del Backend

```
RestaurantReservation.Domain/
├── Entities/          ← 7 entidades (AuditBase, Restaurant, Table, MenuItem,
│                                     Customer, Reservation, Order, OrderItem)
├── Enums/             ← TableStatus, ReservationStatus, MenuItemCategory, OrderStatus
├── Interfaces/
│   ├── Repositories/  ← IGenericRepository<T> + interfaces específicas
│   └── Services/      ← Interfaces de servicios de dominio
└── Services/          ← Lógica de negocio con validaciones

RestaurantReservation.DataAccess/
├── Context/           ← RestaurantDbContext (EF Core Code-First)
├── Repositories/      ← GenericRepository<T> + repositorios específicos
├── Seeders/           ← DataSeeder con datos iniciales
└── Migrations/        ← Migraciones generadas por EF Core

RestaurantReservation.API/
├── Controllers/       ← 6 controllers REST
├── DTOs/              ← Request y Response DTOs
├── Mapping/           ← MappingProfile (AutoMapper)
└── Program.cs         ← Registro de servicios + Swagger + CORS
```

## Endpoints principales

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/restaurants | Listar restaurantes |
| POST | /api/restaurants | Crear restaurante |
| GET | /api/tables/restaurant/{id}/available | Mesas disponibles |
| PATCH | /api/tables/{id}/status | Cambiar estado de mesa |
| GET | /api/menuitems/restaurant/{id} | Menú del restaurante |
| GET | /api/reservations | Listar reservas |
| POST | /api/reservations | Crear reserva |
| PATCH | /api/reservations/{id}/status | Confirmar/cancelar reserva |
| POST | /api/orders | Crear orden con ítems |
| PATCH | /api/orders/{id}/status | Actualizar estado de orden |
