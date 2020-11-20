using System;
using System.Collections.Generic;
using System.Text;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.DAO
{
    public class MensajeDAO_SqlServer : ConexionDB, Interfaces.IMensajeDAO
    {
        public void EnviarMensajeTema(MensajeATema mensaje)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Insertar mensaje");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"INSERT INTO mensajes_temas VALUES (@emisor_id,@tema_id,@texto,@fecha);";
                    command.Parameters.AddWithValue("@emisor_id", mensaje.Emisor.ID);
                    command.Parameters.AddWithValue("@tema_id", mensaje.Receptor.ID);
                    command.Parameters.AddWithValue("@texto", mensaje.Texto);
                    command.Parameters.AddWithValue("@fecha", mensaje.Fecha);
                    transaction.Commit();
                    command.ExecuteNonQuery();

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

        public void EnviarMensajeUsuario(MensajeAUsuario mensaje)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Insertar mensaje");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"INSERT INTO mensajes_usuarios VALUES (@emisor_id,@receptor_id,@texto,@fecha);";
                    command.Parameters.AddWithValue("@emisor_id", mensaje.Emisor.ID);
                    command.Parameters.AddWithValue("@receptor_id", mensaje.Receptor.ID);
                    command.Parameters.AddWithValue("@texto", mensaje.Texto);
                    command.Parameters.AddWithValue("@fecha", mensaje.Fecha);
                    transaction.Commit();
                    command.ExecuteNonQuery();

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

        public List<Mensaje> ObtenerMensajesTema(int temaID)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar mensajes");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT mensajes_temas.id, emisor_id, tema_id,fecha,texto, nombre, apellido from mensajes_temas INNER JOIN usuarios ON mensajes_temas.emisor_id = usuarios.id WHERE tema_id = {temaID} ORDER BY fecha";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var chat = new List<Mensaje>();
                            while (response.Read())
                            {
                                var mensaje = new MensajeATema()
                                {
                                    ID = response.GetInt32(0),
                                    Emisor = new Usuario() { ID = response.GetInt32(1), Nombre = response.GetString(5), Apellido = response.GetString(6) },
                                    Receptor = new Tema() { ID = response.GetInt32(2) },
                                    Fecha = response.GetDateTime(3),
                                    Texto = response.GetString(4)
                                };

                                chat.Add(mensaje);
                            }
                            return chat;
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

        public List<Mensaje> ObtenerMensajesUsuarios(int userID1, int userID2)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar mensajes");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT id, emisor_id, receptor_id,fecha,texto from mensajes_usuarios WHERE emisor_id = {userID1} and receptor_id ={userID2} or emisor_id = {userID2} and receptor_id ={userID1} ORDER BY fecha";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var chat = new List<Mensaje>();
                            while (response.Read())
                            {
                                var mensaje = new MensajeAUsuario()
                                {
                                    ID = response.GetInt32(0),
                                    Emisor = new Usuario() { ID = response.GetInt32(1) },
                                    Receptor = new Usuario() { ID = response.GetInt32(2) },
                                    Fecha = response.GetDateTime(3),
                                    Texto = response.GetString(4)
                                };

                                chat.Add(mensaje);
                            }
                            return chat;
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
    }
}
