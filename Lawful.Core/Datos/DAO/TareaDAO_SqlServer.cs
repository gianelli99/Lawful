using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lawful.Core.Datos.DAO
{
    public class TareaDAO_SqlServer : ConexionDB, Interfaces.ITareaDAO
    {
        public void CambiarEstado(Tarea tarea)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Cambiar estado");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"UPDATE tareas SET fecha_por_hacer=@fecha_por_hacer, fecha_en_curso=@fecha_en_curso, fecha_finalizada=@fecha_finalizada, estado=@estado WHERE id = {tarea.ID};";
                    command.Parameters.AddWithValue("@fecha_por_hacer", tarea.FechaPorHacer);
                    command.Parameters.AddWithValue("@fecha_en_curso", tarea.FechaEnCurso);
                    command.Parameters.AddWithValue("@fecha_finalizada", tarea.FechaFinalizada);
                    command.Parameters.AddWithValue("@estado", tarea.Estado.DBValue);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    return;
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {

                        throw ex2;
                    }
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public void Eliminar(int tareaId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Eliminar tarea");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"DELETE FROM incidencias WHERE tarea_id = {tareaId};DELETE FROM tareas_comentarios WHERE tarea_id = {tareaId};DELETE FROM tareas WHERE id = {tareaId};";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    return;
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {

                        throw ex2;
                    }
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public void Insertar(Tarea tarea, int temaId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Insertar tarea");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"INSERT INTO tareas (titulo,descripcion,fecha_por_hacer,fecha_en_curso,fecha_finalizada,importancia,usuario_id,tema_id,estado) VALUES (@titulo, @descripcion, @fecha_por_hacer,@fecha_en_curso,@fecha_finalizada, @importancia, @usuario_id, @tema_id, @estado);SELECT CAST(scope_identity() AS int);";
                    command.Parameters.AddWithValue("@titulo", tarea.Titulo);
                    command.Parameters.AddWithValue("@descripcion", tarea.Descripcion);
                    command.Parameters.AddWithValue("@fecha_por_hacer", tarea.FechaPorHacer);
                    command.Parameters.AddWithValue("@fecha_en_curso", tarea.FechaEnCurso);
                    command.Parameters.AddWithValue("@fecha_finalizada", tarea.FechaFinalizada);
                    command.Parameters.AddWithValue("@importancia", tarea.Importancia);
                    command.Parameters.AddWithValue("@usuario_id", tarea.Responsable.ID);
                    command.Parameters.AddWithValue("@tema_id", temaId);
                    command.Parameters.AddWithValue("@estado", tarea.Estado.DBValue);
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            tarea.ID = response.GetInt32(0);
                        }
                    }
                    command.CommandText = "INSERT INTO incidencias (descripcion, is_done, tarea_id) VALUES";
                    if (tarea.IncidenciasSecundarias.Count > 0)
                    {
                        foreach (var incidencia in tarea.IncidenciasSecundarias)
                        {
                            var isdone = incidencia.IsDone ? 1 : 0;

                            command.CommandText += $"('{incidencia.Descripcion}',{isdone},{tarea.ID}),"; // command.Parameters.AddWithValue("@desc", incidencia.Descripcion); // Se removió el addWithValue porque rompía y decia "No se puede repetir el @descripcion" 
                        }
                        command.CommandText = command.CommandText.Remove(command.CommandText.Length - 1);
                        command.CommandText += ";";

                        command.ExecuteNonQuery();
                    }


                    transaction.Commit();
                    return;
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {

                        throw ex2;
                    }
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<Tarea> ListarPorTema(int temaId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar tareas");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT titulo, descripcion, fecha_por_hacer, fecha_en_curso, fecha_finalizada, importancia, usuario_id, nombre, apellido, tareas.id, tareas.estado FROM tareas INNER JOIN usuarios ON usuarios.id = tareas.usuario_id WHERE tema_id = {temaId};";
                    command.CommandText += $"SELECT tareas_comentarios.id, tareas_comentarios.descripcion, tareas_comentarios.fecha, tareas_comentarios.usuario_id, tarea_id, nombre, apellido FROM tareas_comentarios INNER JOIN tareas ON tareas.id = tarea_id INNER JOIN usuarios ON usuarios.id = tareas_comentarios.usuario_id WHERE tareas.tema_id = {temaId};";
                    command.CommandText += $"SELECT incidencias.id, incidencias.descripcion, is_done, incidencias.tarea_id FROM incidencias INNER JOIN tareas ON tareas.id = incidencias.tarea_id WHERE tema_id = {temaId};";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var tareas = new List<Tarea>();
                            while (response.Read())
                            {
                                var tarea = new Tarea()
                                {
                                    ID = response.GetInt32(9),
                                    Titulo = response.GetString(0),
                                    Descripcion = response.GetString(1),
                                    FechaPorHacer = response.GetDateTime(2),
                                    FechaEnCurso = response.IsDBNull(3) ? DateTime.MinValue : response.GetDateTime(3),
                                    FechaFinalizada = response.IsDBNull(4) ? DateTime.MinValue : response.GetDateTime(4),
                                    //FechaEnCurso = response.GetDateTime(3),
                                    //FechaFinalizada = response.GetDateTime(4),
                                    Importancia = response.GetInt32(5),
                                };
                                var user = new Usuario()
                                {
                                    ID = response.GetInt32(6),
                                    Nombre = response.GetString(7),
                                    Apellido = response.GetString(8)
                                };
                                tarea.Responsable = user;
                                SetState(tarea, response.GetInt32(10)); //Era 9
                                tareas.Add(tarea);
                            }
                            response.NextResult();
                            while (response.Read())
                            {
                                var comentario = new Comentario()
                                {
                                    ID = response.GetInt32(0),
                                    Descripcion = response.GetString(1),
                                    Fecha = response.GetDateTime(2),
                                };
                                var user = new Usuario()
                                {
                                    ID = response.GetInt32(3),
                                    Nombre = response.GetString(5),
                                    Apellido = response.GetString(6)
                                };
                                comentario.Owner = user;
                                int tareaId = response.GetInt32(4);
                                foreach (var tarea in tareas)
                                {
                                    if (tarea.ID == tareaId)
                                    {
                                        tarea.Comentarios.Add(comentario);
                                        break;
                                    }
                                }
                            }
                            response.NextResult();
                            while (response.Read())
                            {
                                var incidencia = new Incidencia()
                                {
                                    ID = response.GetInt32(0),
                                    Descripcion = response.GetString(1),
                                    IsDone = response.GetBoolean(2)
                                };
                                int tareaId = response.GetInt32(3);
                                foreach (var tarea in tareas)
                                {
                                    if (tarea.ID == tareaId)
                                    {
                                        tarea.IncidenciasSecundarias.Add(incidencia);
                                        break;
                                    }
                                }
                            }

                            return tareas;
                        }
                        return new List<Tarea>();
                    }
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        private void SetState(Tarea tarea, int dbStateValue)
        {
            switch (dbStateValue)
            {
                case 1:
                    tarea.ChangeState(new Modelo.TareaEstados.PorHacer());
                    break;
                case 2:
                    tarea.ChangeState(new Modelo.TareaEstados.EnCurso());
                    break;
                case 3:
                    tarea.ChangeState(new Modelo.TareaEstados.Finalizada());
                    break;
                default:
                    break;
            }
        }

        public void Modificar(Tarea tarea)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Modificar tarea");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"UPDATE tareas SET titulo=@titulo, descripcion=@descripcion, importancia=@importancia, usuario_id=@usuarios_id WHERE id = {tarea.ID};";
                    command.Parameters.AddWithValue("@titulo", tarea.Titulo);
                    command.Parameters.AddWithValue("@descripcion", tarea.Descripcion);
                    command.Parameters.AddWithValue("@importancia", tarea.Importancia);
                    command.Parameters.AddWithValue("@usuarios_id", tarea.Responsable.ID);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    return;
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {

                        throw ex2;
                    }
                }
            }
            throw new Exception("Ha ocurrido un error");
        }
        public void InsertarComentario(int tareaID, Comentario comentario)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Insertar comentario");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"INSERT INTO tareas_comentarios VALUES (@descripcion, @fecha, @usuario_id, @tarea_id);";
                    command.Parameters.AddWithValue("@descripcion", comentario.Descripcion);
                    command.Parameters.AddWithValue("@fecha", comentario.Fecha);
                    command.Parameters.AddWithValue("@usuario_id", comentario.Owner.ID);
                    command.Parameters.AddWithValue("@tarea_id", tareaID);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    return;
                }
                catch (Exception)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {

                        throw ex2;
                    }
                }
            }
            throw new Exception("Ha ocurrido un error");
        }
    }
}
