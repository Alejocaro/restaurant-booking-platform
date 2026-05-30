import axios from 'axios';

const api = axios.create({ baseURL: 'http://localhost:5088/api' });

const patchJson = (url: string, value: number) =>
  api.patch(url, JSON.stringify(value), { headers: { 'Content-Type': 'application/json' } });

export const restaurantService = {
  getAll: () => api.get('/restaurants').then(r => r.data),
  getById: (id: number) => api.get(`/restaurants/${id}`).then(r => r.data),
  create: (data: object) => api.post('/restaurants', data).then(r => r.data),
  update: (id: number, data: object) => api.put(`/restaurants/${id}`, data),
  delete: (id: number) => api.delete(`/restaurants/${id}`),
};

export const tableService = {
  getAll: () => api.get('/tables').then(r => r.data),
  getByRestaurant: (id: number) => api.get(`/tables/restaurant/${id}`).then(r => r.data),
  getById: (id: number) => api.get(`/tables/${id}`).then(r => r.data),
  create: (data: object) => api.post('/tables', data).then(r => r.data),
  update: (id: number, data: object) => api.put(`/tables/${id}`, data),
  updateStatus: (id: number, status: number) => patchJson(`/tables/${id}/status`, status),
  delete: (id: number) => api.delete(`/tables/${id}`),
};

export const menuItemService = {
  getAll: () => api.get('/menuitems').then(r => r.data),
  getByRestaurant: (id: number) => api.get(`/menuitems/restaurant/${id}`).then(r => r.data),
  getById: (id: number) => api.get(`/menuitems/${id}`).then(r => r.data),
  create: (data: object) => api.post('/menuitems', data).then(r => r.data),
  update: (id: number, data: object) => api.put(`/menuitems/${id}`, data),
  delete: (id: number) => api.delete(`/menuitems/${id}`),
};

export const customerService = {
  getAll: () => api.get('/customers').then(r => r.data),
  getById: (id: number) => api.get(`/customers/${id}`).then(r => r.data),
  create: (data: object) => api.post('/customers', data).then(r => r.data),
  update: (id: number, data: object) => api.put(`/customers/${id}`, data),
  delete: (id: number) => api.delete(`/customers/${id}`),
};

export const reservationService = {
  getAll: () => api.get('/reservations').then(r => r.data),
  getByCustomer: (id: number) => api.get(`/reservations/customer/${id}`).then(r => r.data),
  getById: (id: number) => api.get(`/reservations/${id}`).then(r => r.data),
  create: (data: object) => api.post('/reservations', data).then(r => r.data),
  update: (id: number, data: object) => api.put(`/reservations/${id}`, data),
  updateStatus: (id: number, status: number) => patchJson(`/reservations/${id}/status`, status),
  delete: (id: number) => api.delete(`/reservations/${id}`),
};

export const orderService = {
  getAll: () => api.get('/orders').then(r => r.data),
  getById: (id: number) => api.get(`/orders/${id}`).then(r => r.data),
  getByReservation: (id: number) => api.get(`/orders/reservation/${id}`).then(r => r.data),
  create: (data: object) => api.post('/orders', data).then(r => r.data),
  updateStatus: (id: number, status: number) => patchJson(`/orders/${id}/status`, status),
  delete: (id: number) => api.delete(`/orders/${id}`),
};
