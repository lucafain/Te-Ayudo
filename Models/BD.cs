using Microsoft.Data.SqlClient; // Slide 83: La librería SqlClient
using Dapper;                    // Slide 86: Dapper MicroORM (Query, QueryFirstOrDefault, Execute)
using System.Collections.Generic;
using System.Linq;

namespace TeAyudo.Models;

public static class BD
{
    private static string _connectionString = @"Server=(localdb)\MSSQLLocalDB;DataBase=Te_Ayudo;Trusted_Connection=True;TrustServerCertificate=True;";

    public static void InicializarBD()
    {
        try
        {
            string masterConnectionString = @"Server=(localdb)\MSSQLLocalDB;DataBase=master;Trusted_Connection=True;TrustServerCertificate=True;";
            using (SqlConnection masterDb = new SqlConnection(masterConnectionString))
            {
                string sqlCrearBase = "IF DB_ID('Te_Ayudo') IS NULL CREATE DATABASE Te_Ayudo;";
                masterDb.Execute(sqlCrearBase);
            }
        }
        catch
        {
            // Si master no es accesible o la base ya existe, continuamos
        }

        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlCrearTabla = @"
                    IF OBJECT_ID('dbo.Usuarios', 'U') IS NULL
                    BEGIN
                        CREATE TABLE dbo.Usuarios (
                            IdUsuario INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                            NombreUsuario NVARCHAR(100) NULL,
                            Nombre NVARCHAR(100) NULL,
                            Apellido NVARCHAR(100) NULL,
                            Email NVARCHAR(150) NOT NULL,
                            Telefono NVARCHAR(30) NULL,
                            Provincia NVARCHAR(100) NULL,
                            Direccion NVARCHAR(150) NULL,
                            Calle NVARCHAR(150) NULL,
                            CodigoPostal NVARCHAR(20) NULL,
                            FechaRegistro DATETIME2 NOT NULL DEFAULT (SYSDATETIME()),
                            TipoUsuario NVARCHAR(30) NOT NULL,
                            Contrasena NVARCHAR(200) NOT NULL,
                            FotoPerfilRuta NVARCHAR(260) NULL,
                            AmbitoLaboral NVARCHAR(120) NULL,
                            LinkedIn NVARCHAR(300) NULL,
                            CvRuta NVARCHAR(260) NULL,
                            AceptaTerminos BIT NOT NULL DEFAULT (0)
                        );
                    END";
                connection.Execute(sqlCrearTabla);
            }
        }
        catch
        {
            // Manejo de contingencia si la tabla ya está disponible
        }
    }

    public static Usuarios? ObtenerUsuarioPorEmail(string email)
    {
        InicializarBD();
        string sql = "SELECT * FROM Usuarios WHERE Email = @pEmail";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pEmail = email });
        }
    }

    public static Usuarios? ObtenerUsuarioPorCredenciales(string email, string contrasena)
    {
        InicializarBD();
        string sql = "SELECT * FROM Usuarios WHERE Email = @pEmail AND Contrasena = @pContrasena";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pEmail = email, pContrasena = contrasena });
        }
    }

    public static Usuarios? ObtenerUsuarioPorId(int id)
    {
        InicializarBD();
        string sql = "SELECT * FROM Usuarios WHERE IdUsuario = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pId = id });
        }
    }

    public static bool GuardarUsuario(Usuarios usuario)
    {
        if (usuario == null || string.IsNullOrEmpty(usuario.Email))
        {
            return false;
        }

        if (ObtenerUsuarioPorEmail(usuario.Email) != null)
        {
            return false;
        }

        string sql = @"
            INSERT INTO Usuarios 
            (NombreUsuario, Nombre, Apellido, Email, Telefono, Provincia, Direccion, Calle, CodigoPostal, FechaRegistro, TipoUsuario, Contrasena, FotoPerfilRuta, AmbitoLaboral, LinkedIn, CvRuta, AceptaTerminos)
            VALUES 
            (@NombreUsuario, @Nombre, @Apellido, @Email, @Telefono, @Provincia, @Direccion, @Calle, @CodigoPostal, @FechaRegistro, @TipoUsuario, @Contrasena, @FotoPerfilRuta, @AmbitoLaboral, @LinkedIn, @CvRuta, @AceptaTerminos)";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new
            {
                NombreUsuario = usuario.NombreUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Provincia = usuario.Provincia,
                Direccion = usuario.Direccion,
                Calle = usuario.Calle,
                CodigoPostal = usuario.CodigoPostal,
                FechaRegistro = usuario.FechaRegistro,
                TipoUsuario = usuario.TipoUsuario,
                Contrasena = usuario.Contrasena,
                FotoPerfilRuta = usuario.FotoPerfilRuta,
                AmbitoLaboral = usuario.AmbitoLaboral,
                LinkedIn = usuario.LinkedIn,
                CvRuta = usuario.CvRuta,
                AceptaTerminos = usuario.AceptaTerminos
            });

            return registrosAfectados > 0;
        }
    }

    public static Usuarios? ObtenerHistorialJoven(int idJoven)
    {
        return ObtenerUsuarioPorId(idJoven);
    }

    public static List<Usuarios> ObtenerUltimosAyudados(int idJoven)
    {
        InicializarBD();
        string sql = "SELECT * FROM Usuarios WHERE TipoUsuario = 'Adulto mayor' OR TipoUsuario = 'Adulto'";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Usuarios>(sql).ToList();
        }
    }
}
