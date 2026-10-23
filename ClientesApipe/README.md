23/10/2026

Para validar que la API y la seguridad con JWT funcionaran correctamente, seguimos este flujo paso a paso en Postman:

1. Registro de usuario: Hicimos un POST al endpoint /api/auth/registrar enviando un JSON con el username y la contraseña.
El sistema respondió con un 200 OK confirmando la creación del usuario.

2. Inicio de sesión y obtención del Token: Con esas mismas credenciales, realizamos un POST al endpoint /api/auth/login.
La API validó los datos y nos devolvió el token JWT firmado digitalmente.

3. Consulta de Clientes (GET): Probamos el endpoint protegido GET /api/clientes configurando la pestaña de autorización en Postman como Bearer Token y pegando el token generado.
El servidor respondió con un 200 OK mostrando la lista completa de clientes.

4. Actualización de rol (Administrador): Como el endpoint para eliminar registros (DELETE) exigía permisos especiales ([Authorize(Roles = "Admin")]),
fuimos a la base de datos y actualizamos el rol del usuario a Admin ejecutando la sentencia SQL:

UPDATE Usuario SET rol = 'Admin' WHERE username = 'jair';
GO

5. Generación del Token de Admin y Eliminación (DELETE): Volvimos a hacer login en Postman para obtener un token actualizado que ya incluía los privilegios de administrador.
Finalmente, armamos la petición DELETE apuntando al cliente que queríamos borrar (/api/clientes/1), le pegamos el nuevo token en la autorización y la API respondió con un 204 No Content,
confirmando la eliminación del registro.

---------------------------------

BD:

CREATE DATABASE Democlientes
GO

USE Democlientes
GO

CREATE TABLE clientes
(
	id				INT IDENTITY(1,1)	PRIMARY KEY,
	apellidos		NVARCHAR(70)		NOT NULL,
	nombres			NVARCHAR(78)		NOT NULL,
	telefono		NVARCHAR(9)			NOT NULL,
	correo			NVARCHAR(150)		NOT NULL,
	dni				CHAR(8)				NOT NULL,
	direccion		NVARCHAR(150)		NOT NULL,
	CONSTRAINT		uk_dni_cli			UNIQUE (dni),
	CONSTRAINT		ck_dni_cli			CHECK (dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
)
GO

INSERT INTO clientes
(apellidos, nombres, telefono, correo, dni, direccion) VALUES
('Torres Peña', 'Maria', '987654321', 'maria@gmail.com', '11221122', 'Chincha Alta'),
('Carbajal Sotelo', 'Rocio', '963852741', 'rocio@gmail.com', '74185263', 'Pueblo Nuevo')
GO

SELECT * FROM clientes
GO

CREATE TABLE productos
(
	id				INT IDENTITY(1,1) PRIMARY KEY,
	nombre			NVARCHAR(70)		NOT NULL,
	descripcion		NVARCHAR(70)		NOT NULL,
	precio			DECIMAL(7,2)		NOT NULL,
	garantia		TINYINT				NOT NULL,
	numserie		NVARCHAR(40)		NULL

)
GO

INSERT INTO productos
	(nombre, descripcion, precio, garantia, numserie) VALUES
	('Teclado', 'Logitech negro USB', 75, 12, 'ABC-12345'),
	('Mouse', 'Genius USB', 40, 6, 'NULL')
GO

SELECT * FROM productos
GO