import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { restaurantService, reservationService, customerService, orderService } from '../services/api';

export default function HomePage() {
  const [stats, setStats] = useState({ restaurants: 0, reservations: 0, customers: 0, orders: 0 });

  useEffect(() => {
    Promise.all([
      restaurantService.getAll(),
      reservationService.getAll(),
      customerService.getAll(),
      orderService.getAll(),
    ]).then(([r, res, c, o]) =>
      setStats({ restaurants: r.length, reservations: res.length, customers: c.length, orders: o.length })
    );
  }, []);

  const cards = [
    { label: 'Restaurantes', value: stats.restaurants, to: '/restaurants', icon: '🏪' },
    { label: 'Mesas',        value: '-',                to: '/tables',      icon: '🪑' },
    { label: 'Reservas',     value: stats.reservations, to: '/reservations',icon: '📅' },
    { label: 'Clientes',     value: stats.customers,    to: '/customers',   icon: '👥' },
    { label: 'Órdenes',      value: stats.orders,       to: '/orders',      icon: '🧾' },
  ];

  return (
    <div>
      {/* ── Banner ── */}
      <div
        className="relative h-72 md:h-96 bg-zinc-900 bg-cover bg-center"
        style={{ backgroundImage: "url('/banner.jpg')" }}
      >
        {/* overlay oscuro para que el texto sea legible */}
        <div className="absolute inset-0 bg-black/55" />
        <div className="relative z-10 h-full flex flex-col items-center justify-center text-center px-4">
          <h1 className="text-4xl md:text-5xl font-bold text-white tracking-tight drop-shadow-lg">
            Sistema de Reservas
          </h1>
          <p className="mt-3 text-zinc-300 text-lg md:text-xl">
            Gestión completa de restaurantes, mesas y clientes
          </p>
          <Link
            to="/reservations"
            className="mt-6 bg-white text-zinc-900 font-semibold px-6 py-2.5 rounded-full hover:bg-zinc-100 transition text-sm shadow-lg"
          >
            Nueva reserva →
          </Link>
        </div>
      </div>

      {/* ── Stats ── */}
      <div className="max-w-5xl mx-auto px-4 py-10">
        <h2 className="text-lg font-semibold text-zinc-700 mb-5">Panel de administración</h2>
        <div className="grid grid-cols-2 md:grid-cols-5 gap-4">
          {cards.map(c => (
            <Link
              key={c.to}
              to={c.to}
              className="bg-white border border-zinc-200 rounded-2xl p-5 shadow-sm hover:shadow-md hover:border-zinc-400 transition group text-center"
            >
              <div className="text-3xl mb-2">{c.icon}</div>
              <div className="text-3xl font-bold text-zinc-900 group-hover:text-black">{c.value}</div>
              <div className="mt-1 text-xs text-zinc-500 font-medium uppercase tracking-wide">{c.label}</div>
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
}
