import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import HomePage from './pages/HomePage';
import RestaurantsPage from './pages/RestaurantsPage';
import RestaurantDetailPage from './pages/RestaurantDetailPage';
import TablesPage from './pages/TablesPage';
import CustomersPage from './pages/CustomersPage';
import ReservationsPage from './pages/ReservationsPage';
import OrdersPage from './pages/OrdersPage';

export default function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-50">
        <Navbar />
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/restaurants" element={<RestaurantsPage />} />
          <Route path="/restaurants/:id" element={<RestaurantDetailPage />} />
          <Route path="/tables" element={<TablesPage />} />
          <Route path="/customers" element={<CustomersPage />} />
          <Route path="/reservations" element={<ReservationsPage />} />
          <Route path="/orders" element={<OrdersPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}
