import { useEffect, useState } from 'react';
import { restaurantService, tableService } from '../services/api';
import type { Restaurant, Table } from '../types';
import { TABLE_STATUS } from '../types';

const STATUS_STYLE: Record<number, { bg: string; border: string; text: string; dot: string }> = {
  0: { bg: 'bg-green-50',  border: 'border-green-300', text: 'text-green-700', dot: 'bg-green-500' },
  1: { bg: 'bg-red-50',    border: 'border-red-300',   text: 'text-red-700',   dot: 'bg-red-500'   },
  2: { bg: 'bg-yellow-50', border: 'border-yellow-300',text: 'text-yellow-700',dot: 'bg-yellow-500'},
  3: { bg: 'bg-gray-100',  border: 'border-gray-300',  text: 'text-gray-500',  dot: 'bg-gray-400'  },
};

const STATUS_LABELS = TABLE_STATUS;

export default function TablesPage() {
  const [restaurants, setRestaurants] = useState<Restaurant[]>([]);
  const [selected, setSelected] = useState<Restaurant | null>(null);
  const [tables, setTables] = useState<Table[]>([]);
  const [filterStatus, setFilterStatus] = useState<number | 'all'>('all');
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    restaurantService.getAll().then(setRestaurants);
  }, []);

  const handleSelect = async (id: number) => {
    const restaurant = restaurants.find(r => r.id === id) ?? null;
    setSelected(restaurant);
    setFilterStatus('all');
    if (!restaurant) { setTables([]); return; }
    setLoading(true);
    const data = await tableService.getByRestaurant(id);
    setTables(data);
    setLoading(false);
  };

  const handleStatusChange = async (tableId: number, newStatus: number) => {
    try {
      await tableService.updateStatus(tableId, newStatus);
      setTables(prev => prev.map(t => t.id === tableId ? { ...t, status: newStatus, statusName: STATUS_LABELS[newStatus as keyof typeof STATUS_LABELS] } : t));
    } catch (err: any) {
      alert(err.response?.data?.message ?? 'Error al actualizar estado');
    }
  };

  const filtered = filterStatus === 'all' ? tables : tables.filter(t => t.status === filterStatus);

  const counts = [0, 1, 2, 3].map(s => ({
    status: s,
    count: tables.filter(t => t.status === s).length,
  }));

  return (
    <div className="max-w-6xl mx-auto px-4 py-8">
      <h2 className="text-2xl font-bold text-gray-800 mb-6">Disponibilidad de Mesas</h2>

      {/* Selector de restaurante */}
      <div className="bg-white border border-gray-200 rounded-xl p-5 shadow-sm mb-6">
        <label className="block text-sm font-medium text-gray-700 mb-2">Selecciona un restaurante</label>
        <select
          className="w-full md:w-80 border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-zinc-400"
          value={selected?.id ?? ''}
          onChange={e => handleSelect(Number(e.target.value))}
        >
          <option value="">-- Elige un restaurante --</option>
          {restaurants.map(r => (
            <option key={r.id} value={r.id}>{r.name} — {r.address}</option>
          ))}
        </select>
      </div>

      {selected && (
        <>
          {/* Info del restaurante */}
          <div className="bg-zinc-50 border border-zinc-200 rounded-xl px-5 py-4 mb-5 flex flex-wrap items-center gap-4">
            <div>
              <p className="font-semibold text-zinc-900 text-lg">{selected.name}</p>
              <p className="text-zinc-700 text-sm">{selected.address}</p>
            </div>
            <div className="ml-auto flex gap-3 flex-wrap">
              {counts.map(c => (
                <div key={c.status} className={`flex items-center gap-1.5 px-3 py-1.5 rounded-full text-xs font-medium border ${STATUS_STYLE[c.status].bg} ${STATUS_STYLE[c.status].border} ${STATUS_STYLE[c.status].text}`}>
                  <span className={`w-2 h-2 rounded-full ${STATUS_STYLE[c.status].dot}`} />
                  {STATUS_LABELS[c.status as keyof typeof STATUS_LABELS]}: {c.count}
                </div>
              ))}
            </div>
          </div>

          {/* Filtro por estado */}
          <div className="flex gap-2 mb-5 flex-wrap">
            {['all', 0, 1, 2, 3].map(s => (
              <button
                key={s}
                onClick={() => setFilterStatus(s as number | 'all')}
                className={`px-3 py-1.5 rounded-full text-xs font-medium border transition-colors ${
                  filterStatus === s
                    ? 'bg-zinc-900 text-white border-zinc-900'
                    : 'bg-white text-gray-600 border-gray-300 hover:border-zinc-500'
                }`}
              >
                {s === 'all' ? 'Todas' : STATUS_LABELS[s as keyof typeof STATUS_LABELS]}
              </button>
            ))}
          </div>

          {/* Grid de mesas */}
          {loading ? (
            <p className="text-gray-400 text-sm">Cargando mesas...</p>
          ) : filtered.length === 0 ? (
            <p className="text-gray-400 text-sm">No hay mesas con ese estado.</p>
          ) : (
            <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4">
              {filtered.map(t => {
                const style = STATUS_STYLE[t.status];
                return (
                  <div
                    key={t.id}
                    className={`${style.bg} ${style.border} border-2 rounded-xl p-4 flex flex-col items-center gap-2 shadow-sm`}
                  >
                    {/* Icono mesa */}
                    <div className={`text-3xl ${t.status === 3 ? 'opacity-30' : ''}`}>🪑</div>

                    <p className="font-bold text-gray-800 text-lg">Mesa #{t.tableNumber}</p>
                    <p className="text-xs text-gray-500">{t.capacity} {t.capacity === 1 ? 'persona' : 'personas'}</p>

                    {/* Badge de estado */}
                    <span className={`flex items-center gap-1 text-xs font-semibold px-2 py-0.5 rounded-full ${style.bg} ${style.text} border ${style.border}`}>
                      <span className={`w-1.5 h-1.5 rounded-full ${style.dot}`} />
                      {STATUS_LABELS[t.status as keyof typeof STATUS_LABELS]}
                    </span>

                    {/* Cambiar estado */}
                    <select
                      className="mt-1 w-full text-xs border border-gray-300 rounded-md px-1 py-1 bg-white text-gray-600 focus:outline-none focus:ring-1 focus:ring-zinc-400"
                      value={t.status}
                      onChange={e => handleStatusChange(t.id, Number(e.target.value))}
                    >
                      {Object.entries(STATUS_LABELS).map(([val, label]) => (
                        <option key={val} value={val}>{label}</option>
                      ))}
                    </select>
                  </div>
                );
              })}
            </div>
          )}
        </>
      )}

      {!selected && (
        <div className="text-center py-16 text-gray-400">
          <p className="text-5xl mb-4">🍽️</p>
          <p className="text-lg font-medium">Selecciona un restaurante para ver sus mesas</p>
        </div>
      )}
    </div>
  );
}
