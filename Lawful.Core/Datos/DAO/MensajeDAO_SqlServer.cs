using System;
using System.Collections.Generic;
using System.Text;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.DAO
{
    public class MensajeDAO_SqlServer : ConexionDB, Interfaces.IMensajeDAO
    {
        public void EnviarMensaje(Mensaje mensaje)
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
                    command.CommandText = $"INSERT INTO mensajes VALUES (@emisor_id,@receptor_id,@texto,@fecha);";
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

        public List<Mensaje> ObtenerMensajes(int userID1, int userID2)
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
                    command.CommandText = $"SELECT id, emisor_id, receptor_id,fecha,texto from mensajes WHERE emisor_id = {userID1} and receptor_id ={userID2} or emisor_id = {userID2} and receptor_id ={userID1} ORDER BY fecha";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var chat = new List<Mensaje>();
                            while (response.Read())
                            {
                                var mensaje = new Mensaje()
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
