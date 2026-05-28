import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { restaurantService } from '../services/api';
import type { Restaurant } from '../types';

const empty = { name: '', address: '', phone: '', email: '', description: '', capacity: 0 };

export default function RestaurantsPage() {
  const [restaurants, setRestaurants] = useState<Restaurant[]>([]);
  const [form, setForm] = useState(empty);
  const [editing, setEditing] = useState<number | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [error, setError] = useState('');

  const load = () => restaurantService.getAll().then(setRestaurants);
  useEffect(() => { load(); }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      if (editing) await restaurantService.update(editing, form);
      else await restaurantService.create(form);
      setForm(empty); setEditing(null); setShowForm(false); load();
    } catch (err: any) {
      setError(err.response?.data?.message ?? 'Error al guardar');
    }
  };

  const handleEdit = (r: Restaurant) => {
    setForm({ name: r.name, address: r.address, phone: r.phone, email: r.email, description: r.description ?? '', capacity: r.capacity });
    setEditing(r.id); setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (confirm('¿Eliminar restaurante?')) { await restaurantService.delete(id); load(); }
  };

  return (
    <div className="max-w-6xl mx-auto px-4 py-8">
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-2xl font-bold text-gray-800">Restaurantes</h2>
        <button onClick={() => { setForm(empty); setEditing(null); setShowForm(true); }}
          className="bg-zinc-900 text-white px-4 py-2 rounded-lg hover:bg-zinc-800 text-sm font-medium">
          + Nuevo restaurante
        </button>
      </div>

      {showForm && (
        <form onSubmit={handleSubmit} className="bg-white border border-gray-200 rounded-xl p-6 mb-6 shadow-sm">
          <h3 className="font-semibold text-gray-700 mb-4">{editing ? 'Editar' : 'Nuevo'} restaurante</h3>
          {error && <p className="text-red-500 text-sm mb-3">{error}</p>}
          <div className="grid grid-cols-2 gap-4">
            {[['name','Nombre'], ['address','Dirección'], ['phone','Teléfono'], ['email','Email']].map(([k,l]) => (
              <div key={k}>
                <label className="text-xs text-gray-500 font-medium">{l}</label>
                <input required className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                  value={(form as any)[k]} onChange={e => setForm(f => ({...f, [k]: e.target.value}))} />
              </div>
            ))}
            <div>
              <label className="text-xs text-gray-500 font-medium">Capacidad</label>
              <input type="number" required className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={form.capacity} onChange={e => setForm(f => ({...f, capacity: +e.target.value}))} />
            </div>
            <div>
              <label className="text-xs text-gray-500 font-medium">Descripción</label>
              <input className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                value={form.description} onChange={e => setForm(f => ({...f, description: e.target.value}))} />
            </div>
          </div>
          <div className="flex gap-2 mt-4">
            <button type="submit" className="bg-zinc-900 text-white px-4 py-2 rounded-lg text-sm font-medium hover:bg-zinc-800">Guardar</button>
            <button type="button" onClick={() => setShowForm(false)} className="border px-4 py-2 rounded-lg text-sm hover:bg-gray-50">Cancelar</button>
          </div>
        </form>
      )}

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {restaurants.map(r => (
          <div key={r.id} className="bg-white border border-gray-200 rounded-xl p-5 shadow-sm hover:shadow-md transition">
            <div className="flex justify-between items-start mb-2">
              <h3 className="font-semibold text-gray-800">{r.name}</h3>
              <span className="text-xs bg-zinc-100 text-zinc-700 px-2 py-0.5 rounded-full">Cap. {r.capacity}</span>
            </div>
            <p className="text-xs text-gray-500 mb-1">{r.address}</p>
            <p className="text-xs text-gray-500 mb-3">{r.email} · {r.phone}</p>
            <div className="flex gap-3 text-xs text-gray-500 mb-3">
              <span>{r.tablesCount} mesas</span>
              <span>{r.menuItemsCount} ítems de menú</span>
            </div>
            <div className="flex gap-2">
              <Link to={`/restaurants/${r.id}`} className="text-xs text-blue-600 hover:underline">Ver detalle</Link>
              <button onClick={() => handleEdit(r)} className="text-xs text-zinc-700 hover:underline">Editar</button>
              <button onClick={() => handleDelete(r.id)} className="text-xs text-red-500 hover:underline">Eliminar</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
