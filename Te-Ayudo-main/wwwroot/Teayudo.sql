USE [master]
GO
/****** Object:  Database [Te_Ayudo]    Script Date: 21/10/2026 13:37:37 ******/
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
/****** Object:  User [alumno]    Script Date: 21/10/2026 13:37:37 ******/
CREATE USER [alumno] FOR LOGIN [alumno] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Mensajes]    Script Date: 21/10/2026 13:37:37 ******/
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
/****** Object:  Table [dbo].[Postulaciones]    Script Date: 21/10/2026 13:37:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Postulaciones](
	[IdPostulacion] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdTarea] [int] NOT NULL,
	[estado] [varchar](50) NOT NULL,
	[calificacion] [float] NOT NULL,
	[puntual] [bit] NOT NULL,
 CONSTRAINT [PK_Postulaciones] PRIMARY KEY CLUSTERED 
(
	[IdPostulacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tareas]    Script Date: 21/10/2026 13:37:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tareas](
	[IdTarea] [int] IDENTITY(1,1) NOT NULL,
	[titulo] [varchar](50) NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
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
/****** Object:  Table [dbo].[Usuarios]    Script Date: 21/10/2026 13:37:37 ******/
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
USE [master]
GO
ALTER DATABASE [Te_Ayudo] SET  READ_WRITE 
GO
