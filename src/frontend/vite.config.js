import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:7203',
        changeOrigin: true,
        secure: false, // ⬅️ esto permite certificados autofirmados
      },
    },
  },
});

