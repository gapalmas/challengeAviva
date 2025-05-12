import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import styles from './OrdersPage.module.css';

export default function OrdersPage() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  const { data: orders = [] } = useQuery({
    queryKey: ['orders'],
    queryFn: () => axios.get('/api/orders').then(res => res.data)
  });

  const mutation = useMutation({
    mutationFn: ({ id, type }) => axios.post(`/api/orders/${id}/${type}`),
    onSuccess: () => queryClient.invalidateQueries(['orders'])
  });

  const action = (id, type) => {
    mutation.mutate({ id, type });
  };

  const goToDetail = (id) => {
    navigate(`/orders/${id}`);
  };

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

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>Órdenes registradas</h2>
      <div className={styles.tableWrapper}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>ID</th>
              <th>Total</th>
              <th>Fee</th>
              <th>Estado</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {orders.map((o) => (
              <tr key={o.id}>
                <td>#{o.id.slice(0, 6)}</td>
                <td>${o.total}</td>
                <td>${o.fee}</td>
                <td>{getStatusText(o.status)}</td>
                <td>
                  <div className={styles.actions}>
                    <button onClick={() => action(o.id, 'pay')}>Pagar</button>
                    <button onClick={() => action(o.id, 'cancel')}>Cancelar</button>
                    <button onClick={() => goToDetail(o.id)}>Ver detalle</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
