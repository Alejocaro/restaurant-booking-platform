import { useEffect, useState } from 'react';
import { orderService, reservationService, menuItemService } from '../services/api';
import type { Order, Reservation, MenuItem } from '../types';
import { ORDER_STATUS } from '../types';
import StatusBadge from '../components/StatusBadge';

const STATUS_COLOR: Record<number, string> = { 0: 'yellow', 1: 'blue', 2: 'green', 3: 'red' };

export default function OrdersPage() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [reservations, setReservations] = useState<Reservation[]>([]);
  const [menuItems, setMenuItems] = useState<MenuItem[]>([]);
  const [showForm, setShowForm] = useState(false);
  const [form, setForm] = useState({ reservationId: 0, notes: '', items: [{ menuItemId: 0, quantity: 1 }] });
  const [error, setError] = useState('');
  const [expanded, setExpanded] = useState<number | null>(null);

  const load = () => orderService.getAll().then(setOrders);
  useEffect(() => {
    load();
    reservationService.getAll().then(r => setReservations(r.filter((res: Reservation) => res.status === 1)));
    menuItemService.getAll().then(setMenuItems);
  }, []);

  const addItem = () => setForm(f => ({ ...f, items: [...f.items, { menuItemId: 0, quantity: 1 }] }));
  const removeItem = (i: number) => setForm(f => ({ ...f, items: f.items.filter((_, idx) => idx !== i) }));

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault(); setError('');
    try {
      await orderService.create({ ...form, items: form.items.filter(i => i.menuItemId > 0) });
      setForm({ reservationId: 0, notes: '', items: [{ menuItemId: 0, quantity: 1 }] });
      setShowForm(false); load();
    } catch (err: any) { setError(err.response?.data?.message ?? 'Error al crear orden'); }
  };

  const handleStatus = async (id: number, status: number) => {
    await orderService.updateStatus(id, status); load();
  };

  const handleDelete = async (id: number) => {
    if (confirm('¿Eliminar orden?')) { await orderService.delete(id); load(); }
  };

  return (
    <div className="max-w-6xl mx-auto px-4 py-8">
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-2xl font-bold text-gray-800">Órdenes</h2>
        <button onClick={() => setShowForm(true)}
          className="bg-purple-600 text-white px-4 py-2 rounded-lg hover:bg-purple-700 text-sm font-medium">
          + Nueva orden
        </button>
      </div>

      {showForm && (
        <form onSubmit={handleSubmit} className="bg-white border rounded-xl p-6 mb-6 shadow-sm">
          <h3 className="font-semibold text-gray-700 mb-4">Nueva orden</h3>
          {error && <p className="text-red-500 text-sm mb-3">{error}</p>}
          <div className="grid grid-cols-2 gap-4 mb-4">
            <div>
              <label className="text-xs text-gray-500 font-medium">Reserva confirmada</label>
              <select required className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={form.reservationId} onChange={e => setForm(f => ({...f, reservationId: +e.target.value}))}>
                <option value={0}>Seleccionar...</option>
                {reservations.map(r => (
                  <option key={r.id} value={r.id}>#{r.id} — {r.customerName} — {r.restaurantName}</option>
                ))}
              </select>
            </div>
            <div>
              <label className="text-xs text-gray-500 font-medium">Notas</label>
              <input className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={form.notes} onChange={e => setForm(f => ({...f, notes: e.target.value}))} />
            </div>
          </div>
          <div className="mb-3">
            <div className="flex justify-between items-center mb-2">
              <label className="text-xs text-gray-500 font-medium">Ítems</label>
              <button type="button" onClick={addItem} className="text-xs text-purple-600 hover:underline">+ Agregar ítem</button>
            </div>
            {form.items.map((item, i) => (
              <div key={i} className="flex gap-2 mb-2">
                <select required className="flex-1 border rounded-lg px-3 py-2 text-sm"
                  value={item.menuItemId} onChange={e => setForm(f => ({ ...f, items: f.items.map((it, idx) => idx === i ? {...it, menuItemId: +e.target.value} : it) }))}>
                  <option value={0}>Seleccionar ítem...</option>
                  {menuItems.filter(m => m.isAvailable).map(m => (
                    <option key={m.id} value={m.id}>{m.name} (${m.price.toLocaleString()})</option>
                  ))}
                </select>
                <input type="number" min={1} className="w-20 border rounded-lg px-3 py-2 text-sm"
                  value={item.quantity} onChange={e => setForm(f => ({ ...f, items: f.items.map((it, idx) => idx === i ? {...it, quantity: +e.target.value} : it) }))} />
                {form.items.length > 1 && (
                  <button type="button" onClick={() => removeItem(i)} className="text-red-400 hover:text-red-600 text-sm px-2">✕</button>
                )}
              </div>
            ))}
          </div>
          <div className="flex gap-2">
            <button type="submit" className="bg-purple-600 text-white px-4 py-2 rounded-lg text-sm font-medium hover:bg-purple-700">Crear orden</button>
            <button type="button" onClick={() => setShowForm(false)} className="border px-4 py-2 rounded-lg text-sm hover:bg-gray-50">Cancelar</button>
          </div>
        </form>
      )}

      <div className="space-y-3">
        {orders.map(o => (
          <div key={o.id} className="bg-white border rounded-xl shadow-sm overflow-hidden">
            <div className="flex items-center justify-between px-5 py-4 cursor-pointer hover:bg-gray-50"
              onClick={() => setExpanded(expanded === o.id ? null : o.id)}>
              <div className="flex items-center gap-4">
                <span className="font-semibold text-gray-700">Orden #{o.id}</span>
                <span className="text-sm text-gray-500">{o.customerName}</span>
                <StatusBadge label={ORDER_STATUS[o.status as keyof typeof ORDER_STATUS]} color={STATUS_COLOR[o.status]} />
              </div>
              <div className="flex items-center gap-4">
                <span className="font-semibold text-green-700">${o.totalAmount.toLocaleString()}</span>
                <div className="flex gap-2">
                  {o.status === 0 && <button onClick={e => { e.stopPropagation(); handleStatus(o.id, 1); }} className="text-xs text-blue-600 hover:underline">Iniciar</button>}
                  {o.status === 1 && <button onClick={e => { e.stopPropagation(); handleStatus(o.id, 2); }} className="text-xs text-green-600 hover:underline">Completar</button>}
                  {o.status !== 2 && o.status !== 3 && <button onClick={e => { e.stopPropagation(); handleStatus(o.id, 3); }} className="text-xs text-red-500 hover:underline">Cancelar</button>}
                  <button onClick={e => { e.stopPropagation(); handleDelete(o.id); }} className="text-xs text-gray-400 hover:underline">Eliminar</button>
                </div>
              </div>
            </div>
            {expanded === o.id && (
              <div className="border-t px-5 py-3 bg-gray-50">
                {o.notes && <p className="text-xs text-gray-500 mb-2">Nota: {o.notes}</p>}
                <table className="w-full text-sm">
                  <thead className="text-xs text-gray-500">
                    <tr><th className="text-left py-1">Ítem</th><th className="text-right">Cant.</th><th className="text-right">Precio</th><th className="text-right">Subtotal</th></tr>
                  </thead>
                  <tbody>
                    {o.items.map(item => (
                      <tr key={item.id} className="border-t border-gray-100">
                        <td className="py-1">{item.menuItemName}</td>
                        <td className="text-right">{item.quantity}</td>
                        <td className="text-right">${item.unitPrice.toLocaleString()}</td>
                        <td className="text-right font-medium">${item.subtotal.toLocaleString()}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}
