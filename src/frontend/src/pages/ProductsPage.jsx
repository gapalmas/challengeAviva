import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { useQuery, useMutation } from '@tanstack/react-query';
import axios from 'axios';
import styles from './ProductsPage.module.css';

export default function ProductsPage() {
  const navigate = useNavigate();
  const [selected, setSelected] = useState([]);
  const [mode, setMode] = useState('Cash');

  const { data: products = [] } = useQuery({
    queryKey: ['products'],
    queryFn: () => axios.get('/api/Products').then(res => res.data)
  });

  const mutation = useMutation({
    mutationFn: (order) =>
      axios.post('/api/Orders', order, {
        headers: {
          'Content-Type': 'application/json',
        },
      }),
    onSuccess: () => navigate('/orders'),
  });

  const toggle = (id) => {
    setSelected((prev) =>
      prev.includes(id) ? prev.filter((p) => p !== id) : [...prev, id]
    );
  };

  const submit = () => {
    const paymentModeValue = { Cash: 0, Card: 1, Transfer: 2 }[mode];

    mutation.mutate({
      productIds: selected,
      paymentMode: paymentModeValue,
    });
  };

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>Selecciona productos</h2>

      <div className={styles.tableWrapper}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Seleccionar</th>
              <th>Producto</th>
              <th>Precio</th>
            </tr>
          </thead>
          <tbody>
            {products.map((p) => (
              <tr key={p.id}>
                <td>
                  <input
                    type="checkbox"
                    checked={selected.includes(p.id)}
                    onChange={() => toggle(p.id)}
                  />
                </td>
                <td>{p.name}</td>
                <td>${p.unitPrice}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className={styles.paymentSection}>
        <label>Método de pago</label>
        <select value={mode} onChange={(e) => setMode(e.target.value)}>
          <option value="Cash">Cash</option>
          <option value="Card">Card</option>
          <option value="Transfer">Transfer</option>
        </select>
      </div>

      <button onClick={submit} disabled={mutation.isPending}>
        {mutation.isPending ? 'Creando...' : 'Crear Orden'}
      </button>
    </div>
  );
}
