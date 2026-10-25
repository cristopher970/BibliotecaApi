-- ============================================================
-- Script de base de datos: Biblioteca
-- Crea la base, las tablas y los datos iniciales (roles, categorias
-- y un usuario administrador).
-- Es idempotente: se puede ejecutar varias veces sin duplicar datos.
-- Ejecutar en SQL Server Management Studio (SSMS).
-- ============================================================

IF DB_ID('Biblioteca') IS NULL
    CREATE DATABASE Biblioteca;
GO

USE Biblioteca;
GO

-- ---------------- Tablas ----------------

IF OBJECT_ID('dbo.Categoria', 'U') IS NULL
CREATE TABLE dbo.Categoria (
    Id     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
);
GO

IF OBJECT_ID('dbo.Rol', 'U') IS NULL
CREATE TABLE dbo.Rol (
    Id     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
);
GO

IF OBJECT_ID('dbo.Socio', 'U') IS NULL
CREATE TABLE dbo.Socio (
    Id       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Rut      VARCHAR(12)  NOT NULL,
    Nombre   VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20)  NULL
);
GO

IF OBJECT_ID('dbo.Libro', 'U') IS NULL
CREATE TABLE dbo.Libro (
    Id          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Titulo      VARCHAR(150) NOT NULL,
    Autor       VARCHAR(100) NOT NULL,
    Isbn        VARCHAR(20)  NULL,
    Stock       INT NOT NULL DEFAULT 1,
    CategoriaId INT NOT NULL REFERENCES dbo.Categoria(Id)
);
GO

IF OBJECT_ID('dbo.Usuario', 'U') IS NULL
CREATE TABLE dbo.Usuario (
    Id             INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NombreCompleto VARCHAR(100) NOT NULL,
    Email          VARCHAR(100) NOT NULL,
    PasswordHash   VARCHAR(255) NOT NULL,
    RolId          INT NOT NULL REFERENCES dbo.Rol(Id)
);
GO

IF OBJECT_ID('dbo.Prestamo', 'U') IS NULL
CREATE TABLE dbo.Prestamo (
    Id              INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FechaPrestamo   DATETIME NOT NULL DEFAULT GETDATE(),
    FechaDevolucion DATETIME NULL,
    Devuelto        BIT NOT NULL,
    LibroId         INT NOT NULL REFERENCES dbo.Libro(Id),
    SocioId         INT NOT NULL REFERENCES dbo.Socio(Id),
    UsuarioId       INT NOT NULL REFERENCES dbo.Usuario(Id)
);
GO

-- ---------------- Datos iniciales ----------------

-- Roles. Importante: la API autoriza por el Id del rol (1 = Administrador, 2 = Bibliotecario),
-- por eso deben crearse en este orden en una base nueva.
IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE Nombre = 'Administrador')
    INSERT INTO dbo.Rol (Nombre) VALUES ('Administrador');
IF NOT EXISTS (SELECT 1 FROM dbo.Rol WHERE Nombre = 'Bibliotecario')
    INSERT INTO dbo.Rol (Nombre) VALUES ('Bibliotecario');
GO

-- Categorias de ejemplo
IF NOT EXISTS (SELECT 1 FROM dbo.Categoria WHERE Nombre = 'Novela')
    INSERT INTO dbo.Categoria (Nombre) VALUES ('Novela');
IF NOT EXISTS (SELECT 1 FROM dbo.Categoria WHERE Nombre = 'Ciencia')
    INSERT INTO dbo.Categoria (Nombre) VALUES ('Ciencia');
IF NOT EXISTS (SELECT 1 FROM dbo.Categoria WHERE Nombre = 'Historia')
    INSERT INTO dbo.Categoria (Nombre) VALUES ('Historia');
GO

-- Usuario administrador inicial (necesario para poder iniciar sesion y crear mas usuarios).
-- Email: admin@biblioteca.cl   Contrasena inicial: Admin12345!
-- El valor guardado es el hash BCrypt de esa contrasena, no la contrasena en texto plano.
IF NOT EXISTS (SELECT 1 FROM dbo.Usuario WHERE Email = 'admin@biblioteca.cl')
    INSERT INTO dbo.Usuario (NombreCompleto, Email, PasswordHash, RolId)
    VALUES ('Administrador', 'admin@biblioteca.cl',
            '$2a$11$8yC/P.pIU79wJjbrJqFHd.t3U8Y4lHqQM7VOKZuoxETc0/rZsON/.',
            (SELECT Id FROM dbo.Rol WHERE Nombre = 'Administrador'));
GO
