USE [master]
GO
/****** Object:  Database [Te_Ayudo]    Script Date: 26/10/2026 13:51:11 ******/
CREATE DATABASE [Te_Ayudo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Te_Ayudo', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Te_Ayudo.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Te_Ayudo_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Te_Ayudo_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Te_Ayudo] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Te_Ayudo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Te_Ayudo] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Te_Ayudo] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Te_Ayudo] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Te_Ayudo] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Te_Ayudo] SET ARITHABORT OFF 
GO
ALTER DATABASE [Te_Ayudo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Te_Ayudo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Te_Ayudo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Te_Ayudo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Te_Ayudo] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Te_Ayudo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Te_Ayudo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Te_Ayudo] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Te_Ayudo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Te_Ayudo] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Te_Ayudo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Te_Ayudo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Te_Ayudo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Te_Ayudo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Te_Ayudo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Te_Ayudo] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Te_Ayudo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Te_Ayudo] SET RECOVERY FULL 
GO
ALTER DATABASE [Te_Ayudo] SET  MULTI_USER 
GO
ALTER DATABASE [Te_Ayudo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Te_Ayudo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Te_Ayudo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Te_Ayudo] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Te_Ayudo] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Te_Ayudo', N'ON'
GO
ALTER DATABASE [Te_Ayudo] SET QUERY_STORE = OFF
GO
USE [Te_Ayudo]
GO
/****** Object:  User [alumno]    Script Date: 26/10/2026 13:51:11 ******/
CREATE USER [alumno] FOR LOGIN [alumno] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Mensajes]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mensajes](
	[IdMensaje] [int] IDENTITY(1,1) NOT NULL,
	[contenido] [varchar](max) NOT NULL,
	[fechaEnvio] [datetime] NOT NULL,
	[IdEmisor] [int] NOT NULL,
	[IdReceptor] [int] NOT NULL,
 CONSTRAINT [PK_Mensajes] PRIMARY KEY CLUSTERED 
(
	[IdMensaje] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Postulaciones]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Postulaciones](
	[IdPostulacion] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdTarea] [int] NOT NULL,
	[estado] [varchar](50) NOT NULL,
	[calificacion] [float] NULL,
	[puntual] [bit] NULL,
 CONSTRAINT [PK_Postulaciones] PRIMARY KEY CLUSTERED 
