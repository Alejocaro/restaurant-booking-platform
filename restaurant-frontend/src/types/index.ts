export interface Restaurant {
  id: number;
  name: string;
  address: string;
  phone: string;
  email: string;
  description?: string;
  capacity: number;
  tablesCount: number;
  menuItemsCount: number;
  createdAt: string;
}

export interface Table {
  id: number;
  tableNumber: number;
  capacity: number;
  status: number;
  statusName: string;
  restaurantId: number;
  restaurantName: string;
  createdAt: string;
}

export interface MenuItem {
  id: number;
  name: string;
  description?: string;
  price: number;
  category: number;
  categoryName: string;
  isAvailable: boolean;
  restaurantId: number;
  restaurantName: string;
  createdAt: string;
}

export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phone: string;
  createdAt: string;
}

export interface Reservation {
  id: number;
  customerId: number;
  customerName: string;
  tableId: number;
  tableNumber: number;
  restaurantName: string;
  reservationDate: string;
  partySize: number;
  status: number;
  statusName: string;
  specialRequests?: string;
  createdAt: string;
}

export interface OrderItem {
  id: number;
  menuItemId: number;
  menuItemName: string;
  quantity: number;
  unitPrice: number;
  subtotal: number;
}

export interface Order {
  id: number;
  reservationId: number;
  customerName: string;
  status: number;
  statusName: string;
  notes?: string;
  totalAmount: number;
  items: OrderItem[];
  createdAt: string;
}

export const TABLE_STATUS = { 0: 'Disponible', 1: 'Ocupada', 2: 'Reservada', 3: 'Fuera de servicio' };
export const RESERVATION_STATUS = { 0: 'Pendiente', 1: 'Confirmada', 2: 'Cancelada', 3: 'Completada', 4: 'No presentó' };
export const ORDER_STATUS = { 0: 'Pendiente', 1: 'En proceso', 2: 'Completada', 3: 'Cancelada' };
export const MENU_CATEGORY = { 0: 'Entrada', 1: 'Plato principal', 2: 'Postre', 3: 'Bebida', 4: 'Ensalada', 5: 'Sopa' };
