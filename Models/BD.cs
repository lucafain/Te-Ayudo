using Microsoft.Data.SqlClient; // Slide 83: La librería SqlClient
using Dapper;                    // Slide 86: Dapper MicroORM (Query, QueryFirstOrDefault, Execute)
using System.Collections.Generic;
using System.Linq;

namespace TeAyudo.Models;

public static class BD
{
    private static string _connectionString = @"Server=localhost;DataBase=Te_Ayudo;Integrated Security=True;TrustServerCertificate=True;";

    public static Usuarios? ObtenerUsuarioPorEmail(string email)
    {
        string sql = "SELECT * FROM Usuarios WHERE email = @pEmail";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pEmail = email });
        }
    }

    public static Usuarios? ObtenerUsuarioPorCredenciales(string email, string contrasena)
    {
        string sql = "SELECT * FROM Usuarios WHERE email = @pEmail AND contrasena = @pContrasena";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pEmail = email, pContrasena = contrasena });
        }
    }

    public static Usuarios? ObtenerUsuarioPorId(int id)
    {
        string sql = "SELECT * FROM Usuarios WHERE IdUsuario = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pId = id });
        }
    }

    public static Usuarios? ObtenerPrimerUsuario()
    {
        string sql = "SELECT TOP 1 * FROM Usuarios";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql);
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
            (nombre, apellido, email, provincia, direccion, calle, codigoPostal, fechaRegistro, tipoUsuario, contrasena, aceptaTerminos, fotoPerfil)
            VALUES 
            (@Nombre, @Apellido, @Email, @Provincia, @Direccion, @Calle, @CodigoPostal, @FechaRegistro, @TipoUsuario, @Contrasena, @AceptaTerminos, @FotoPerfil)";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Provincia = usuario.Provincia,
                Direccion = usuario.Direccion,
                Calle = usuario.Calle,
                CodigoPostal = usuario.CodigoPostal,
                FechaRegistro = usuario.FechaRegistro,
                TipoUsuario = usuario.TipoUsuario,
                Contrasena = usuario.Contrasena,
                AceptaTerminos = usuario.AceptaTerminos,
                FotoPerfil = usuario.FotoPerfil
            });

            return registrosAfectados > 0;
        }
    }

    public static Usuarios? ObtenerHistorialJoven(int idJoven)
    {
        return ObtenerUsuarioPorId(idJoven);
    }

    // tipoUsuario = 0 (false) son los Adultos
    public static List<Usuarios> ObtenerUltimosAyudados(int idJoven)
    {
        string sql = "SELECT * FROM Usuarios WHERE tipoUsuario = 0";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Usuarios>(sql).ToList();
        }
    }

    // tipoUsuario = 1 (true) son los Jóvenes
    public static List<Usuarios> ObtenerJovenesDisponibles()
    {
        string sql = "SELECT * FROM Usuarios WHERE tipoUsuario = 1";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Usuarios>(sql).ToList();
        }
    }

    public static bool GuardarTarea(Tareas tarea)
    {
        if (tarea == null || string.IsNullOrEmpty(tarea.Titulo) || string.IsNullOrEmpty(tarea.Descripcion))
        {
            return false;
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            // Leemos las columnas que REALMENTE tiene la tabla Tareas en SQL Server,
            // así nunca se manda una columna que no existe (ej: diaTarea / horaTarea).
            List<string> columnasTabla = connection.Query<string>(
                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tareas'").ToList();

            Dictionary<string, object?> posiblesValores = new Dictionary<string, object?>();
            posiblesValores.Add("titulo", tarea.Titulo);
            posiblesValores.Add("descripcion", tarea.Descripcion);
            posiblesValores.Add("estado", tarea.Estado);
            posiblesValores.Add("fechaCreacion", tarea.FechaCreacion);
            posiblesValores.Add("lugar", tarea.Lugar);
            posiblesValores.Add("diaTarea", tarea.DiaTarea);
            posiblesValores.Add("horaTarea", tarea.HoraTarea);
            posiblesValores.Add("aclaracion", tarea.Aclaracion);
            posiblesValores.Add("monto", tarea.Monto);
            posiblesValores.Add("tipoLugar", tarea.TipoLugar);
            posiblesValores.Add("IdUsuarioSolicitante", tarea.IdUsuarioSolicitante);

            List<string> columnasInsert = new List<string>();
            List<string> parametrosInsert = new List<string>();
            DynamicParameters parametros = new DynamicParameters();

            foreach (string columnaTabla in columnasTabla)
            {
                foreach (KeyValuePair<string, object?> par in posiblesValores)
                {
                    if (par.Key.ToLower() == columnaTabla.ToLower())
                    {
                        columnasInsert.Add(columnaTabla);
                        parametrosInsert.Add("@" + columnaTabla);
                        parametros.Add(columnaTabla, par.Value);
                    }
                }
            }

            string sql = "INSERT INTO Tareas (" + string.Join(", ", columnasInsert) + ") VALUES (" + string.Join(", ", parametrosInsert) + ")";
            int registrosAfectados = connection.Execute(sql, parametros);
            return registrosAfectados > 0;
        }
    }

    public static List<Tareas> ObtenerTareasPorSolicitante(int idUsuarioSolicitante)
    {
        string sql = "SELECT * FROM Tareas WHERE IdUsuarioSolicitante = @pId ORDER BY IdTarea DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Tareas>(sql, new { pId = idUsuarioSolicitante }).ToList();
        }
    }

    public static List<Tareas> ObtenerTareasDisponibles()
    {
        string sql = "SELECT * FROM Tareas WHERE estado = 1 ORDER BY IdTarea DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Tareas>(sql).ToList();
        }
    }
}
