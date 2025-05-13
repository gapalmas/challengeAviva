# 🧾 Order Management System(OMS)

Sistema de gestión de órdenes que incluye:

- ✅ Backend en .NET 8 con Clean Architecture.
- ✅ Frontend en React + Vite.
- ✅ Comunicación entre capas mediante API REST.
- ✅ Soporte para métodos de pago, detalles de orden y gestión de productos.

---

## 📦 Requisitos

### Backend (.NET 8)

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio Code, Visual Studio 2022+ o cualquier editor con soporte para .NET

### Frontend (React)

- [Node.js 18+](https://nodejs.org/)
- [Yarn](https://yarnpkg.com/)
- Navegador moderno (Chrome, Edge, Firefox)

---

## 🚀 Cómo ejecutar el proyecto

### 🔧 Backend

1. Ir a la carpeta del backend:

   ```bash
   cd backend/
   ```

2. Restaurar paquetes y compilar:

   ```bash
   dotnet restore
   dotnet build
   ```

3. Ejecutar la aplicación:

   ```bash
   dotnet run --project src/App.API
   ```
---

### 💻 Frontend

1. Ir a la carpeta del frontend:

   ```bash
   cd frontend/
   ```

2. Instalar dependencias:

   ```bash
   yarn
   ```

3. Ejecutar el servidor de desarrollo:

   ```bash
   yarn dev
   ```

---

## 📚 Notas

- El frontend consume los endpoints expuestos por la API .NET 8.
---
[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/gapalmas/challengeAviva)