using System;
using System.Collections.Generic;
using System.Text;
using Lawful.Core.Datos;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.DAO
{
    public class TareaDAO_SqlServer : ConexionDB, Interfaces.ITareaDAO
    {
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
                    command.CommandText = $"INSERT INTO tareas (titulo,descripcion,fecha_por_hacer,importancia,usuario_id,tema_id) VALUES (@titulo, @descripcion, @fecha_por_hacer, @importancia, @usuario_id, @tema_id);SELECT CAST(scope_identity() AS int);";
                    command.Parameters.AddWithValue("@titulo", tarea.Titulo);
                    command.Parameters.AddWithValue("@descripcion", tarea.Descripcion);
                    command.Parameters.AddWithValue("@fecha_por_hacer", tarea.FechaPorHacer);
                    command.Parameters.AddWithValue("@importancia", tarea.Importancia);
                    command.Parameters.AddWithValue("@usuario_id", tarea.Responsable.ID);
                    command.Parameters.AddWithValue("@tema_id", temaId);
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            tarea.ID = response.GetInt32(0);
                        }
                    }
                    command.CommandText = "INSERT INTO incidencias VALUES";
                    if (tarea.IncidenciasSecundarias.Count > 0)
                    {
                        foreach (var incidencia in tarea.IncidenciasSecundarias)
                        {
                            command.CommandText += $"(@descripcion,{incidencia.IsDone},{tarea.ID}),";
                            command.Parameters.AddWithValue("@descripcion", incidencia.Descripcion);
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
                    command.CommandText = $"SELECT titulo, descripcion, fecha_por_hacer, fecha_en_curso, fecha_finalizada, importancia, usuario_id, nombre, apellido FROM tareas INNER JOIN usuarios ON usuarios.id = tareas.usuario_id WHERE tema_id = {temaId};";
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
                                    ID = response.GetInt32(0),
                                    Descripcion = response.GetString(1),
                                    FechaPorHacer = response.GetDateTime(2),
                                    FechaEnCurso = response.IsDBNull(3) ? DateTime.MinValue : response.GetDateTime(3),
                                    FechaFinalizada = response.IsDBNull(4) ? DateTime.MinValue : response.GetDateTime(4),
                                    Importancia = response.GetInt32(5),
                                };
                                var user = new Usuario()
                                {
                                    ID = response.GetInt32(6),
                                    Nombre = response.GetString(7),
                                    Apellido = response.GetString(8)
                                };
                                tarea.Responsable = user;
                                tareas.Add(tarea);
                            }
                            return temas;
                        }
                        return null;
                    }
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public void Modificar(Tarea tarea)
        {
            throw new NotImplementedException();
        }
    }
}
