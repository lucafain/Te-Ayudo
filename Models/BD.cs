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

        string direccionAUsar = string.IsNullOrWhiteSpace(tarea.Lugar) ? "" : tarea.Lugar.Trim();
        if (string.IsNullOrWhiteSpace(direccionAUsar))
        {
            direccionAUsar = "Sin dirección especificada";
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
            posiblesValores.Add("lugar", direccionAUsar);
            posiblesValores.Add("direccion", direccionAUsar);
            posiblesValores.Add("diaTarea", tarea.DiaTarea);
            posiblesValores.Add("horaTarea", tarea.HoraTarea);
            // Nombres alternativos que puede tener la columna en la BD
            posiblesValores.Add("diaFecha", tarea.DiaTarea.Date.Add(tarea.HoraTarea));
            posiblesValores.Add("horaFecha", tarea.HoraTarea);
            posiblesValores.Add("hora", tarea.HoraTarea);
            posiblesValores.Add("aclaracion", tarea.Aclaracion);
            posiblesValores.Add("monto", tarea.Monto);
            posiblesValores.Add("tipoLugar", tarea.TipoLugar);
            posiblesValores.Add("barrio", tarea.Barrio);
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

    public static bool ModificarTarea(Tareas tarea)
    {
        if (tarea == null || tarea.IdTarea <= 0 || string.IsNullOrEmpty(tarea.Titulo) || string.IsNullOrEmpty(tarea.Descripcion))
        {
            return false;
        }

        // Solo se puede modificar si no tiene postulantes ni aceptados
        Tareas? actual = ObtenerTareaPorId(tarea.IdTarea);
        if (actual == null || !actual.Estado || ContarPostulantes(tarea.IdTarea) > 0)
        {
            return false;
        }

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<string> columnasTabla = connection.Query<string>(
                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tareas'").ToList();

            Dictionary<string, object?> posiblesValores = new Dictionary<string, object?>();
            posiblesValores.Add("titulo", tarea.Titulo);
            posiblesValores.Add("descripcion", tarea.Descripcion);
            posiblesValores.Add("lugar", tarea.Lugar);
            posiblesValores.Add("diaTarea", tarea.DiaTarea);
            posiblesValores.Add("horaTarea", tarea.HoraTarea);
            posiblesValores.Add("diaFecha", tarea.DiaTarea.Date.Add(tarea.HoraTarea));
            posiblesValores.Add("horaFecha", tarea.HoraTarea);
            posiblesValores.Add("hora", tarea.HoraTarea);
            posiblesValores.Add("aclaracion", tarea.Aclaracion);
            posiblesValores.Add("monto", tarea.Monto);
            posiblesValores.Add("tipoLugar", tarea.TipoLugar);
            posiblesValores.Add("barrio", tarea.Barrio);

            List<string> asignacionesUpdate = new List<string>();
            DynamicParameters parametros = new DynamicParameters();
            parametros.Add("IdTarea", tarea.IdTarea);

            foreach (string columnaTabla in columnasTabla)
            {
                foreach (KeyValuePair<string, object?> par in posiblesValores)
                {
                    if (par.Key.ToLower() == columnaTabla.ToLower())
                    {
                        asignacionesUpdate.Add(columnaTabla + " = @" + columnaTabla);
                        parametros.Add(columnaTabla, par.Value);
                    }
                }
            }

            if (asignacionesUpdate.Count == 0)
            {
                return false;
            }

            string sql = "UPDATE Tareas SET " + string.Join(", ", asignacionesUpdate) + " WHERE IdTarea = @IdTarea";
            int filas = connection.Execute(sql, parametros);
            return filas > 0;
        }
    }

    public static List<Tareas> ObtenerTareasPorSolicitante(int idUsuarioSolicitante)
    {
        string sql = "SELECT * FROM Tareas WHERE IdUsuarioSolicitante = @pId ORDER BY IdTarea DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<Tareas> misTareas = connection.Query<Tareas>(sql, new { pId = idUsuarioSolicitante }).ToList();
            if (misTareas.Count == 0)
            {
                misTareas = connection.Query<Tareas>("SELECT * FROM Tareas ORDER BY IdTarea DESC").ToList();
            }
            return misTareas;
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

    // Tareas activas que el joven todavía no vio: ni postuladas ni descartadas por él
    public static List<Tareas> ObtenerTareasPendientes(int idJoven)
    {
        string sql = @"
            SELECT T.* 
            FROM Tareas T
            WHERE T.estado = 1
              AND NOT EXISTS (SELECT 1 FROM Postulaciones P WHERE P.IdTarea = T.IdTarea AND P.IdUsuario = @pIdJoven)
            ORDER BY T.IdTarea DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Tareas>(sql, new { pIdJoven = idJoven }).ToList();
        }
    }

    public static List<Tareas> ObtenerTareasRechazadas(int idJoven)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            string sql = esBit
                ? @"SELECT T.*
                    FROM Tareas T
                    INNER JOIN Postulaciones P ON T.IdTarea = P.IdTarea
                    WHERE T.estado = 1
                      AND P.IdUsuario = @pIdJoven
                      AND P.estado = 0"
                : @"SELECT T.*
                    FROM Tareas T
                    INNER JOIN Postulaciones P ON T.IdTarea = P.IdTarea
                    WHERE T.estado = 1
                      AND P.IdUsuario = @pIdJoven
                      AND P.estado IN ('Descartada', 'Rechazada')";

            return connection.Query<Tareas>(sql, new { pIdJoven = idJoven }).ToList();
        }
    }

    public static bool EliminarPostulacionRechazada(int idTarea, int idJoven)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            string sql = esBit
                ? "DELETE FROM Postulaciones WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdJoven AND estado = 0"
                : "DELETE FROM Postulaciones WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdJoven AND estado IN ('Descartada', 'Rechazada')";

            int filas = connection.Execute(sql, new { pIdTarea = idTarea, pIdJoven = idJoven });
            return filas > 0;
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

    private static bool EsEstadoPostulacionesBit(SqlConnection connection)
    {
        string? tipo = connection.QueryFirstOrDefault<string>(
            "SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Postulaciones' AND COLUMN_NAME = 'estado'");
        return tipo != null && tipo.ToLower() == "bit";
    }

    public static bool GuardarPostulacion(Postulaciones postulacion)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            object valorEstado = esBit
                ? (object)(postulacion.Estado == "Pendiente" || postulacion.Estado == "Aceptada")
                : postulacion.Estado;

            int cantidadExistente = connection.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM Postulaciones WHERE IdUsuario = @IdUsuario AND IdTarea = @IdTarea",
                new { IdUsuario = postulacion.IdUsuario, IdTarea = postulacion.IdTarea });

            if (cantidadExistente > 0)
            {
                connection.Execute(
                    "UPDATE Postulaciones SET estado = @Estado WHERE IdUsuario = @IdUsuario AND IdTarea = @IdTarea",
                    new { Estado = valorEstado, IdUsuario = postulacion.IdUsuario, IdTarea = postulacion.IdTarea });
                return true;
            }

            List<string> columnasTabla = connection.Query<string>(
                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Postulaciones' AND COLUMN_NAME <> 'IdPostulacion'").ToList();

            Dictionary<string, object?> posiblesValores = new Dictionary<string, object?>();
            posiblesValores.Add("IdUsuario", postulacion.IdUsuario);
            posiblesValores.Add("IdTarea", postulacion.IdTarea);
            posiblesValores.Add("estado", valorEstado);
            posiblesValores.Add("calificacion", postulacion.Calificacion);
            posiblesValores.Add("puntual", postulacion.Puntual);

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

            string sqlInsert = "INSERT INTO Postulaciones (" + string.Join(", ", columnasInsert) + ") VALUES (" + string.Join(", ", parametrosInsert) + ")";
            int filas = connection.Execute(sqlInsert, parametros);
            return filas > 0;
        }
    }

    // Solo los jóvenes que siguen postulados (Pendiente o Aceptada)
    public static List<Usuarios> ObtenerPostuladosPorTarea(int idTarea)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            string sql = esBit
                ? @"SELECT U.* 
                    FROM Usuarios U
                    INNER JOIN Postulaciones P ON U.IdUsuario = P.IdUsuario
                    WHERE P.IdTarea = @pIdTarea AND P.estado = 1"
                : @"SELECT U.* 
                    FROM Usuarios U
                    INNER JOIN Postulaciones P ON U.IdUsuario = P.IdUsuario
                    WHERE P.IdTarea = @pIdTarea AND P.estado IN ('Pendiente', 'Aceptada')";

            return connection.Query<Usuarios>(sql, new { pIdTarea = idTarea }).ToList();
        }
    }

    public static Usuarios? ObtenerJovenAceptadoPorTarea(int idTarea)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            Tareas? tarea = ObtenerTareaPorId(idTarea);
            if (tarea == null || tarea.Estado)
            {
                return null;
            }

            bool esBit = EsEstadoPostulacionesBit(connection);
            string sql = esBit
                ? @"SELECT TOP 1 U.* 
                    FROM Usuarios U
                    INNER JOIN Postulaciones P ON U.IdUsuario = P.IdUsuario
                    WHERE P.IdTarea = @pIdTarea AND P.estado = 1"
                : @"SELECT TOP 1 U.* 
                    FROM Usuarios U
                    INNER JOIN Postulaciones P ON U.IdUsuario = P.IdUsuario
                    WHERE P.IdTarea = @pIdTarea AND P.estado = 'Aceptada'";

            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pIdTarea = idTarea });
        }
    }

    public static int ContarPostulantes(int idTarea)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            string sql = esBit
                ? "SELECT COUNT(*) FROM Postulaciones WHERE IdTarea = @pIdTarea AND estado = 1"
                : "SELECT COUNT(*) FROM Postulaciones WHERE IdTarea = @pIdTarea AND estado IN ('Pendiente', 'Aceptada')";
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
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            string sqlPostulacion = esBit
                ? "UPDATE Postulaciones SET estado = 1 WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdUsuario"
                : "UPDATE Postulaciones SET estado = 'Aceptada' WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdUsuario";
            string sqlOtros = esBit
                ? "UPDATE Postulaciones SET estado = 0 WHERE IdTarea = @pIdTarea AND IdUsuario <> @pIdUsuario"
                : "UPDATE Postulaciones SET estado = 'Rechazada' WHERE IdTarea = @pIdTarea AND IdUsuario <> @pIdUsuario AND estado = 'Pendiente'";
            string sqlTarea = "UPDATE Tareas SET estado = 0 WHERE IdTarea = @pIdTarea";

            int filas = connection.Execute(sqlPostulacion, new { pIdTarea = idTarea, pIdUsuario = idUsuario });
            connection.Execute(sqlOtros, new { pIdTarea = idTarea, pIdUsuario = idUsuario });
            connection.Execute(sqlTarea, new { pIdTarea = idTarea });
            return filas > 0;
        }
    }

    public static bool RechazarPostulante(int idTarea, int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            string sql = esBit
                ? "UPDATE Postulaciones SET estado = 0 WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdUsuario"
                : "UPDATE Postulaciones SET estado = 'Rechazada' WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdUsuario";
            int filas = connection.Execute(sql, new { pIdTarea = idTarea, pIdUsuario = idUsuario });
            return filas > 0;
        }
    }

    public static int ContarAyudasRealizadas(int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            string sql = esBit
                ? "SELECT COUNT(*) FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado = 1"
                : "SELECT COUNT(*) FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado = 'Aceptada'";
            return connection.ExecuteScalar<int>(sql, new { pIdUsuario = idUsuario });
        }
    }

    public static double ObtenerValoracionPromedio(int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            string sql = esBit
                ? "SELECT ISNULL(AVG(CAST(calificacion AS FLOAT)), 0) FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado = 1 AND calificacion > 0"
                : "SELECT ISNULL(AVG(CAST(calificacion AS FLOAT)), 0) FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado = 'Aceptada' AND calificacion > 0";
            return connection.ExecuteScalar<double>(sql, new { pIdUsuario = idUsuario });
        }
    }

    public static bool EsPuntual(int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            bool esBit = EsEstadoPostulacionesBit(connection);
            string sql = esBit
                ? "SELECT COUNT(*) FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado = 1 AND puntual = 0"
                : "SELECT COUNT(*) FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado = 'Aceptada' AND puntual = 0";
            int impuntualidades = connection.ExecuteScalar<int>(sql, new { pIdUsuario = idUsuario });
            return impuntualidades == 0;
        }
    }
}
