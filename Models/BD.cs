using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace TeAyudo.Models;

public class BD 
{
    public static string ConnectionString { get; set; } = @"Server=localhost;DataBase=NombreBase;Integrated Security=True;TrustServerCertificate=True;";
    private static bool _databaseReady;
    private static readonly object DatabaseLock = new();

    public static void EnsureDatabaseReady()
    {
        if (_databaseReady)
        {
            return;
        }

        lock (DatabaseLock)
        {
            if (_databaseReady)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new InvalidOperationException("La cadena de conexión no está configurada.");
            }

            SqlConnectionStringBuilder builder = new(ConnectionString);
            string databaseName = builder.InitialCatalog;
            if (string.IsNullOrWhiteSpace(databaseName))
            {
                throw new InvalidOperationException("La cadena de conexión no tiene nombre de base de datos.");
            }

            SqlConnectionStringBuilder masterBuilder = new(ConnectionString)
            {
                InitialCatalog = "master"
            };

            string createDatabaseSql = $"IF DB_ID('{databaseName.Replace("'", "''")}') IS NULL CREATE DATABASE [{databaseName}];";

            using (SqlConnection masterConnection = new(masterBuilder.ConnectionString))
            using (SqlCommand createDbCommand = new(createDatabaseSql, masterConnection))
            {
                masterConnection.Open();
                createDbCommand.ExecuteNonQuery();
            }

            const string createSchemaSql = """
                DECLARE @NeedsReset BIT = 0;

                IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL
                BEGIN
                    IF COL_LENGTH('dbo.Usuarios', 'Contrasena') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'AceptaTerminos') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'NombreUsuario') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'Nombre') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'Apellido') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'Email') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'Telefono') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'Provincia') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'Direccion') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'Calle') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'CodigoPostal') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'FechaRegistro') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'TipoUsuario') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'FotoPerfilRuta') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'AmbitoLaboral') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'LinkedIn') IS NULL SET @NeedsReset = 1;
                    IF COL_LENGTH('dbo.Usuarios', 'CvRuta') IS NULL SET @NeedsReset = 1;

                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name = 'Email' AND (system_type_id <> 231 OR max_length < 300)) SET @NeedsReset = 1;
                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name = 'Contrasena' AND (system_type_id <> 231 OR max_length < 400)) SET @NeedsReset = 1;
                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name = 'FotoPerfilRuta' AND (system_type_id <> 231 OR max_length < 520)) SET @NeedsReset = 1;
                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name = 'CvRuta' AND (system_type_id <> 231 OR max_length < 520)) SET @NeedsReset = 1;
                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name = 'FechaRegistro' AND system_type_id <> 42) SET @NeedsReset = 1;
                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name = 'Direccion' AND system_type_id <> 231) SET @NeedsReset = 1;
                    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.Usuarios') AND name = 'AceptaTerminos' AND system_type_id <> 104) SET @NeedsReset = 1;
                END;

                IF @NeedsReset = 1 AND OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL
                BEGIN
                    DROP TABLE dbo.Usuarios;
                END;

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
                        FechaRegistro DATETIME2 NOT NULL CONSTRAINT DF_Usuarios_FechaRegistro DEFAULT (SYSDATETIME()),
                        TipoUsuario NVARCHAR(30) NOT NULL,
                        Contrasena NVARCHAR(200) NOT NULL,
                        FotoPerfilRuta NVARCHAR(260) NULL,
                        AmbitoLaboral NVARCHAR(120) NULL,
                        LinkedIn NVARCHAR(300) NULL,
                        CvRuta NVARCHAR(260) NULL,
                        AceptaTerminos BIT NOT NULL CONSTRAINT DF_Usuarios_AceptaTerminos DEFAULT (0)
                    );
                END;

                IF NOT EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = 'IX_Usuarios_Email' AND object_id = OBJECT_ID('dbo.Usuarios')
                )
                BEGIN
                    CREATE UNIQUE INDEX IX_Usuarios_Email ON dbo.Usuarios(Email);
                END;
                """;

            using (SqlConnection appConnection = new(ConnectionString))
            using (SqlCommand createSchemaCommand = new(createSchemaSql, appConnection))
            {
                appConnection.Open();
                createSchemaCommand.ExecuteNonQuery();
            }

