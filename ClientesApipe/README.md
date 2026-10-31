# ClientesApipe: API integrada

API ASP.NET Core .NET 10 para Clientes, Productos, Proveedores y autenticación de Usuarios.

## Qué se integró

Se usó ClientesApipe-master.zip como estructura y nombre del proyecto y se adaptó el avance de ClientesApi.rar a los espacios de nombres ClientesApipe.

- Se corrigió ClientesApipe.csproj: ahora usa Microsoft.NET.Sdk.Web. Se retiraron las dependencias y el archivo de prueba vacío de NUnit del proyecto web.
- Se incorporó el filtro buscar de Clientes y el avance de Productos del RAR.
- Se añadió Models/Proveedor.cs, Controllers/ProveedoresController.cs y DbSet<Proveedor> en AppDbContext.
- Se conservó la autenticación de Usuarios de ClientesApipe.
- Se agregó el perfil HTTP de desarrollo en el puerto 5243 para AppDemoClientes.
- Se inicializaron las propiedades string de los modelos para compilar sin advertencias de nulabilidad.

## Ejecutar después, en una PC con SQL Server

1. Instalar el SDK .NET 10 y usar un Visual Studio compatible con .NET 10 y Windows Forms si se quiere abrir la interfaz.
2. En una instalación nueva, ejecutar SQL/DemoClientes_Completo.sql en SQL Server Management Studio. Crea DemoClientes y las cuatro tablas si no existen. Si ya existe la BD con las tablas originales, revisar/ejecutar SQL/Proveedores.sql. Ningún script borra o modifica tablas existentes.
3. Ajustar ConnectionStrings:DefaultConnection en ClientesApipe/appsettings.json a la instancia SQL real. El archivo entregado usa autenticación de Windows, localhost y DemoClientes. Para SQL Express, usar Server=.\SQLEXPRESS en la cadena (en JSON escribir .\\SQLEXPRESS). Para autenticación SQL, configurar usuario/contraseña localmente; la contraseña del archivo original no se incluyó en esta entrega.
4. Abrir ClientesApipe.slnx y ejecutar el perfil http. En consola, desde esta carpeta:

```powershell
dotnet restore
dotnet build
dotnet run --project ClientesApipe/ClientesApipe.csproj --launch-profile http
```

La API queda en http://localhost:5243. Scalar está en http://localhost:5243/scalar/v1 durante Development.

## Proveedores: contrato de datos

| SQL | C# / JSON | Tipo |
| --- | --- | --- |
| id | Id / id | INT, PK, IDENTITY(1,1) |
| nombre | Nombre / nombre | VARCHAR(100), obligatorio |
| RUC | Ruc / ruc | VARCHAR(11), 11 dígitos |
| telefono | Telefono / telefono | VARCHAR(15), obligatorio |
| direccion | Direccion / direccion | VARCHAR(50), opcional |

Se asumió que la tabla se llama dbo.proveedores, que SQL Server genera el ID y que direccion permite NULL, como espera la entidad del escritorio. Si la tabla real difiere, ajustar el mapeo antes de ejecutar. Ser Primary Key por sí solo no hace que id se genere automáticamente. Proveedores.sql muestra la propiedad IDENTITY para comprobarla.

El RUC se valida por formato y se rechazan duplicados al registrar o actualizar. No se consulta SUNAT ni se comprueba el dígito verificador. La verificación de duplicados está en la API, sin cambiar índices de la tabla existente.

| Método | Ruta | Resultado |
| --- | --- | --- |
| GET | /api/proveedores?buscar=texto | 200, lista filtrada por nombre o RUC |
| GET | /api/proveedores/{id} | 200 o 404 |
| POST | /api/proveedores | 201 con entidad y Location; 400 inválido, 409 RUC repetido |
| PUT | /api/proveedores/{id} | 204; 400 IDs distintos/datos inválidos, 404 inexistente, 409 RUC repetido |
| DELETE | /api/proveedores/{id} | 204 o 404 |

ClientesApipe/Proveedores.http contiene solicitudes para probar en Visual Studio.

## Flujo con el escritorio y autenticación

Iniciar primero la API y mantenerla abierta. Luego ejecutar el proyecto UI de AppDemoClientes. FrmMain abre Clientes o Proveedores.

Para reproducir el flujo sin login que ya tenía el RAR, Clientes, Productos y Proveedores se mantienen accesibles sin token. Esto difiere del ZIP antiguo ClientesApipe, que protegía Clientes con JWT y la eliminación con rol Admin. Los endpoints /api/auth/registrar y /api/auth/login y la validación JWT se conservan. Si se vuelven a proteger los mantenimientos, el escritorio también necesitará login y envío de Bearer token. La clave JWT incluida es solo de ejemplo para desarrollo local.

## Verificación realizada

- La API y el escritorio compilaron con .NET 10, sin errores ni advertencias.
- Pasaron 23 comprobaciones contra el controlador real por HTTP con EF Core InMemory y las capas BOL/DAL reales del escritorio: CRUD, búsqueda, RUC duplicado, límites de campos, IDs y registros inexistentes.
- No se ejecutó SQL Server ni se operaron visualmente los formularios. La base en memoria se usó solamente en las pruebas, no está activada en la API entregada. Para usar la entrega se necesita SQL Server.
