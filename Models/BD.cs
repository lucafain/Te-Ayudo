using Microsoft.Data.SqlClient;

namespace TeAyudo.Models;

public class BD 
{
    public static string ConnectionString { get; set; } = string.Empty;
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

        const string sql = """
            SELECT TOP 1 IdUsuario, NombreUsuario, Nombre, Apellido, Email, Telefono, Provincia, Direccion, Calle, CodigoPostal, FechaRegistro, TipoUsuario, Contrasena, FotoPerfilRuta, AmbitoLaboral, LinkedIn, CvRuta, AceptaTerminos
            FROM Usuarios
            WHERE Email = @Email;
            """;

        using SqlConnection connection = CrearConexion();
        using SqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Email", email);
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

        const string sql = """
            SELECT TOP 1 IdUsuario, NombreUsuario, Nombre, Apellido, Email, Telefono, Provincia, Direccion, Calle, CodigoPostal, FechaRegistro, TipoUsuario, Contrasena, FotoPerfilRuta, AmbitoLaboral, LinkedIn, CvRuta, AceptaTerminos
            FROM Usuarios
            WHERE Email = @Email AND Contrasena = @Contrasena;
            """;

        using SqlConnection connection = CrearConexion();
        using SqlCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@Contrasena", contrasena);
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
        command.Parameters.AddWithValue("@IdUsuario", id);
        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();
        return reader.Read() ? MapUsuario(reader) : null;
    }

    private static SqlConnection CrearConexion()
    {
        EnsureDatabaseReady();

        return new SqlConnection(ConnectionString);
    }

    private static void AgregarParametros(SqlCommand command, Usuarios usuario)
    {
        command.Parameters.AddWithValue("@NombreUsuario", (object?)usuario.NombreUsuario ?? DBNull.Value);
        command.Parameters.AddWithValue("@Nombre", (object?)usuario.Nombre ?? DBNull.Value);
        command.Parameters.AddWithValue("@Apellido", (object?)usuario.Apellido ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)usuario.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Telefono", (object?)usuario.Telefono ?? DBNull.Value);
        command.Parameters.AddWithValue("@Provincia", (object?)usuario.Provincia ?? DBNull.Value);
        command.Parameters.AddWithValue("@Direccion", (object?)usuario.Direccion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Calle", (object?)usuario.Calle ?? DBNull.Value);
        command.Parameters.AddWithValue("@CodigoPostal", (object?)usuario.CodigoPostal ?? DBNull.Value);
        command.Parameters.AddWithValue("@FechaRegistro", usuario.FechaRegistro == default ? DateTime.Now : usuario.FechaRegistro);
        command.Parameters.AddWithValue("@TipoUsuario", (object?)usuario.TipoUsuario ?? DBNull.Value);
        command.Parameters.AddWithValue("@Contrasena", (object?)usuario.Contrasena ?? DBNull.Value);
        command.Parameters.AddWithValue("@FotoPerfilRuta", (object?)usuario.FotoPerfilRuta ?? DBNull.Value);
        command.Parameters.AddWithValue("@AmbitoLaboral", (object?)usuario.AmbitoLaboral ?? DBNull.Value);
        command.Parameters.AddWithValue("@LinkedIn", (object?)usuario.LinkedIn ?? DBNull.Value);
        command.Parameters.AddWithValue("@CvRuta", (object?)usuario.CvRuta ?? DBNull.Value);
        command.Parameters.AddWithValue("@AceptaTerminos", usuario.AceptaTerminos);
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