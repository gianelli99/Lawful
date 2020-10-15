using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace Lawful.Core.Datos.DAO
{
    public class TemaDAO_SqlServer : ConexionDB, Interfaces.ITemaDAO
    {
        public Tema Consultar(int id)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Consulta Tema");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT id, descripcion, estado, fecha_creacion, fecha_cierre, everyone_can_edit,titulo FROM temas WHERE id = {id}; SELECT usuarios.id, nombre, apellido FROM usuarios INNER JOIN usuarios_temas ON usuarios.id = usuarios_temas.usuario_id WHERE tema_id = {id};";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            var tema = new Tema();
                            var usuarios = new List<Usuario>();
                            tema.Usuarios = usuarios;
                            tema.ID = response.GetInt32(0);
                            tema.Descripcion = response.GetString(1);
                            tema.Estado = response.GetBoolean(2);
                            tema.FechaCreacion = response.GetDateTime(3);
                            tema.FechaCierre = response.GetDateTime(4);
                            tema.EveryoneCanEdit = response.GetBoolean(5);
                            tema.Titulo = response.GetString(6);

                            response.NextResult();
                            while (response.Read())
                            {
                                var user = new Usuario
                                {
                                    ID = response.GetInt32(0),
                                    Nombre = response.GetString(1),
                                    Apellido = response.GetString(2)
                                };
                                tema.Usuarios.Add(user);
                            }

                            return tema;
                        }
                    }
                    throw new Exception("No se ha podido encontrar el tema");
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public void Eliminar(int id)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Eliminar tema");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"UPDATE temas set estado=0 WHERE id = {id}";
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

        public void Insertar(Tema tema)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Insertar Tema");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"INSERT INTO temas VALUES (@descripcion, @estado, @fecha_creacion, @fecha_cierre, @everyone_can_edit, @titulo);SELECT CAST(scope_identity() AS int);";
                    command.Parameters.AddWithValue("@descripcion", tema.Descripcion);
                    command.Parameters.AddWithValue("@estado", 1);
                    command.Parameters.AddWithValue("@fecha_creacion", tema.FechaCreacion);
                    command.Parameters.AddWithValue("@fecha_cierre", tema.FechaCierre);
                    command.Parameters.AddWithValue("@everyone_can_edit", tema.EveryoneCanEdit);
                    command.Parameters.AddWithValue("@titulo", tema.Titulo);
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            tema.ID = response.GetInt32(0);
                        }
                    }

                    if (tema.Usuarios.Count>0)
                    {
                        string accionesQuery = "INSERT INTO usuarios_temas VALUES";
                        foreach (var usuario in tema.Usuarios)
                        {
                            accionesQuery += $"({usuario.ID}, {tema.ID}),";
                        }
                        accionesQuery = accionesQuery.Remove(accionesQuery.Length - 1);
                        accionesQuery += ";";
                        command.CommandText += accionesQuery;
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

        public List<Tema> ListarPorUsuario(int userId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar temas");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT temas.id, descripcion, estado, fecha_creacion, fecha_cierre, everyone_can_edit, titulo FROM temas INNER JOIN usuarios_temas ON temas.id = usuarios_temas.tema_id WHERE estado = 1 AND usuarios_temas.usuario_id = {userId}";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var temas = new List<Tema>();
                            while (response.Read())
                            {
                                var tema = new Tema
                                {
                                    ID = response.GetInt32(0),
                                    Descripcion = response.GetString(1),
                                    Estado = response.GetBoolean(2),
                                    FechaCreacion = response.GetDateTime(3),
                                    FechaCierre = response.GetDateTime(4),
                                    EveryoneCanEdit = response.GetBoolean(5),
                                    Titulo = response.GetString(6)
                                };

                                temas.Add(tema);
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

        public void Modificar(Tema tema)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Modificar Tema");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    Trace.WriteLine($"El id del tema ${tema.Titulo} es ${tema.ID}", "Tema ID");

                    command.CommandText = $"UPDATE temas SET descripcion=@descripcion, estado=@estado, fecha_creacion=@fecha_creacion, fecha_cierre=@fecha_cierre, everyone_can_edit=@everyone_can_edit, titulo=@titulo WHERE id = {tema.ID};";
                    command.Parameters.AddWithValue("@descripcion", tema.Descripcion);
                    command.Parameters.AddWithValue("@estado", tema.Estado);
                    command.Parameters.AddWithValue("@fecha_creacion", tema.FechaCreacion);
                    command.Parameters.AddWithValue("@fecha_cierre", tema.FechaCierre);
                    command.Parameters.AddWithValue("@everyone_can_edit", tema.EveryoneCanEdit);
                    command.Parameters.AddWithValue("@titulo", tema.Titulo);
                  
                    command.CommandText += $"DELETE FROM usuarios_temas WHERE tema_id = {tema.ID};";
                    string usuariosQuery = "";
                    foreach (var usuario in tema.Usuarios)
                    {
                        usuariosQuery += $"INSERT INTO usuarios_temas VALUES({usuario.ID},{tema.ID});";
                        Trace.WriteLine($"El id del usuario ${usuario.GetNombreCompleto() } es ${usuario.ID}, mientras que el tema es ${tema.ID}", "usuarios_temas");
                    }
                    command.CommandText += usuariosQuery;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    /*
                     
                    - [x] 1. Dropear todos los usuariostemas donde el temaid.
                    - [x] 2. Copiar insert de usuariostemas del insert tema. 
                    - [] Hacer que la fecha de inicio no sea una fecha nueva si no que sea la que ya estaba seteada.

                     */


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
