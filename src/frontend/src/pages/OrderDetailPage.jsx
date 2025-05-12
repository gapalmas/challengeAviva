import { useParams, Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import axios from 'axios';
import styles from './OrderDetailPage.module.css';

export default function OrderDetailPage() {
  const { id } = useParams();

  const { data: order, isLoading, isError } = useQuery({
    queryKey: ['order', id],
    queryFn: () => axios.get(`/api/orders/${id}`).then(res => res.data)
  });

  const getStatusText = (status) => {
    switch (status) {
      case 1:
        return 'Pagado';
      case 2:
        return 'Cancelado';
      default:
        return 'Pendiente';
    }
  };

  const getPaymentModeText = (mode) => {
    switch (mode) {
      case 0:
        return 'Efectivo';
      case 1:
        return 'Tarjeta';
      case 2:
        return 'Transferencia';
      default:
        return 'Desconocido';
    }
  };

  if (isLoading) return <p>Cargando detalle...</p>;
  if (isError || !order) return <p>No se pudo cargar la orden.</p>;

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>Detalle de orden #{order.id.slice(0, 6)}</h2>

      <div className={styles.section}>
        <p><strong>Estado:</strong> {getStatusText(order.status)}</p>
        <p><strong>Método de pago:</strong> {getPaymentModeText(order.paymentMode)}</p>
        <p><strong>Total:</strong> ${order.total} + Fee ${order.fee}</p>
      </div>

      <div className={styles.section}>
        <h3>Productos</h3>
        <ul className={styles.products}>
          {order.products.map((p, i) => (
            <li key={i}>
              {p.name} - ${p.unitPrice}
            </li>
          ))}
        </ul>
      </div>

      <Link to="/orders" className={styles.backLink}>← Volver a órdenes</Link>
    </div>
  );
}
