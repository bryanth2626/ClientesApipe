-- Para una instalación nueva. Ejecutar en SQL Server Management Studio.
-- No elimina ni altera tablas existentes. No incluye datos personales.
USE [master];
GO
IF DB_ID(N'DemoClientes') IS NULL
    EXEC(N'CREATE DATABASE [DemoClientes]');
GO
USE [DemoClientes];
GO

IF OBJECT_ID(N'dbo.clientes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.clientes
    (
        id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_clientes PRIMARY KEY,
        apellidos NVARCHAR(70) NOT NULL,
        nombres NVARCHAR(70) NOT NULL,
        telefono NVARCHAR(9) NOT NULL,
        correo NVARCHAR(70) NOT NULL,
        dni NVARCHAR(8) NOT NULL CONSTRAINT UQ_clientes_dni UNIQUE,
        direccion NVARCHAR(150) NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.productos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.productos
    (
        id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_productos PRIMARY KEY,
        nombre NVARCHAR(100) NOT NULL,
        descripcion NVARCHAR(100) NOT NULL,
        precio DECIMAL(7,2) NOT NULL,
        garantia TINYINT NOT NULL,
        numserie NVARCHAR(40) NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.Usuario', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuario
    (
        id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Usuario PRIMARY KEY,
        username NVARCHAR(50) NOT NULL CONSTRAINT UQ_Usuario_username UNIQUE,
        passwordhash NVARCHAR(200) NOT NULL,
        rol NVARCHAR(30) NOT NULL CONSTRAINT DF_Usuario_rol DEFAULT N'Usuario'
    );
END;
GO

IF OBJECT_ID(N'dbo.proveedores', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.proveedores
    (
        id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_proveedores PRIMARY KEY,
        nombre VARCHAR(100) NOT NULL,
        RUC VARCHAR(11) NOT NULL,
        telefono VARCHAR(15) NOT NULL,
        direccion VARCHAR(50) NULL
    );
END;
GO
