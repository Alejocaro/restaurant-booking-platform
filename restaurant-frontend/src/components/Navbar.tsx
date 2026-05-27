import { Link, useLocation } from 'react-router-dom';

const links = [
  { to: '/', label: 'Inicio' },
  { to: '/restaurants', label: 'Restaurantes' },
  { to: '/tables', label: 'Mesas' },
  { to: '/reservations', label: 'Reservas' },
  { to: '/customers', label: 'Clientes' },
  { to: '/orders', label: 'Órdenes' },
];

export default function Navbar() {
  const { pathname } = useLocation();
  return (
    <nav className="bg-zinc-900 text-white shadow-md">
      <div className="max-w-6xl mx-auto px-4 flex items-center gap-6 h-14">
        <span className="font-bold text-lg tracking-wide">🍽 RestaurantApp</span>
        {links.map(l => (
          <Link
            key={l.to}
            to={l.to}
            className={`text-sm font-medium hover:text-zinc-300 transition-colors ${pathname === l.to ? 'underline underline-offset-4 text-white' : 'text-zinc-400'}`}
          >
            {l.label}
          </Link>
        ))}
      </div>
    </nav>
  );
}
