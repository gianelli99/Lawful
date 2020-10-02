using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

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
                    command.CommandText = $"SELECT id, descripcion, estado, fecha_creacion, fecha_cierre, everyone_can_edit FROM temas WHERE id = {id};";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            var tema = new Modelo.Tema();

                            tema.ID = response.GetInt32(0);
                            tema.Descripcion = response.GetString(1);
                            tema.Estado = response.GetBoolean(2);
                            tema.FechaCreacion = response.GetDateTime(3);
                            tema.FechaCierre = response.GetDateTime(4);
                            tema.EveryoneCanEdit = response.GetBoolean(5);
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
                    command.CommandText = $"INSERT INTO temas VALUES (@descripcion, @estado, @fecha_creacion, @fecha_cierre, @everyone_can_edit);";
                    command.Parameters.AddWithValue("@descripcion", tema.Descripcion);
                    command.Parameters.AddWithValue("@estado", 1);
                    command.Parameters.AddWithValue("@fecha_creacion", tema.FechaCreacion);
                    command.Parameters.AddWithValue("@fecha_cierre", tema.FechaCierre);
                    command.Parameters.AddWithValue("@everyone_can_edit", tema.EveryoneCanEdit);
                    
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

        public List<Tema> Listar()
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
                    command.CommandText = $"SELECT id, descripcion, estado, fecha_creacion, fecha_cierre, everyone_can_edit FROM temas WHERE estado = 1";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var temas = new List<Modelo.Tema>();
                            while (response.Read())
                            {
                                var tema = new Modelo.Tema();

                                tema.ID = response.GetInt32(0);
                                tema.Descripcion = response.GetString(1);
                                tema.Estado = response.GetBoolean(2);
                                tema.FechaCreacion = response.GetDateTime(3);
                                tema.FechaCierre = response.GetDateTime(4);
                                tema.EveryoneCanEdit = response.GetBoolean(5);
                                temas.Add(tema);
                            }
                            return temas;
                        }
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
                transaction = connection.BeginTransaction("Insertar Tema");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"UPDATE temas SET descripcion=@descripcion, estado=@estado, fecha_creacion=@fecha_creacion, fecha_cierre=@fecha_cierre, everyone_can_edit=@everyone_can_edit WHERE id = {tema.ID};";
                    command.Parameters.AddWithValue("@descripcion", tema.Descripcion);
                    command.Parameters.AddWithValue("@estado", tema.Estado);
                    command.Parameters.AddWithValue("@fecha_creacion", tema.FechaCreacion);
                    command.Parameters.AddWithValue("@fecha_cierre", tema.FechaCierre);
                    command.Parameters.AddWithValue("@everyone_can_edit", tema.EveryoneCanEdit);

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