(
	[IdPostulacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tareas]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tareas](
	[IdTarea] [int] IDENTITY(1,1) NOT NULL,
	[titulo] [varchar](50) NOT NULL,
	[descripcion] [varchar](max) NOT NULL,
	[estado] [bit] NOT NULL,
	[fechaCreacion] [date] NOT NULL,
	[lugar] [varchar](50) NOT NULL,
	[diaTarea] [date] NOT NULL,
	[horaTarea] [time](7) NOT NULL,
	[aclaracion] [varchar](max) NULL,
	[monto] [float] NOT NULL,
	[tipoLugar] [varchar](50) NOT NULL,
	[IdUsuarioSolicitante] [int] NOT NULL,
 CONSTRAINT [PK_Tareas] PRIMARY KEY CLUSTERED 
(
	[IdTarea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[IdUsuario] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[apellido] [varchar](50) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[provincia] [varchar](50) NOT NULL,
	[direccion] [int] NOT NULL,
	[calle] [varchar](100) NOT NULL,
	[codigoPostal] [varchar](8) NOT NULL,
	[fechaRegistro] [date] NOT NULL,
	[tipoUsuario] [bit] NOT NULL,
	[contrasena] [varchar](50) NOT NULL,
	[aceptaTerminos] [bit] NULL,
	[fotoPerfil] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Mensajes_Usuarios] FOREIGN KEY([IdEmisor])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Mensajes_Usuarios]
GO
ALTER TABLE [dbo].[Mensajes]  WITH CHECK ADD  CONSTRAINT [FK_Mensajes_Usuarios1] FOREIGN KEY([IdReceptor])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Mensajes] CHECK CONSTRAINT [FK_Mensajes_Usuarios1]
GO
ALTER TABLE [dbo].[Postulaciones]  WITH CHECK ADD  CONSTRAINT [FK_Postulaciones_Tareas] FOREIGN KEY([IdTarea])
REFERENCES [dbo].[Tareas] ([IdTarea])
GO
ALTER TABLE [dbo].[Postulaciones] CHECK CONSTRAINT [FK_Postulaciones_Tareas]
GO
ALTER TABLE [dbo].[Postulaciones]  WITH CHECK ADD  CONSTRAINT [FK_Postulaciones_Usuarios] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Postulaciones] CHECK CONSTRAINT [FK_Postulaciones_Usuarios]
GO
ALTER TABLE [dbo].[Tareas]  WITH CHECK ADD  CONSTRAINT [FK_Tareas_Usuarios] FOREIGN KEY([IdUsuarioSolicitante])
REFERENCES [dbo].[Usuarios] ([IdUsuario])
GO
ALTER TABLE [dbo].[Tareas] CHECK CONSTRAINT [FK_Tareas_Usuarios]
GO
/****** Object:  StoredProcedure [dbo].[BuscarTareasPorLugar]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BuscarTareasPorLugar]
    @lugar VARCHAR(50)
AS
BEGIN
    SELECT * FROM Tareas WHERE lugar LIKE '%' + @lugar + '%' AND estado = 1
END
GO
/****** Object:  StoredProcedure [dbo].[CalificarPostulacion]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CalificarPostulacion]
    @IdPostulacion INT,
    @calificacion FLOAT,
    @puntual BIT
AS
BEGIN
    UPDATE Postulaciones SET calificacion = @calificacion, puntual = @puntual WHERE IdPostulacion = @IdPostulacion
END
GO
/****** Object:  StoredProcedure [dbo].[CambiarEstadoPostulacion]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CambiarEstadoPostulacion]
    @IdPostulacion INT,
    @estado VARCHAR(50)
AS
BEGIN
    UPDATE Postulaciones SET estado = @estado WHERE IdPostulacion = @IdPostulacion
END
GO
/****** Object:  StoredProcedure [dbo].[CambiarEstadoTarea]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CambiarEstadoTarea]
    @IdTarea INT,
    @estado BIT
AS
BEGIN
    UPDATE Tareas SET estado = @estado WHERE IdTarea = @IdTarea
END
GO
/****** Object:  StoredProcedure [dbo].[CrearPostulacion]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CrearPostulacion]
    @IdUsuario INT,
    @IdTarea INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE IdUsuario = @IdUsuario)
    BEGIN
        PRINT 'Error: el usuario no existe.'
    END
    ELSE IF NOT EXISTS (SELECT 1 FROM Tareas WHERE IdTarea = @IdTarea AND estado = 1)
    BEGIN
        PRINT 'Error: la tarea no existe o está cerrada.'
    END
    ELSE IF EXISTS (SELECT 1 FROM Postulaciones WHERE IdUsuario = @IdUsuario AND IdTarea = @IdTarea)
    BEGIN
        PRINT 'Error: ya te postulaste a esta tarea.'
    END
    ELSE
    BEGIN
        INSERT INTO Postulaciones (IdUsuario, IdTarea, estado, calificacion, puntual)
        VALUES (@IdUsuario, @IdTarea, 'Pendiente', NULL, NULL)

        PRINT 'Postulación creada correctamente.'
    END
END
GO
/****** Object:  StoredProcedure [dbo].[EnviarMensaje]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[EnviarMensaje]
    @contenido VARCHAR(MAX),
    @IdEmisor INT,
    @IdReceptor INT
AS
BEGIN
    INSERT INTO Mensajes (contenido, fechaEnvio, IdEmisor, IdReceptor) VALUES (@contenido, GETDATE(), @IdEmisor, @IdReceptor)
END
GO
/****** Object:  StoredProcedure [dbo].[IniciarSesion]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IniciarSesion]
    @email VARCHAR(50),
    @contrasena VARCHAR(50)
AS
BEGIN
    SELECT IdUsuario, nombre, apellido, email, tipoUsuario FROM Usuarios WHERE email = @email AND contrasena = @contrasena
END
GO
/****** Object:  StoredProcedure [dbo].[ModificarUsuario]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ModificarUsuario]
    @IdUsuario INT,
    @nombre VARCHAR(50),
    @apellido VARCHAR(50),
    @email VARCHAR(50),
    @provincia VARCHAR(50),
    @direccion INT,
    @calle VARCHAR(100),
    @codigoPostal VARCHAR(8),
    @contrasena VARCHAR(50),
    @fotoPerfil VARCHAR(MAX)
AS
BEGIN
    UPDATE Usuarios SET nombre = @nombre, apellido = @apellido, email = @email, provincia = @provincia, direccion = @direccion, calle = @calle, codigoPostal = @codigoPostal, contrasena = @contrasena, fotoPerfil = @fotoPerfil WHERE IdUsuario = @IdUsuario
END
GO
/****** Object:  StoredProcedure [dbo].[MostrarConversacion]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[MostrarConversacion]
    @IdUsuario1 INT,
    @IdUsuario2 INT
AS
BEGIN
    SELECT * FROM Mensajes WHERE (IdEmisor = @IdUsuario1 AND IdReceptor = @IdUsuario2) OR (IdEmisor = @IdUsuario2 AND IdReceptor = @IdUsuario1) ORDER BY fechaEnvio
END
GO
/****** Object:  StoredProcedure [dbo].[MostrarMensajesRecibidos]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MostrarMensajesRecibidos]
    @IdReceptor INT
AS
BEGIN
    SELECT M.IdMensaje, M.contenido, M.fechaEnvio, M.IdEmisor, U.nombre, U.apellido FROM Mensajes M INNER JOIN Usuarios U ON M.IdEmisor = U.IdUsuario WHERE M.IdReceptor = @IdReceptor ORDER BY M.fechaEnvio DESC
END
GO
/****** Object:  StoredProcedure [dbo].[MostrarPerfil]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MostrarPerfil]
    @IdUsuario INT
AS
BEGIN
    SELECT IdUsuario, nombre, apellido, email, provincia, direccion, calle, codigoPostal, fechaRegistro, tipoUsuario, fotoPerfil FROM Usuarios WHERE IdUsuario = @IdUsuario
END
GO
/****** Object:  StoredProcedure [dbo].[MostrarPostulacionesCompletas]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[MostrarPostulacionesCompletas]
    @IdTarea INT
AS
BEGIN
    SELECT P.IdPostulacion, P.estado, P.calificacion, P.puntual, U.IdUsuario, U.nombre, U.apellido, T.IdTarea, T.titulo, T.descripcion, T.lugar, T.diaTarea, T.horaTarea, T.monto FROM Postulaciones P INNER JOIN Usuarios U ON P.IdUsuario = U.IdUsuario INNER JOIN Tareas T ON P.IdTarea = T.IdTarea WHERE T.IdTarea = @IdTarea
END
GO
/****** Object:  StoredProcedure [dbo].[MostrarPostulacionesUsuario]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MostrarPostulacionesUsuario]
    @IdUsuario INT
AS
BEGIN
    SELECT * FROM Postulaciones WHERE IdUsuario = @IdUsuario
END
GO
/****** Object:  StoredProcedure [dbo].[MostrarPostulantesTarea]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MostrarPostulantesTarea]
    @IdTarea INT
AS
BEGIN
    SELECT U.IdUsuario, U.nombre, U.apellido, U.email, P.IdPostulacion, P.estado, P.calificacion, P.puntual FROM Postulaciones P INNER JOIN Usuarios U ON P.IdUsuario = U.IdUsuario WHERE P.IdTarea = @IdTarea
END
GO
/****** Object:  StoredProcedure [dbo].[MostrarTareasConUsuarios]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MostrarTareasConUsuarios]
AS
BEGIN
    SELECT T.IdTarea, T.titulo, T.descripcion, T.estado, T.fechaCreacion, T.lugar, T.diaTarea, T.horaTarea, T.aclaracion, T.monto, T.tipoLugar, U.IdUsuario, U.nombre, U.apellido FROM Tareas T INNER JOIN Usuarios U ON T.IdUsuarioSolicitante = U.IdUsuario
END
GO
/****** Object:  StoredProcedure [dbo].[MostrarTareasDisponibles]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MostrarTareasDisponibles]
AS
BEGIN
    SELECT * FROM Tareas WHERE estado = 1
END
GO
/****** Object:  StoredProcedure [dbo].[MostrarTareasUsuario]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MostrarTareasUsuario]
    @IdUsuarioSolicitante INT
AS
BEGIN
    SELECT * FROM Tareas WHERE IdUsuarioSolicitante = @IdUsuarioSolicitante
END
GO
/****** Object:  StoredProcedure [dbo].[PublicarTarea]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PublicarTarea]
    @titulo VARCHAR(50),
    @descripcion VARCHAR(MAX),
    @lugar VARCHAR(50),
    @diaTarea DATE,
    @horaTarea TIME,
    @aclaracion VARCHAR(MAX),
    @monto FLOAT,
    @tipoLugar VARCHAR(50),
    @IdUsuarioSolicitante INT
AS
BEGIN
    INSERT INTO Tareas (titulo, descripcion, estado, fechaCreacion, lugar, diaTarea, horaTarea, aclaracion, monto, tipoLugar, IdUsuarioSolicitante) VALUES (@titulo, @descripcion, 1, GETDATE(), @lugar, @diaTarea, @horaTarea, @aclaracion, @monto, @tipoLugar, @IdUsuarioSolicitante)
END
GO
/****** Object:  StoredProcedure [dbo].[RegistrarUsuario]    Script Date: 26/10/2026 13:51:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RegistrarUsuario]
    @nombre VARCHAR(50),
    @apellido VARCHAR(50),
    @email VARCHAR(50),
    @provincia VARCHAR(50),
    @direccion INT,
    @calle VARCHAR(100),
    @codigoPostal VARCHAR(8),
    @tipoUsuario BIT,
    @contrasena VARCHAR(50),
    @aceptaTerminos BIT,
    @fotoPerfil VARCHAR(MAX)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Usuarios WHERE email = @email)
    BEGIN
        INSERT INTO Usuarios (nombre, apellido, email, provincia, direccion, calle, codigoPostal, fechaRegistro, tipoUsuario, contrasena, aceptaTerminos, fotoPerfil) VALUES (@nombre, @apellido, @email, @provincia, @direccion, @calle, @codigoPostal, GETDATE(), @tipoUsuario, @contrasena, @aceptaTerminos, @fotoPerfil)

        PRINT 'Usuario registrado correctamente.'
    END
    ELSE
    BEGIN
        PRINT 'Error: el correo ya está registrado.'
    END
END
GO
USE [master]
GO
ALTER DATABASE [Te_Ayudo] SET  READ_WRITE 
GO
