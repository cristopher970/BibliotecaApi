# BibliotecaApi

API RESTful para gestionar una biblioteca: libros, categorías, socios y préstamos.
Trabajo 2 de Construcción de Software.

**Tecnologías:** ASP.NET Core Web API (.NET 10) con controladores, Entity Framework Core, SQL Server,
autenticación JWT con roles, versionamiento por URL (`/api/v1/...`) y documentación con Swagger.

## Requisitos

- .NET 10 SDK
- SQL Server (Developer o Express) y SQL Server Management Studio

## 1. Crear la base de datos

Ejecutar en SSMS el script `Docs/script-base-datos.sql`. Crea la base `Biblioteca`, las tablas,
los roles, tres categorías de ejemplo y un usuario administrador. El script se puede ejecutar
más de una vez sin duplicar datos.

> La API autoriza por el Id del rol: **1 = Administrador** y **2 = Bibliotecario**.

## 2. Configurar las variables de entorno

Las credenciales no se guardan en el repositorio. Antes de ejecutar hay que definir:

| Variable | Contenido |
|---|---|
| `ConnectionStrings__MiConexion` | Cadena de conexión a SQL Server |
| `Jwt__Key` | Clave secreta para firmar los tokens (mínimo 32 caracteres) |

Ejemplo en PowerShell (cerrar y volver a abrir la terminal o Visual Studio después de definirlas):

```powershell
setx ConnectionStrings__MiConexion "Server=TU_SERVIDOR;Database=Biblioteca;Trusted_Connection=True;TrustServerCertificate=True;"
setx Jwt__Key "una-clave-secreta-de-al-menos-32-caracteres"
```

Si falta `Jwt__Key`, la API no arranca.

## 3. Ejecutar

```powershell
dotnet run --launch-profile https
```

Swagger queda disponible en `https://localhost:7138/swagger`. Usar siempre la dirección `https`.

## 4. Iniciar sesión

1. En `POST /api/v1/User/login` ingresar `email` = `admin@biblioteca.cl` y `pass` = `Admin12345!`.
2. Copiar solo el valor del `token` (sin comillas).
3. Pulsar **Authorize** en Swagger, pegar el token y confirmar.

El token dura 60 minutos.

## Roles y permisos

| Acción | Sin token | Bibliotecario (2) | Administrador (1) |
|---|---|---|---|
| Consultar (GET) | 401 | 200 | 200 |
| Crear (POST) y editar (PUT) | 401 | permitido | permitido |
| Eliminar (DELETE) | 401 | 403 | 204 |
| Gestionar usuarios (`/User`) | 401 | 403 | 200 |

## Estructura del proyecto

- `Controllers/`: controladores `Libros`, `Categorias`, `Socios`, `Prestamos` y `User`.
- `Models/`: entidades y `BibliotecaContext` (Entity Framework Core).
- `Repositories/`: patrón Repository (`ILibroRepository`, `LibroRepository`), registrado como `Scoped`.
- `Middleware/`: `RequestLoggingMiddleware`, que registra método, ruta, código de estado y duración.
- `Migrations/`: migración `Inicial`, registrada como línea base sobre las tablas existentes.
- `Docs/`: `swagger.json` exportado y `script-base-datos.sql`.