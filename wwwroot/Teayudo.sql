USE [master]
GO
/****** Object:  Database [Te_Ayudo]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  User [alumno]    Script Date: 26/10/2026 14:23:11 ******/
CREATE USER [alumno] FOR LOGIN [alumno] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Mensajes]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  Table [dbo].[Postulaciones]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  Table [dbo].[Tareas]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  Table [dbo].[Usuarios]    Script Date: 26/10/2026 14:23:11 ******/
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
SET IDENTITY_INSERT [dbo].[Mensajes] ON 

INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (1, N'Hola, queria consultar si sigue disponible la tarea.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 1, 3)
INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (2, N'Si, sigue disponible. Gracias por consultar.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 3, 1)
INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (3, N'Hola, me postule para pasear los perros.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 5, 7)
INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (4, N'Perfecto, despues reviso tu postulacion.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 7, 5)
INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (5, N'Hola, queria saber que programas hay que instalar.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 6, 4)
INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (6, N'Necesito instalar algunos programas para estudiar.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 4, 6)
INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (7, N'Hola, puedo ayudarte con el repaso de matematica.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 2, 4)
INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (8, N'Gracias, te confirmo el horario mas tarde.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 4, 2)
INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (9, N'Hola, queria consultar por el horario.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 8, 3)
INSERT [dbo].[Mensajes] ([IdMensaje], [contenido], [fechaEnvio], [IdEmisor], [IdReceptor]) VALUES (10, N'Te confirmo el horario durante el dia.', CAST(N'2026-10-26T14:20:17.457' AS DateTime), 3, 8)
SET IDENTITY_INSERT [dbo].[Mensajes] OFF
GO
SET IDENTITY_INSERT [dbo].[Postulaciones] ON 

INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (1, 1, 1, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (2, 2, 1, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (3, 5, 2, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (4, 6, 3, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (5, 8, 4, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (6, 1, 5, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (7, 2, 6, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (8, 5, 7, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (9, 6, 8, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (10, 8, 1, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (11, 5, 3, N'Pendiente', NULL, NULL)
INSERT [dbo].[Postulaciones] ([IdPostulacion], [IdUsuario], [IdTarea], [estado], [calificacion], [puntual]) VALUES (12, 6, 4, N'Pendiente', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Postulaciones] OFF
GO
SET IDENTITY_INSERT [dbo].[Tareas] ON 

INSERT [dbo].[Tareas] ([IdTarea], [titulo], [descripcion], [estado], [fechaCreacion], [lugar], [diaTarea], [horaTarea], [aclaracion], [monto], [tipoLugar], [IdUsuarioSolicitante]) VALUES (1, N'Limpiar patio', N'Barrer y ordenar el patio', 1, CAST(N'2026-10-26' AS Date), N'Almagro', CAST(N'2026-11-04' AS Date), CAST(N'11:00:00' AS Time), N'Llevar guantes', 4500, N'Domicilio', 3)
INSERT [dbo].[Tareas] ([IdTarea], [titulo], [descripcion], [estado], [fechaCreacion], [lugar], [diaTarea], [horaTarea], [aclaracion], [monto], [tipoLugar], [IdUsuarioSolicitante]) VALUES (2, N'Comprar remedios', N'Retirar un pedido de farmacia', 1, CAST(N'2026-10-26' AS Date), N'Caballito', CAST(N'2026-11-05' AS Date), CAST(N'12:00:00' AS Time), N'El pedido ya esta pago', 2500, N'Exterior', 4)
INSERT [dbo].[Tareas] ([IdTarea], [titulo], [descripcion], [estado], [fechaCreacion], [lugar], [diaTarea], [horaTarea], [aclaracion], [monto], [tipoLugar], [IdUsuarioSolicitante]) VALUES (3, N'Regar plantas', N'Regar plantas del balcon', 1, CAST(N'2026-10-26' AS Date), N'Villa Crespo', CAST(N'2026-11-06' AS Date), CAST(N'09:30:00' AS Time), N'Hay varias macetas', 2000, N'Domicilio', 3)
INSERT [dbo].[Tareas] ([IdTarea], [titulo], [descripcion], [estado], [fechaCreacion], [lugar], [diaTarea], [horaTarea], [aclaracion], [monto], [tipoLugar], [IdUsuarioSolicitante]) VALUES (4, N'Pasear dos perros', N'Pasear dos perros por la plaza', 1, CAST(N'2026-10-26' AS Date), N'Palermo', CAST(N'2026-11-07' AS Date), CAST(N'15:00:00' AS Time), N'Los perros usan correa', 4000, N'Exterior', 7)
INSERT [dbo].[Tareas] ([IdTarea], [titulo], [descripcion], [estado], [fechaCreacion], [lugar], [diaTarea], [horaTarea], [aclaracion], [monto], [tipoLugar], [IdUsuarioSolicitante]) VALUES (5, N'Ayuda con computadora', N'Instalar programas basicos', 1, CAST(N'2026-10-26' AS Date), N'Almagro', CAST(N'2026-11-08' AS Date), CAST(N'14:00:00' AS Time), N'Es una computadora de escritorio', 6000, N'Domicilio', 4)
INSERT [dbo].[Tareas] ([IdTarea], [titulo], [descripcion], [estado], [fechaCreacion], [lugar], [diaTarea], [horaTarea], [aclaracion], [monto], [tipoLugar], [IdUsuarioSolicitante]) VALUES (6, N'Ordenar biblioteca', N'Ordenar libros por tema', 1, CAST(N'2026-10-26' AS Date), N'Caballito', CAST(N'2026-11-09' AS Date), CAST(N'10:00:00' AS Time), N'No hace falta experiencia', 3500, N'Domicilio', 3)
INSERT [dbo].[Tareas] ([IdTarea], [titulo], [descripcion], [estado], [fechaCreacion], [lugar], [diaTarea], [horaTarea], [aclaracion], [monto], [tipoLugar], [IdUsuarioSolicitante]) VALUES (7, N'Llevar paquetes', N'Llevar paquetes al correo', 1, CAST(N'2026-10-26' AS Date), N'Palermo', CAST(N'2026-11-10' AS Date), CAST(N'13:00:00' AS Time), N'Son tres paquetes chicos', 3000, N'Exterior', 7)
INSERT [dbo].[Tareas] ([IdTarea], [titulo], [descripcion], [estado], [fechaCreacion], [lugar], [diaTarea], [horaTarea], [aclaracion], [monto], [tipoLugar], [IdUsuarioSolicitante]) VALUES (8, N'Ayudar con estudio', N'Repasar matematica de secundaria', 1, CAST(N'2026-10-26' AS Date), N'Villa Crespo', CAST(N'2026-11-11' AS Date), CAST(N'17:00:00' AS Time), N'Nivel secundario', 5000, N'Domicilio', 4)
SET IDENTITY_INSERT [dbo].[Tareas] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([IdUsuario], [nombre], [apellido], [email], [provincia], [direccion], [calle], [codigoPostal], [fechaRegistro], [tipoUsuario], [contrasena], [aceptaTerminos], [fotoPerfil]) VALUES (1, N'Mateo', N'Perez Curci', N'mateo@gmail.com', N'Buenos Aires', 150, N'Av. Medrano', N'1179', CAST(N'2026-10-26' AS Date), 0, N'Mateo123', 1, N'')
INSERT [dbo].[Usuarios] ([IdUsuario], [nombre], [apellido], [email], [provincia], [direccion], [calle], [codigoPostal], [fechaRegistro], [tipoUsuario], [contrasena], [aceptaTerminos], [fotoPerfil]) VALUES (2, N'Rocco', N'Diaz', N'rocco@gmail.com', N'Buenos Aires', 220, N'Av. Boedo', N'1218', CAST(N'2026-10-26' AS Date), 0, N'Rocco123', 1, N'')
INSERT [dbo].[Usuarios] ([IdUsuario], [nombre], [apellido], [email], [provincia], [direccion], [calle], [codigoPostal], [fechaRegistro], [tipoUsuario], [contrasena], [aceptaTerminos], [fotoPerfil]) VALUES (3, N'Santiago', N'Montiel', N'santiago@gmail.com', N'Buenos Aires', 330, N'Av. Pueyrredon', N'1032', CAST(N'2026-10-26' AS Date), 1, N'Santiago123', 1, N'')
INSERT [dbo].[Usuarios] ([IdUsuario], [nombre], [apellido], [email], [provincia], [direccion], [calle], [codigoPostal], [fechaRegistro], [tipoUsuario], [contrasena], [aceptaTerminos], [fotoPerfil]) VALUES (4, N'Camilo', N'Rey', N'camilo@gmail.com', N'Buenos Aires', 440, N'Av. Warnes', N'1414', CAST(N'2026-10-26' AS Date), 1, N'Camilo123', 1, N'')
INSERT [dbo].[Usuarios] ([IdUsuario], [nombre], [apellido], [email], [provincia], [direccion], [calle], [codigoPostal], [fechaRegistro], [tipoUsuario], [contrasena], [aceptaTerminos], [fotoPerfil]) VALUES (5, N'Nicolas', N'Reniero', N'nicolas@gmail.com', N'Buenos Aires', 550, N'Av. San Juan', N'1147', CAST(N'2026-10-26' AS Date), 0, N'Nicolas123', 1, N'')
INSERT [dbo].[Usuarios] ([IdUsuario], [nombre], [apellido], [email], [provincia], [direccion], [calle], [codigoPostal], [fechaRegistro], [tipoUsuario], [contrasena], [aceptaTerminos], [fotoPerfil]) VALUES (6, N'Lucia', N'Sosa', N'lucia@gmail.com', N'Buenos Aires', 660, N'Av. Directorio', N'1406', CAST(N'2026-10-26' AS Date), 0, N'Lucia123', 1, N'')
INSERT [dbo].[Usuarios] ([IdUsuario], [nombre], [apellido], [email], [provincia], [direccion], [calle], [codigoPostal], [fechaRegistro], [tipoUsuario], [contrasena], [aceptaTerminos], [fotoPerfil]) VALUES (7, N'Nahuel', N'Molina', N'nahuel@gmail.com', N'Buenos Aires', 770, N'Av. Belgrano', N'1092', CAST(N'2026-10-26' AS Date), 1, N'Nahuel123', 1, N'')
INSERT [dbo].[Usuarios] ([IdUsuario], [nombre], [apellido], [email], [provincia], [direccion], [calle], [codigoPostal], [fechaRegistro], [tipoUsuario], [contrasena], [aceptaTerminos], [fotoPerfil]) VALUES (8, N'Agustina', N'Romero', N'agustina@gmail.com', N'Buenos Aires', 880, N'Av. Diaz Velez', N'1405', CAST(N'2026-10-26' AS Date), 0, N'Agustina123', 1, N'')
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
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
/****** Object:  StoredProcedure [dbo].[BuscarTareasPorLugar]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[CalificarPostulacion]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[CambiarEstadoPostulacion]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[CambiarEstadoTarea]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[CrearPostulacion]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[EnviarMensaje]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[IniciarSesion]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[ModificarUsuario]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[MostrarConversacion]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[MostrarMensajesRecibidos]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[MostrarPerfil]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[MostrarPostulacionesCompletas]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[MostrarPostulacionesUsuario]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[MostrarPostulantesTarea]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[MostrarTareasConUsuarios]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[MostrarTareasDisponibles]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[MostrarTareasUsuario]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[PublicarTarea]    Script Date: 26/10/2026 14:23:11 ******/
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
/****** Object:  StoredProcedure [dbo].[RegistrarUsuario]    Script Date: 26/10/2026 14:23:11 ******/
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
