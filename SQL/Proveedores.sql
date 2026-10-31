-- Ejecutar en DemoClientes. No elimina ni modifica tablas existentes.
USE [DemoClientes];
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

-- Revisar que las columnas y la generación del ID coincidan con el modelo.
SELECT c.name AS campo, TYPE_NAME(c.user_type_id) AS tipo,
       c.max_length AS longitud, c.is_nullable AS permite_null,
       c.is_identity AS es_identity
FROM sys.columns AS c
WHERE c.object_id = OBJECT_ID(N'dbo.proveedores')
ORDER BY c.column_id;
GO

IF COLUMNPROPERTY(OBJECT_ID(N'dbo.proveedores'), N'id', 'IsIdentity') = 0
    PRINT 'ATENCION: id no es IDENTITY. Esta API espera que SQL Server genere el ID. No se ha modificado la tabla.';
GO
