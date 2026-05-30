import { useEffect, useState } from 'react';
import { reservationService, customerService, tableService, restaurantService } from '../services/api';
import type { Reservation, Customer, Table, Restaurant } from '../types';
import { RESERVATION_STATUS } from '../types';
import StatusBadge from '../components/StatusBadge';

const STATUS_COLOR: Record<number, string> = { 0: 'yellow', 1: 'green', 2: 'red', 3: 'blue', 4: 'gray' };
const empty = { customerId: 0, tableId: 0, reservationDate: '', partySize: 1, specialRequests: '' };

export default function ReservationsPage() {
  const [reservations, setReservations] = useState<Reservation[]>([]);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [tables, setTables] = useState<Table[]>([]);
  const [restaurants, setRestaurants] = useState<Restaurant[]>([]);
  const [selectedRestaurant, setSelectedRestaurant] = useState(0);
  const [filteredTables, setFilteredTables] = useState<Table[]>([]);
  const [form, setForm] = useState(empty);
  const [showForm, setShowForm] = useState(false);
  const [error, setError] = useState('');

  const load = () => reservationService.getAll().then(setReservations);
  useEffect(() => {
    load();
    customerService.getAll().then(setCustomers);
    tableService.getAll().then(setTables);
    restaurantService.getAll().then(setRestaurants);
  }, []);

  useEffect(() => {
    setFilteredTables(selectedRestaurant ? tables.filter(t => t.restaurantId === selectedRestaurant) : tables);
  }, [selectedRestaurant, tables]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault(); setError('');
    try {
      await reservationService.create({ ...form, reservationDate: new Date(form.reservationDate).toISOString() });
      setForm(empty); setShowForm(false); load();
    } catch (err: any) { setError(err.response?.data?.message ?? 'Error al crear reserva'); }
  };

  const handleStatus = async (id: number, status: number) => {
    try {
      await reservationService.updateStatus(id, status);
      load();
    } catch (err: any) {
      alert(err.response?.data?.message ?? 'Error al actualizar el estado');
    }
  };

  const handleDelete = async (id: number) => {
    if (confirm('¿Eliminar reserva?')) {
      try {
        await reservationService.delete(id);
        load();
      } catch (err: any) {
        alert(err.response?.data?.message ?? 'Error al eliminar');
      }
    }
  };

  return (
    <div className="max-w-6xl mx-auto px-4 py-8">
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-2xl font-bold text-gray-800">Reservas</h2>
        <button onClick={() => { setForm(empty); setShowForm(true); }}
          className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 text-sm font-medium">
          + Nueva reserva
        </button>
      </div>

      {showForm && (
        <form onSubmit={handleSubmit} className="bg-white border rounded-xl p-6 mb-6 shadow-sm">
          <h3 className="font-semibold text-gray-700 mb-4">Nueva reserva</h3>
          {error && <p className="text-red-500 text-sm mb-3">{error}</p>}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="text-xs text-gray-500 font-medium">Cliente</label>
              <select required className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={form.customerId} onChange={e => setForm(f => ({...f, customerId: +e.target.value}))}>
                <option value={0}>Seleccionar...</option>
                {customers.map(c => <option key={c.id} value={c.id}>{c.fullName}</option>)}
              </select>
            </div>
            <div>
              <label className="text-xs text-gray-500 font-medium">Restaurante</label>
              <select className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={selectedRestaurant} onChange={e => setSelectedRestaurant(+e.target.value)}>
                <option value={0}>Todos</option>
                {restaurants.map(r => <option key={r.id} value={r.id}>{r.name}</option>)}
              </select>
            </div>
            <div>
              <label className="text-xs text-gray-500 font-medium">Mesa</label>
              <select required className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={form.tableId} onChange={e => setForm(f => ({...f, tableId: +e.target.value}))}>
                <option value={0}>Seleccionar...</option>
                {filteredTables.map(t => (
                  <option key={t.id} value={t.id}>Mesa #{t.tableNumber} — {t.restaurantName} (cap. {t.capacity})</option>
                ))}
              </select>
            </div>
            <div>
              <label className="text-xs text-gray-500 font-medium">Fecha y hora</label>
              <input type="datetime-local" required className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={form.reservationDate} onChange={e => setForm(f => ({...f, reservationDate: e.target.value}))} />
            </div>
            <div>
              <label className="text-xs text-gray-500 font-medium">Personas</label>
              <input type="number" min={1} required className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={form.partySize} onChange={e => setForm(f => ({...f, partySize: +e.target.value}))} />
            </div>
            <div>
              <label className="text-xs text-gray-500 font-medium">Solicitudes especiales</label>
              <input className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={form.specialRequests} onChange={e => setForm(f => ({...f, specialRequests: e.target.value}))} />
            </div>
          </div>
          <div className="flex gap-2 mt-4">
            <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded-lg text-sm font-medium hover:bg-blue-700">Guardar</button>
            <button type="button" onClick={() => setShowForm(false)} className="border px-4 py-2 rounded-lg text-sm hover:bg-gray-50">Cancelar</button>
          </div>
        </form>
      )}

      <div className="bg-white border rounded-xl shadow-sm overflow-hidden">
        <table className="w-full text-sm">
          <thead className="bg-gray-50 text-gray-600 text-xs uppercase">
            <tr>
              {['Cliente','Restaurante','Mesa','Fecha','Personas','Estado','Acciones'].map(h => (
                <th key={h} className="px-4 py-3 text-left">{h}</th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-100">
            {reservations.map(r => (
              <tr key={r.id} className="hover:bg-gray-50">
                <td className="px-4 py-3 font-medium text-gray-800">{r.customerName}</td>
                <td className="px-4 py-3 text-gray-600">{r.restaurantName}</td>
                <td className="px-4 py-3 text-gray-600">#{r.tableNumber}</td>
                <td className="px-4 py-3 text-gray-600">{new Date(r.reservationDate).toLocaleString('es-CO')}</td>
                <td className="px-4 py-3 text-gray-600">{r.partySize}</td>
                <td className="px-4 py-3">
                  <StatusBadge label={RESERVATION_STATUS[r.status as keyof typeof RESERVATION_STATUS]} color={STATUS_COLOR[r.status]} />
                </td>
                <td className="px-4 py-3">
                  <div className="flex gap-2 flex-wrap">
                    {r.status === 0 && <button onClick={() => handleStatus(r.id, 1)} className="text-xs text-green-600 hover:underline">Confirmar</button>}
                    {r.status !== 2 && r.status !== 3 && <button onClick={() => handleStatus(r.id, 2)} className="text-xs text-red-500 hover:underline">Cancelar</button>}
                    {r.status === 1 && <button onClick={() => handleStatus(r.id, 3)} className="text-xs text-blue-600 hover:underline">Completar</button>}
                    <button onClick={() => handleDelete(r.id)} className="text-xs text-gray-400 hover:underline">Eliminar</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
