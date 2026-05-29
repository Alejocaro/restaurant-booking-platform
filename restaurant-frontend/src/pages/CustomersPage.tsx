import { useEffect, useState } from 'react';
import { customerService } from '../services/api';
import type { Customer } from '../types';

const empty = { firstName: '', lastName: '', email: '', phone: '' };

export default function CustomersPage() {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [form, setForm] = useState(empty);
  const [editing, setEditing] = useState<number | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [error, setError] = useState('');

  const load = () => customerService.getAll().then(setCustomers);
  useEffect(() => { load(); }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault(); setError('');
    try {
      if (editing) await customerService.update(editing, form);
      else await customerService.create(form);
      setForm(empty); setEditing(null); setShowForm(false); load();
    } catch (err: any) { setError(err.response?.data?.message ?? 'Error al guardar'); }
  };

  const handleEdit = (c: Customer) => {
    setForm({ firstName: c.firstName, lastName: c.lastName, email: c.email, phone: c.phone });
    setEditing(c.id); setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (confirm('¿Eliminar cliente?')) { await customerService.delete(id); load(); }
  };

  return (
    <div className="max-w-5xl mx-auto px-4 py-8">
      <div className="flex items-center justify-between mb-6">
        <h2 className="text-2xl font-bold text-gray-800">Clientes</h2>
        <button onClick={() => { setForm(empty); setEditing(null); setShowForm(true); }}
          className="bg-green-600 text-white px-4 py-2 rounded-lg hover:bg-green-700 text-sm font-medium">
          + Nuevo cliente
        </button>
      </div>

      {showForm && (
        <form onSubmit={handleSubmit} className="bg-white border rounded-xl p-6 mb-6 shadow-sm">
          <h3 className="font-semibold text-gray-700 mb-4">{editing ? 'Editar' : 'Nuevo'} cliente</h3>
          {error && <p className="text-red-500 text-sm mb-3">{error}</p>}
          <div className="grid grid-cols-2 gap-4">
            {[['firstName','Nombre'], ['lastName','Apellido'], ['email','Email'], ['phone','Teléfono']].map(([k,l]) => (
              <div key={k}>
                <label className="text-xs text-gray-500 font-medium">{l}</label>
                <input required className="w-full border rounded-lg px-3 py-2 text-sm mt-1"
                  value={(form as any)[k]} onChange={e => setForm(f => ({...f, [k]: e.target.value}))} />
              </div>
            ))}
          </div>
          <div className="flex gap-2 mt-4">
            <button type="submit" className="bg-green-600 text-white px-4 py-2 rounded-lg text-sm font-medium hover:bg-green-700">Guardar</button>
            <button type="button" onClick={() => setShowForm(false)} className="border px-4 py-2 rounded-lg text-sm hover:bg-gray-50">Cancelar</button>
          </div>
        </form>
      )}

      <div className="bg-white border rounded-xl shadow-sm overflow-hidden">
        <table className="w-full text-sm">
          <thead className="bg-gray-50 text-gray-600 text-xs uppercase">
            <tr>
              {['Nombre','Email','Teléfono','Acciones'].map(h => (
                <th key={h} className="px-4 py-3 text-left">{h}</th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-100">
            {customers.map(c => (
              <tr key={c.id} className="hover:bg-gray-50">
                <td className="px-4 py-3 font-medium text-gray-800">{c.fullName}</td>
                <td className="px-4 py-3 text-gray-600">{c.email}</td>
                <td className="px-4 py-3 text-gray-600">{c.phone}</td>
                <td className="px-4 py-3 flex gap-3">
                  <button onClick={() => handleEdit(c)} className="text-zinc-700 hover:underline text-xs">Editar</button>
                  <button onClick={() => handleDelete(c.id)} className="text-red-500 hover:underline text-xs">Eliminar</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