            _databaseReady = true;
        }
    }

    public bool GuardarUsuario(Usuarios usuario)
    {
        if (usuario == null || string.IsNullOrWhiteSpace(usuario.Email))
        {
            return false;
        }

        usuario.Email = usuario.Email.Trim();
        usuario.NombreUsuario = LimpiarTexto(usuario.NombreUsuario);
        usuario.Nombre = LimpiarTexto(usuario.Nombre);
        usuario.Apellido = LimpiarTexto(usuario.Apellido);
        usuario.Telefono = LimpiarTexto(usuario.Telefono);
        usuario.Provincia = LimpiarTexto(usuario.Provincia);
        usuario.Direccion = LimpiarTexto(usuario.Direccion);
        usuario.Calle = LimpiarTexto(usuario.Calle);
        usuario.CodigoPostal = LimpiarTexto(usuario.CodigoPostal);
        usuario.TipoUsuario = LimpiarTexto(usuario.TipoUsuario);
        usuario.Contrasena = LimpiarTexto(usuario.Contrasena);
        usuario.FotoPerfilRuta = LimpiarTexto(usuario.FotoPerfilRuta);
        usuario.AmbitoLaboral = LimpiarTexto(usuario.AmbitoLaboral);
        usuario.LinkedIn = LimpiarTexto(usuario.LinkedIn);
        usuario.CvRuta = LimpiarTexto(usuario.CvRuta);

        if (ObtenerUsuarioPorEmail(usuario.Email) != null)
        {
            return false;
        }

        const string sql = """
            INSERT INTO Usuarios
            (NombreUsuario, Nombre, Apellido, Email, Telefono, Provincia, Direccion, Calle, CodigoPostal, FechaRegistro, TipoUsuario, Contrasena, FotoPerfilRuta, AmbitoLaboral, LinkedIn, CvRuta, AceptaTerminos)
            OUTPUT INSERTED.IdUsuario
            VALUES
            (@NombreUsuario, @Nombre, @Apellido, @Email, @Telefono, @Provincia, @Direccion, @Calle, @CodigoPostal, @FechaRegistro, @TipoUsuario, @Contrasena, @FotoPerfilRuta, @AmbitoLaboral, @LinkedIn, @CvRuta, @AceptaTerminos);
            """;

        using SqlConnection connection = CrearConexion();
        using SqlCommand command = new(sql, connection);
        AgregarParametros(command, usuario);

        connection.Open();
        object? resultado = command.ExecuteScalar();
        if (resultado == null || resultado == DBNull.Value)
        {
            throw new InvalidOperationException("La inserción no devolvió un IdUsuario.");
        }

        usuario.IdUsuario = Convert.ToInt32(resultado);
        return true;
    }

    public Usuarios? ObtenerUsuarioPorEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        email = email.Trim();

        const string sql = """
            SELECT TOP 1 IdUsuario, NombreUsuario, Nombre, Apellido, Email, Telefono, Provincia, Direccion, Calle, CodigoPostal, FechaRegistro, TipoUsuario, Contrasena, FotoPerfilRuta, AmbitoLaboral, LinkedIn, CvRuta, AceptaTerminos
            FROM Usuarios
            WHERE Email = @Email;
            """;

        using SqlConnection connection = CrearConexion();
        using SqlCommand command = new(sql, connection);
        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 150) { Value = email });
        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();
        return reader.Read() ? MapUsuario(reader) : null;
    }

    public Usuarios? ObtenerUsuarioPorCredenciales(string email, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contrasena))
        {
            return null;
        }

        email = email.Trim();
        contrasena = contrasena.Trim();

        const string sql = """
            SELECT TOP 1 IdUsuario, NombreUsuario, Nombre, Apellido, Email, Telefono, Provincia, Direccion, Calle, CodigoPostal, FechaRegistro, TipoUsuario, Contrasena, FotoPerfilRuta, AmbitoLaboral, LinkedIn, CvRuta, AceptaTerminos
            FROM Usuarios
            WHERE Email = @Email AND Contrasena = @Contrasena;
            """;

        using SqlConnection connection = CrearConexion();
        using SqlCommand command = new(sql, connection);
        command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 150) { Value = email });
        command.Parameters.Add(new SqlParameter("@Contrasena", SqlDbType.NVarChar, 200) { Value = contrasena });
        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();
        return reader.Read() ? MapUsuario(reader) : null;
    }

    public Usuarios? ObtenerUsuarioPorId(int id)
    {
        const string sql = """
            SELECT TOP 1 IdUsuario, NombreUsuario, Nombre, Apellido, Email, Telefono, Provincia, Direccion, Calle, CodigoPostal, FechaRegistro, TipoUsuario, Contrasena, FotoPerfilRuta, AmbitoLaboral, LinkedIn, CvRuta, AceptaTerminos
            FROM Usuarios
            WHERE IdUsuario = @IdUsuario;
            """;

        using SqlConnection connection = CrearConexion();
        using SqlCommand command = new(sql, connection);
        command.Parameters.Add(new SqlParameter("@IdUsuario", SqlDbType.Int) { Value = id });
        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();
        return reader.Read() ? MapUsuario(reader) : null;
    }

    // Ejemplo usando Dapper: Obtener lista de Patentes (Query)
    public List<Patente> LevantarPatentes()
    {
        const string query = "SELECT * FROM Patentes";
        using SqlConnection connection = CrearConexion();
        connection.Open();
        var patentes = connection.Query<Patente>(query).ToList();
        return patentes;
    }

    // Ejemplo usando Dapper: Obtener una patente por matrícula (QueryFirstOrDefault)
    public Patente? LevantarPatente(string patente)
    {
        if (string.IsNullOrWhiteSpace(patente)) return null;
        const string query = "SELECT Matricula, FechaCompra FROM Patentes WHERE Matricula = @pPatente";
        using SqlConnection connection = CrearConexion();
        connection.Open();
        var miPatente = connection.QueryFirstOrDefault<Patente>(query, new { pPatente = patente });
        return miPatente;
    }

    private static SqlConnection CrearConexion()
    {
        EnsureDatabaseReady();

        return new SqlConnection(ConnectionString);
    }

    private static void AgregarParametros(SqlCommand command, Usuarios usuario)
    {
        AddTextParameter(command, "@NombreUsuario", 100, usuario.NombreUsuario);
        AddTextParameter(command, "@Nombre", 100, usuario.Nombre);
        AddTextParameter(command, "@Apellido", 100, usuario.Apellido);
        AddTextParameter(command, "@Email", 150, usuario.Email);
        AddTextParameter(command, "@Telefono", 30, usuario.Telefono);
        AddTextParameter(command, "@Provincia", 100, usuario.Provincia);
        AddTextParameter(command, "@Direccion", 150, usuario.Direccion);
        AddTextParameter(command, "@Calle", 150, usuario.Calle);
        AddTextParameter(command, "@CodigoPostal", 20, usuario.CodigoPostal);
        command.Parameters.Add(new SqlParameter("@FechaRegistro", SqlDbType.DateTime2) { Value = usuario.FechaRegistro == default ? DateTime.Now : usuario.FechaRegistro });
        AddTextParameter(command, "@TipoUsuario", 30, usuario.TipoUsuario);
        AddTextParameter(command, "@Contrasena", 200, usuario.Contrasena);
        AddTextParameter(command, "@FotoPerfilRuta", 260, usuario.FotoPerfilRuta);
        AddTextParameter(command, "@AmbitoLaboral", 120, usuario.AmbitoLaboral);
        AddTextParameter(command, "@LinkedIn", 300, usuario.LinkedIn);
        AddTextParameter(command, "@CvRuta", 260, usuario.CvRuta);
        command.Parameters.Add(new SqlParameter("@AceptaTerminos", SqlDbType.Bit) { Value = usuario.AceptaTerminos });
    }

    private static void AddTextParameter(SqlCommand command, string name, int size, string? value)
    {
        command.Parameters.Add(new SqlParameter(name, SqlDbType.NVarChar, size)
        {
            Value = string.IsNullOrWhiteSpace(value) ? DBNull.Value : value.Trim()
        });
    }

    private static string? LimpiarTexto(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    private static Usuarios MapUsuario(SqlDataReader reader)
    {
        return new Usuarios
        {
            IdUsuario = reader.GetInt32(reader.GetOrdinal("IdUsuario")),
            NombreUsuario = GetString(reader, "NombreUsuario"),
            Nombre = GetString(reader, "Nombre"),
            Apellido = GetString(reader, "Apellido"),
            Email = GetString(reader, "Email"),
            Telefono = GetString(reader, "Telefono"),
            Provincia = GetString(reader, "Provincia"),
            Direccion = GetString(reader, "Direccion"),
            Calle = GetString(reader, "Calle"),
            CodigoPostal = GetString(reader, "CodigoPostal"),
            FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
            TipoUsuario = GetString(reader, "TipoUsuario"),
            Contrasena = GetString(reader, "Contrasena"),
            FotoPerfilRuta = GetString(reader, "FotoPerfilRuta"),
            AmbitoLaboral = GetString(reader, "AmbitoLaboral"),
            LinkedIn = GetString(reader, "LinkedIn"),
            CvRuta = GetString(reader, "CvRuta"),
            AceptaTerminos = !reader.IsDBNull(reader.GetOrdinal("AceptaTerminos")) && reader.GetBoolean(reader.GetOrdinal("AceptaTerminos"))
        };
    }

    private static string? GetString(SqlDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }
}