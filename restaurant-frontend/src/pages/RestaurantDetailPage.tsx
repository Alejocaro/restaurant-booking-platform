import { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { restaurantService, tableService, menuItemService } from '../services/api';
import type { Restaurant, Table, MenuItem } from '../types';
import { TABLE_STATUS, MENU_CATEGORY } from '../types';
import StatusBadge from '../components/StatusBadge';

const STATUS_COLOR: Record<number, string> = { 0: 'green', 1: 'red', 2: 'yellow', 3: 'gray' };

export default function RestaurantDetailPage() {
  const { id } = useParams<{ id: string }>();
  const [restaurant, setRestaurant] = useState<Restaurant | null>(null);
  const [tables, setTables] = useState<Table[]>([]);
  const [menu, setMenu] = useState<MenuItem[]>([]);

  useEffect(() => {
    const rid = Number(id);
    restaurantService.getById(rid).then(setRestaurant);
    tableService.getByRestaurant(rid).then(setTables);
    menuItemService.getByRestaurant(rid).then(setMenu);
  }, [id]);

  if (!restaurant) return <div className="p-8 text-gray-500">Cargando...</div>;

  return (
    <div className="max-w-6xl mx-auto px-4 py-8">
      <Link to="/restaurants" className="text-sm text-zinc-700 hover:underline mb-4 inline-block">← Volver</Link>
      <div className="bg-white border rounded-xl p-6 shadow-sm mb-6">
        <h2 className="text-2xl font-bold text-gray-800 mb-1">{restaurant.name}</h2>
        <p className="text-gray-500 text-sm mb-2">{restaurant.address}</p>
        <div className="flex gap-4 text-sm text-gray-600">
          <span>{restaurant.email}</span><span>{restaurant.phone}</span>
          <span>Capacidad: {restaurant.capacity}</span>
        </div>
        {restaurant.description && <p className="text-gray-500 text-sm mt-2">{restaurant.description}</p>}
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div>
          <h3 className="font-semibold text-gray-700 mb-3">Mesas ({tables.length})</h3>
          <div className="space-y-2">
            {tables.map(t => (
              <div key={t.id} className="bg-white border rounded-lg p-3 flex justify-between items-center">
                <span className="text-sm font-medium text-gray-700">Mesa #{t.tableNumber} — {t.capacity} personas</span>
                <StatusBadge label={TABLE_STATUS[t.status as keyof typeof TABLE_STATUS]} color={STATUS_COLOR[t.status]} />
              </div>
            ))}
          </div>
        </div>
        <div>
          <h3 className="font-semibold text-gray-700 mb-3">Menú ({menu.length})</h3>
          <div className="space-y-2">
            {menu.map(m => (
              <div key={m.id} className="bg-white border rounded-lg p-3 flex justify-between items-center">
                <div>
                  <span className="text-sm font-medium text-gray-700">{m.name}</span>
                  <span className="ml-2 text-xs text-gray-400">{MENU_CATEGORY[m.category as keyof typeof MENU_CATEGORY]}</span>
                </div>
                <div className="flex items-center gap-2">
                  <span className="text-sm font-semibold text-green-700">${m.price.toLocaleString()}</span>
                  {!m.isAvailable && <StatusBadge label="No disponible" color="gray" />}
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
}
