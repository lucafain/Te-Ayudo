using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace TeAyudo.Models;

public static class BD
{
    private static string _connectionString = @"Server=localhost;DataBase=Te_Ayudo;Integrated Security=True;TrustServerCertificate=True;";

    public static Usuarios ObtenerUsuarioPorEmail(string email)
    {
        string sql = "SELECT * FROM Usuarios WHERE email = @pEmail";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pEmail = email });
        }
    }

    public static Usuarios ObtenerUsuarioPorCredenciales(string email, string contrasena)
    {
        string sql = "SELECT * FROM Usuarios WHERE email = @pEmail AND contrasena = @pContrasena";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pEmail = email, pContrasena = contrasena });
        }
    }

    public static Usuarios ObtenerUsuarioPorId(int id)
    {
        string sql = "SELECT * FROM Usuarios WHERE IdUsuario = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pId = id });
        }
    }

    public static Usuarios ObtenerPrimerUsuario()
    {
        string sql = "SELECT TOP 1 * FROM Usuarios";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql);
        }
    }

    public static bool GuardarUsuario(Usuarios usuario)
    {
        if (usuario == null || usuario.Email == "")
        {
            return false;
        }

        if (ObtenerUsuarioPorEmail(usuario.Email) != null)
        {
            return false;
        }

        string sql = @"INSERT INTO Usuarios 
            (nombre, apellido, email, provincia, direccion, calle, codigoPostal, fechaRegistro, tipoUsuario, contrasena, aceptaTerminos, fotoPerfil)
            VALUES 
            (@pNombre, @pApellido, @pEmail, @pProvincia, @pDireccion, @pCalle, @pCodigoPostal, @pFechaRegistro, @pTipoUsuario, @pContrasena, @pAceptaTerminos, @pFotoPerfil)";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new
            {
                pNombre = usuario.Nombre,
                pApellido = usuario.Apellido,
                pEmail = usuario.Email,
                pProvincia = usuario.Provincia,
                pDireccion = usuario.Direccion,
                pCalle = usuario.Calle,
                pCodigoPostal = usuario.CodigoPostal,
                pFechaRegistro = usuario.FechaRegistro,
                pTipoUsuario = usuario.TipoUsuario,
                pContrasena = usuario.Contrasena,
                pAceptaTerminos = usuario.AceptaTerminos,
                pFotoPerfil = usuario.FotoPerfil
            });

            return registrosAfectados > 0;
        }
    }

    public static bool ActualizarFotoPerfil(int idUsuario, string fotoPerfil)
    {
        string sql = "UPDATE Usuarios SET fotoPerfil = @pFoto WHERE IdUsuario = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new { pFoto = fotoPerfil, pId = idUsuario });
            return registrosAfectados > 0;
        }
    }

    public static Usuarios ObtenerHistorialJoven(int idJoven)
    {
        return ObtenerUsuarioPorId(idJoven);
    }

    public static List<Usuarios> ObtenerUltimosAyudados(int idJoven)
    {
        string sql = @"SELECT U.* 
            FROM Usuarios U
            INNER JOIN Tareas T ON U.IdUsuario = T.IdUsuarioSolicitante
            INNER JOIN Postulaciones P ON T.IdTarea = P.IdTarea
            WHERE P.IdUsuario = @pIdJoven AND P.estado IN ('Aceptada', 'Finalizada')
            ORDER BY P.IdPostulacion DESC";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<Usuarios> lista = connection.Query<Usuarios>(sql, new { pIdJoven = idJoven }).ToList();
            List<Usuarios> sinRepetidos = new List<Usuarios>();
            Dictionary<int, Usuarios> vistos = new Dictionary<int, Usuarios>();

            foreach (Usuarios u in lista)
            {
                if (!vistos.ContainsKey(u.IdUsuario))
                {
                    vistos.Add(u.IdUsuario, u);
                    sinRepetidos.Add(u);
                }
            }

            return sinRepetidos;
        }
    }

    public static List<Usuarios> ObtenerPersonasQueAyudaron(int idAdulto)
    {
        string sql = @"SELECT U.* 
            FROM Usuarios U
            INNER JOIN Postulaciones P ON U.IdUsuario = P.IdUsuario
            INNER JOIN Tareas T ON P.IdTarea = T.IdTarea
            WHERE T.IdUsuarioSolicitante = @pIdAdulto AND P.estado IN ('Aceptada', 'Finalizada')
            ORDER BY P.IdPostulacion DESC";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<Usuarios> lista = connection.Query<Usuarios>(sql, new { pIdAdulto = idAdulto }).ToList();
            List<Usuarios> sinRepetidos = new List<Usuarios>();
            Dictionary<int, Usuarios> vistos = new Dictionary<int, Usuarios>();

            foreach (Usuarios u in lista)
            {
                if (!vistos.ContainsKey(u.IdUsuario))
                {
                    vistos.Add(u.IdUsuario, u);
                    sinRepetidos.Add(u);
                }
            }

            return sinRepetidos;
        }
    }

    public static List<Tareas> ObtenerTareasRealizadasPorJoven(int idJoven)
    {
        string sql = @"SELECT T.* 
            FROM Tareas T
            INNER JOIN Postulaciones P ON T.IdTarea = P.IdTarea
            WHERE P.IdUsuario = @pIdJoven AND P.estado IN ('Aceptada', 'Finalizada')
            ORDER BY T.IdTarea DESC";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Tareas>(sql, new { pIdJoven = idJoven }).ToList();
        }
    }

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
        string sql = @"INSERT INTO Tareas 
            (titulo, descripcion, estado, fechaCreacion, direccion, diaTarea, horaTarea, aclaracion, monto, tipoLugar, IdUsuarioSolicitante, barrio)
            VALUES 
            (@pTitulo, @pDescripcion, @pEstado, @pFechaCreacion, @pDireccion, @pDiaTarea, @pHoraTarea, @pAclaracion, @pMonto, @pTipoLugar, @pIdUsuarioSolicitante, @pBarrio)";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new
            {
                pTitulo = tarea.Titulo,
                pDescripcion = tarea.Descripcion,
                pEstado = tarea.Estado,
                pFechaCreacion = tarea.FechaCreacion,
                pDireccion = tarea.Lugar,
                pDiaTarea = tarea.DiaTarea,
                pHoraTarea = tarea.HoraTarea,
                pAclaracion = tarea.Aclaracion,
                pMonto = tarea.Monto,
                pTipoLugar = tarea.TipoLugar,
                pIdUsuarioSolicitante = tarea.IdUsuarioSolicitante,
                pBarrio = tarea.Barrio
            });

            return registrosAfectados > 0;
        }
    }

    public static bool ModificarTarea(Tareas tarea)
    {
        Tareas actual = ObtenerTareaPorId(tarea.IdTarea);
        if (actual == null || !actual.Estado || ContarPostulantes(tarea.IdTarea) > 0)
        {
            return false;
        }

        string sql = @"UPDATE Tareas SET 
            titulo = @pTitulo,
            descripcion = @pDescripcion,
            direccion = @pDireccion,
            diaTarea = @pDiaTarea,
            horaTarea = @pHoraTarea,
            aclaracion = @pAclaracion,
            monto = @pMonto,
            tipoLugar = @pTipoLugar,
            barrio = @pBarrio
            WHERE IdTarea = @pIdTarea";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new
            {
                pTitulo = tarea.Titulo,
                pDescripcion = tarea.Descripcion,
                pDireccion = tarea.Lugar,
                pDiaTarea = tarea.DiaTarea,
                pHoraTarea = tarea.HoraTarea,
                pAclaracion = tarea.Aclaracion,
                pMonto = tarea.Monto,
                pTipoLugar = tarea.TipoLugar,
                pBarrio = tarea.Barrio,
                pIdTarea = tarea.IdTarea
            });

            return registrosAfectados > 0;
        }
    }

    public static List<Tareas> ObtenerTareasPorSolicitante(int idUsuarioSolicitante)
    {
        string sql = "SELECT * FROM Tareas WHERE IdUsuarioSolicitante = @pId ORDER BY IdTarea DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<Tareas> misTareas = connection.Query<Tareas>(sql, new { pId = idUsuarioSolicitante }).ToList();
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

    public static List<Tareas> ObtenerTareasPendientes(int idJoven)
    {
        string sql = @"SELECT T.* 
            FROM Tareas T
            WHERE (T.estado = 1 AND NOT EXISTS (SELECT 1 FROM Postulaciones P WHERE P.IdTarea = T.IdTarea AND P.IdUsuario = @pIdJoven))
               OR EXISTS (SELECT 1 FROM Postulaciones P2 WHERE P2.IdTarea = T.IdTarea AND P2.IdUsuario = @pIdJoven AND P2.estado IN ('Aceptada', 'Rechazada'))
            ORDER BY T.IdTarea DESC";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Tareas>(sql, new { pIdJoven = idJoven }).ToList();
        }
    }

    public static Dictionary<int, string> ObtenerEstadosPostulacionesJoven(int idJoven)
    {
        Dictionary<int, string> estados = new Dictionary<int, string>();
        string sql = "SELECT * FROM Postulaciones WHERE IdUsuario = @pIdJoven AND estado IN ('Aceptada', 'Rechazada', 'Finalizada')";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<Postulaciones> lista = connection.Query<Postulaciones>(sql, new { pIdJoven = idJoven }).ToList();
            foreach (Postulaciones p in lista)
            {
                if (!estados.ContainsKey(p.IdTarea))
                {
                    estados.Add(p.IdTarea, p.Estado);
                }
            }
        }
        return estados;
    }

    public static bool FinalizarTareaAceptada(int idTarea, int idJoven)
    {
        string sql = "UPDATE Postulaciones SET estado = 'Finalizada' WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdJoven AND estado = 'Aceptada'";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new { pIdTarea = idTarea, pIdJoven = idJoven });
            return registrosAfectados > 0;
        }
    }

    public static bool EsTareaFinalizada(int idTarea)
    {
        string sql = "SELECT * FROM Postulaciones WHERE IdTarea = @pIdTarea AND estado = 'Finalizada'";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            Postulaciones postulacion = connection.QueryFirstOrDefault<Postulaciones>(sql, new { pIdTarea = idTarea });
            return postulacion != null;
        }
    }

    public static bool EliminarTareaFinalizadaAdulto(int idTarea)
    {
        string sqlPost = "DELETE FROM Postulaciones WHERE IdTarea = @pIdTarea";
        string sqlTarea = "DELETE FROM Tareas WHERE IdTarea = @pIdTarea";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(sqlPost, new { pIdTarea = idTarea });
            int registrosAfectados = connection.Execute(sqlTarea, new { pIdTarea = idTarea });
            return registrosAfectados > 0;
        }
    }

    public static bool GuardarValoracionJoven(int idTarea, double calificacion, bool puntual)
    {
        string sql = "UPDATE Postulaciones SET calificacion = @pCalificacion, puntual = @pPuntual WHERE IdTarea = @pIdTarea AND estado = 'Finalizada'";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new { pCalificacion = calificacion, pPuntual = puntual, pIdTarea = idTarea });
            return registrosAfectados > 0;
        }
    }

    public static bool AceptarTarea(int idTarea)
    {
        string sql = "UPDATE Tareas SET estado = 0 WHERE IdTarea = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new { pId = idTarea });
            return registrosAfectados > 0;
        }
    }

    public static bool RechazarTarea(int idTarea, int idJoven)
    {
        Postulaciones descarte = new Postulaciones(idJoven, idTarea, "Descartada", 0, true);
        return GuardarPostulacion(descarte);
    }

    public static Tareas ObtenerTareaPorId(int idTarea)
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
            string sqlBuscar = "SELECT * FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND IdTarea = @pIdTarea";
            Postulaciones existente = connection.QueryFirstOrDefault<Postulaciones>(sqlBuscar, new
            {
                pIdUsuario = postulacion.IdUsuario,
                pIdTarea = postulacion.IdTarea
            });

            if (existente != null)
            {
                string sqlUpdate = "UPDATE Postulaciones SET estado = @pEstado WHERE IdUsuario = @pIdUsuario AND IdTarea = @pIdTarea";
                int actualizados = connection.Execute(sqlUpdate, new
                {
                    pEstado = postulacion.Estado,
                    pIdUsuario = postulacion.IdUsuario,
                    pIdTarea = postulacion.IdTarea
                });
                return actualizados > 0;
            }

            string sqlInsert = @"INSERT INTO Postulaciones (IdUsuario, IdTarea, estado, calificacion, puntual)
                VALUES (@pIdUsuario, @pIdTarea, @pEstado, @pCalificacion, @pPuntual)";

            int registrosAfectados = connection.Execute(sqlInsert, new
            {
                pIdUsuario = postulacion.IdUsuario,
                pIdTarea = postulacion.IdTarea,
                pEstado = postulacion.Estado,
                pCalificacion = postulacion.Calificacion,
                pPuntual = postulacion.Puntual
            });

            return registrosAfectados > 0;
        }
    }

    public static List<Usuarios> ObtenerPostuladosPorTarea(int idTarea)
    {
        string sql = @"SELECT U.* 
            FROM Usuarios U
            INNER JOIN Postulaciones P ON U.IdUsuario = P.IdUsuario
            WHERE P.IdTarea = @pIdTarea AND P.estado IN ('Pendiente', 'Aceptada')";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.Query<Usuarios>(sql, new { pIdTarea = idTarea }).ToList();
        }
    }

    public static Usuarios ObtenerJovenAceptadoPorTarea(int idTarea)
    {
        Tareas tarea = ObtenerTareaPorId(idTarea);
        if (tarea == null || tarea.Estado)
        {
            return null;
        }

        string sql = @"SELECT TOP 1 U.* 
            FROM Usuarios U
            INNER JOIN Postulaciones P ON U.IdUsuario = P.IdUsuario
            WHERE P.IdTarea = @pIdTarea AND P.estado IN ('Aceptada', 'Finalizada')";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            return connection.QueryFirstOrDefault<Usuarios>(sql, new { pIdTarea = idTarea });
        }
    }

    public static int ContarPostulantes(int idTarea)
    {
        List<Usuarios> lista = ObtenerPostuladosPorTarea(idTarea);
        return lista.Count;
    }

    public static bool CancelarTarea(int idTarea)
    {
        if (ContarPostulantes(idTarea) > 0)
        {
            return false;
        }

        string sqlPostulaciones = "DELETE FROM Postulaciones WHERE IdTarea = @pId";
        string sqlTarea = "DELETE FROM Tareas WHERE IdTarea = @pId";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(sqlPostulaciones, new { pId = idTarea });
            int registrosAfectados = connection.Execute(sqlTarea, new { pId = idTarea });
            return registrosAfectados > 0;
        }
    }

    public static bool AceptarPostulante(int idTarea, int idUsuario)
    {
        string sqlPostulacion = "UPDATE Postulaciones SET estado = 'Aceptada' WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdUsuario";
        string sqlOtros = "UPDATE Postulaciones SET estado = 'Rechazada' WHERE IdTarea = @pIdTarea AND IdUsuario <> @pIdUsuario AND estado = 'Pendiente'";
        string sqlTarea = "UPDATE Tareas SET estado = 0 WHERE IdTarea = @pIdTarea";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sqlPostulacion, new { pIdTarea = idTarea, pIdUsuario = idUsuario });
            connection.Execute(sqlOtros, new { pIdTarea = idTarea, pIdUsuario = idUsuario });
            connection.Execute(sqlTarea, new { pIdTarea = idTarea });
            return registrosAfectados > 0;
        }
    }

    public static bool RechazarPostulante(int idTarea, int idUsuario)
    {
        string sql = "UPDATE Postulaciones SET estado = 'Rechazada' WHERE IdTarea = @pIdTarea AND IdUsuario = @pIdUsuario";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            int registrosAfectados = connection.Execute(sql, new { pIdTarea = idTarea, pIdUsuario = idUsuario });
            return registrosAfectados > 0;
        }
    }

    public static int ContarAyudasRealizadas(int idUsuario)
    {
        string sql = "SELECT * FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado IN ('Aceptada', 'Finalizada')";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<Postulaciones> lista = connection.Query<Postulaciones>(sql, new { pIdUsuario = idUsuario }).ToList();
            return lista.Count;
        }
    }

    public static double ObtenerValoracionPromedio(int idUsuario)
    {
        string sql = "SELECT * FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado IN ('Aceptada', 'Finalizada') AND calificacion > 0";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<Postulaciones> lista = connection.Query<Postulaciones>(sql, new { pIdUsuario = idUsuario }).ToList();
            if (lista.Count == 0)
            {
                return 0;
            }
            double suma = 0;
            foreach (Postulaciones p in lista)
            {
                suma = suma + p.Calificacion;
            }
            return suma / lista.Count;
        }
    }

    public static bool EsPuntual(int idUsuario)
    {
        string sql = "SELECT * FROM Postulaciones WHERE IdUsuario = @pIdUsuario AND estado IN ('Aceptada', 'Finalizada') AND puntual = 0";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            List<Postulaciones> lista = connection.Query<Postulaciones>(sql, new { pIdUsuario = idUsuario }).ToList();
            return lista.Count == 0;
        }
    }
}
