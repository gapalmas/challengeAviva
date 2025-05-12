import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import ProductsPage from './pages/ProductsPage';
import OrdersPage from './pages/OrdersPage';
import OrderDetailPage from './pages/OrderDetailPage';
import './App.css'

const queryClient = new QueryClient();

export default function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <nav style={{ padding: '1rem', background: '#f1f1f1', marginBottom: '1rem' }}>
          <Link to="/" style={{ marginRight: '1rem', textDecoration: 'none' }}>
            Productos
          </Link>
          <Link to="/orders" style={{ textDecoration: 'none' }}>
            Órdenes
          </Link>
        </nav>

        <Routes>
          <Route path="/" element={<ProductsPage />} />
          <Route path="/orders" element={<OrdersPage />} />
          <Route path="/orders/:id" element={<OrderDetailPage />} />
        </Routes>
        
      </BrowserRouter>
    </QueryClientProvider>
  );
}
