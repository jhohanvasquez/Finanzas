# Finanzas

Sistema de gestión de deudas y pagos

## Requisitos

- Node.js >= 18
- .NET >= 9.0
- Angular >= 17

## Instalación

### Backend (API)
1. Clona el repositorio del backend.
2. Instala los paquetes NuGet necesarios:
   - Microsoft.AspNetCore.Cors
   - Microsoft.EntityFrameworkCore
   - Microsoft.EntityFrameworkCore.SqlServer
   - Otros según tu archivo .csproj
3. Configura la cadena de conexión en `appsettings.json`.
4. Ejecuta las migraciones de la base de datos:
   ```bash
   dotnet ef database update
   ```
5. Inicia la API:
   ```bash
   dotnet run
   ```
6. La API estará disponible en `http://localhost:5000/api`.

### Frontend (UI)
1. Clona este repositorio (UI).
2. Instala las dependencias:
   ```bash
   npm install
   ```
3. Inicia la aplicación Angular:
   ```bash
   npm start
   ```
4. Accede a la UI en `http://localhost:4200`

## Detalles técnicos

- **Frontend:** Angular standalone, Material Design, signals, TypeScript.
- **Backend:** .NET 9, API REST, Entity Framework, SQL Server.
- **Autenticación:** Login y registro de usuarios, persistencia en LocalStorage.
- **Gestión de deudas:** Crear, listar, ver detalle, registrar pagos, exportar CSV.
- **Validaciones:** Formularios reactivos, validación de campos, mensajes de error.
- **Estilos:** Monocromático (blanco y negro), diseño moderno y responsivo.
- **Exportación:** Listado de deudas exportable a CSV usando file-saver.

## Funcionalidad

- Registro y login de usuarios.
- Listado general de deudas filtrable por estado.
- Creación y edición de deudas.
- Visualización de detalle de cada deuda.
- Registro de pagos asociados a deudas.
- Exportación de listado de deudas a CSV.
- Validaciones en todos los formularios.

## Ejemplo de uso

### Registro y login
1. Accede a la UI en `http://localhost:4200`.
2. Regístrate con tu nombre, email y contraseña.
3. Inicia sesión para acceder a la gestión de deudas.

### Endpoints principales (API)

- **POST /Usuario/login**
  - Autenticación de usuario.
  - Body: `{ email, passwordHash }`
- **POST /Usuario/registrar**
  - Registro de usuario.
  - Body: `{ nombre, email, passwordHash }`
- **GET /Deuda/consultar/{usuarioId}**
  - Listado de deudas por usuario.
- **POST /Deuda/registrar**
  - Crear nueva deuda.
  - Body: `{ usuarioId, descripcion, montoTotal }`
- **POST /Pago/registrar**
  - Registrar pago a una deuda.
  - Body: `{ deudaId, montoPago, metodoPago }`

### Ejemplo de respuesta de la API

### Listado de deudas (GET /Deuda/consultar/{usuarioId})
```json
[
  {
    "Nombre": "Carlos Pérez",
    "DeudaId": 1,
    "MontoTotal": 500000,
    "TotalPagado": 200000,
    "SaldoPendiente": 300000,
    "Estado": "Parcial"
  },
  {
    "Nombre": "Ana Torres",
    "DeudaId": 2,
    "MontoTotal": 150000,
    "TotalPagado": 150000,
    "SaldoPendiente": 0,
    "Estado": "Pagada"
  }
]
```

### Detalle de deuda (GET /Deuda/consultar/{usuarioId})
```json
{
  "Nombre": "Carlos Pérez",
  "DeudaId": 1,
  "MontoTotal": 500000,
  "TotalPagado": 200000,
  "SaldoPendiente": 300000,
  "Estado": "Parcial"
}
```

### Captura de pantalla

Puedes agregar una imagen de la UI en el README:

```md
![Finanzas UI](./screenshot.png)
```

## Tecnologías implementadas

- **Angular 17**: Framework para la construcción de la interfaz de usuario moderna y responsiva.
- **Angular Material**: Componentes UI con diseño Material y experiencia de usuario optimizada.
- **TypeScript**: Tipado estático y desarrollo seguro en el frontend.
- **RxJS & Signals**: Gestión reactiva de estado y datos en la UI.
- **.NET 9.0**: Backend API RESTful, alto rendimiento y escalabilidad.
- **Entity Framework Core**: ORM para acceso y gestión de datos en SQL Server.
- **SQL Server**: Base de datos relacional para persistencia de usuarios, deudas y pagos.
- **file-saver**: Exportación de datos a CSV desde la UI.
- **LocalStorage**: Persistencia de sesión y estado de usuario en el navegador.
- **CORS**: Configuración para permitir comunicación entre frontend y backend.

Repositorio oficial: [github.com/jhohanvasquez/Finanzas](https://github.com/jhohanvasquez/Finanzas)

## Notas

- Asegúrate de que la API y la UI estén corriendo en los puertos configurados.
- Si tienes problemas de CORS, revisa la configuración en el backend.
- La UI está optimizada para escritorio y dispositivos móviles.

## Autor

Jhohan Vasquez

## Arquitectura y flujo de usuario

### Arquitectura

- **Frontend Angular**
  - Componentes standalone y rutas protegidas por guardas.
  - Servicios para autenticación, gestión de deudas y pagos.
  - Comunicación con la API mediante HttpClient.
  - Estado de usuario persistido en LocalStorage.

- **Backend .NET**
  - Controladores RESTful para usuarios, deudas y pagos.
  - Validaciones y lógica de negocio en los endpoints.
  - Acceso a base de datos mediante Entity Framework.
  - Configuración de CORS para permitir acceso desde la UI.

### Flujo de usuario

1. **Registro/Login**
   - El usuario se registra o inicia sesión en la UI.
   - La sesión se guarda en LocalStorage.
2. **Gestión de deudas**
   - El usuario puede crear, listar y ver detalles de sus deudas.
   - Cada deuda muestra su estado y permite registrar pagos.
3. **Registro de pagos**
   - El usuario ingresa monto y método de pago.
   - El backend actualiza el estado y saldo de la deuda.
4. **Exportación**
   - El usuario puede exportar el listado de deudas a CSV.

## Preguntas frecuentes (FAQ)

### ¿Cómo configuro la base de datos?
- Edita la cadena de conexión en `appsettings.json` del backend.
- Ejecuta las migraciones con `dotnet ef database update`.

### ¿Cómo soluciono problemas de CORS?
- Verifica que el backend tenga habilitado CORS para el origen de la UI.
- Revisa la configuración en `Startup.cs` o `Program.cs`.

### ¿Cómo agrego nuevos usuarios o deudas?
- Usa la UI para registrar usuarios y crear deudas.
- También puedes insertar datos directamente en la base de datos si lo requieres.

### ¿Cómo exporto el listado de deudas?
- Haz clic en el botón "Exportar CSV" en el listado de deudas.

### ¿Cómo reporto un bug o solicito una mejora?
- Abre un issue en el [repositorio oficial](https://github.com/jhohanvasquez/Finanzas).

---

Para dudas o soporte, abre un issue en el repositorio.
