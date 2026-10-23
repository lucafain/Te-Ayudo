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
            // Nombres alternativos que puede tener la columna en la BD
            posiblesValores.Add("diaFecha", tarea.DiaTarea.Date.Add(tarea.HoraTarea));
            posiblesValores.Add("horaFecha", tarea.HoraTarea);
            posiblesValores.Add("hora", tarea.HoraTarea);
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
        string sql = "SELECT * FROM Tareas ORDER BY IdTarea DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Tareas>(sql).ToList();
        }
    }

    // Tareas activas que el joven todavía no vio: ni postuladas ni descartadas por él, y que no son suyas
    public static List<Tareas> ObtenerTareasPendientes(int idJoven)
    {
        string sql = @"
            SELECT T.* 
            FROM Tareas T
            WHERE T.estado = 1
              AND T.IdUsuarioSolicitante <> @pIdJoven
              AND NOT EXISTS (SELECT 1 FROM Postulaciones P WHERE P.IdTarea = T.IdTarea AND P.IdUsuario = @pIdJoven)
            ORDER BY T.IdTarea DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Tareas>(sql, new { pIdJoven = idJoven }).ToList();
        }
    }

    public static bool AceptarTarea(int idTarea)
    {
        string sql = "UPDATE Tareas SET estado = 0 WHERE IdTarea = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int filas = connection.Execute(sql, new { pId = idTarea });
            return filas > 0;
        }
    }

    // El joven descarta la tarea: NO se borra la tarea, se guarda en Postulaciones como "Descartada"
    // para que no le vuelva a aparecer solo a ese joven.
    public static bool RechazarTarea(int idTarea, int idJoven)
    {
        Postulaciones descarte = new Postulaciones(idJoven, idTarea, "Descartada", 0, true);
        return GuardarPostulacion(descarte);
    }

    public static Tareas? ObtenerTareaPorId(int idTarea)
    {
        string sql = "SELECT * FROM Tareas WHERE IdTarea = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Tareas>(sql, new { pId = idTarea });
        }
    }

    public static bool GuardarPostulacion(Postulaciones postulacion)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string sqlExiste = "SELECT * FROM Postulaciones WHERE IdUsuario = @IdUsuario AND IdTarea = @IdTarea";
            Postulaciones? existente = connection.QueryFirstOrDefault<Postulaciones>(sqlExiste, new
            {
                IdUsuario = postulacion.IdUsuario,
                IdTarea = postulacion.IdTarea
            });

            if (existente != null)
            {
                return true;
            }

            string sqlInsert = @"
                INSERT INTO Postulaciones (IdUsuario, IdTarea, estado, calificacion, puntual)
                VALUES (@IdUsuario, @IdTarea, @Estado, @Calificacion, @Puntual)";

            int filas = connection.Execute(sqlInsert, new
            {
                IdUsuario = postulacion.IdUsuario,
                IdTarea = postulacion.IdTarea,
                Estado = postulacion.Estado,
                Calificacion = postulacion.Calificacion,
                Puntual = postulacion.Puntual
            });

            return filas > 0;
        }
    }

    // Solo los jóvenes que siguen postulados (Pendiente o Aceptada)
    public static List<Usuarios> ObtenerPostuladosPorTarea(int idTarea)
    {
        string sql = @"
            SELECT U.* 
            FROM Usuarios U
            INNER JOIN Postulaciones P ON U.IdUsuario = P.IdUsuario
            WHERE P.IdTarea = @pIdTarea
              AND P.estado IN ('Pendiente', 'Aceptada')
            ORDER BY P.IdPostulacion DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Usuarios>(sql, new { pIdTarea = idTarea }).ToList();
        }
    }

    public static int ContarPostulantes(int idTarea)
    {
        string sql = "SELECT COUNT(*) FROM Postulaciones WHERE IdTarea = @pIdTarea AND estado IN ('Pendiente', 'Aceptada')";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.ExecuteScalar<int>(sql, new { pIdTarea = idTarea });
        }
    }

    // Solo se puede cancelar si la tarea no tiene postulantes
    public static bool CancelarTarea(int idTarea)
    {
        if (ContarPostulantes(idTarea) > 0)
        {
            return false;
        }

        // Primero se borran las filas de Postulaciones (descartadas/rechazadas) por la clave foránea
        string sqlPostulaciones = "DELETE FROM Postulaciones WHERE IdTarea = @pId";
        string sqlTarea = "DELETE FROM Tareas WHERE IdTarea = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(sqlPostulaciones, new { pId = idTarea });
            int filas = connection.Execute(sqlTarea, new { pId = idTarea });
            return filas > 0;
        }
    }

    // El adulto acepta a un joven: su postulación pasa a "Aceptada" y la tarea deja de estar activa
    public static bool AceptarPostulante(int idTarea, int idUsuario)
    {
        string sqlPostulacion = "UPDATE Postulaciones SET estado = 'Aceptada' WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdUsuario";
        string sqlOtros = "UPDATE Postulaciones SET estado = 'Rechazada' WHERE IdTarea = @pIdTarea AND IdUsuario <> @pIdUsuario AND estado = 'Pendiente'";
        string sqlTarea = "UPDATE Tareas SET estado = 0 WHERE IdTarea = @pIdTarea";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int filas = connection.Execute(sqlPostulacion, new { pIdTarea = idTarea, pIdUsuario = idUsuario });
            connection.Execute(sqlOtros, new { pIdTarea = idTarea, pIdUsuario = idUsuario });
            connection.Execute(sqlTarea, new { pIdTarea = idTarea });
            return filas > 0;
        }
    }

    public static bool RechazarPostulante(int idTarea, int idUsuario)
    {
        string sql = "UPDATE Postulaciones SET estado = 'Rechazada' WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int filas = connection.Execute(sql, new { pIdTarea = idTarea, pIdUsuario = idUsuario });
            return filas > 0;
        }
    }

    public static int ContarAyudasRealizadas(int idUsuario)
    {
        string sql = "SELECT COUNT(*) FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado = 'Aceptada'";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.ExecuteScalar<int>(sql, new { pIdUsuario = idUsuario });
        }
    }

    public static double ObtenerValoracionPromedio(int idUsuario)
    {
        string sql = "SELECT ISNULL(AVG(calificacion), 0) FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado = 'Aceptada' AND calificacion > 0";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.ExecuteScalar<double>(sql, new { pIdUsuario = idUsuario });
        }
    }

    public static bool EsPuntual(int idUsuario)
    {
        string sql = "SELECT COUNT(*) FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado = 'Aceptada' AND puntual = 0";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int impuntualidades = connection.ExecuteScalar<int>(sql, new { pIdUsuario = idUsuario });
            return impuntualidades == 0;
        }
    }
}
