using System;
using System.Collections.Generic;
using System.Text;
using Lawful.Core.Datos;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.DAO
{
    public class IncidenciaDAO_SqlServer : ConexionDB, Interfaces.IIncidenciaDAO
    {
        public void Agregar(Incidencia incidencia, int tareaId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("agregar incidencia");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"INSERT INTO incidencias VALUES (@descripcion,@is_done,@tarea_id);";
                    command.Parameters.AddWithValue("@descripcion", incidencia.Descripcion);
                    command.Parameters.AddWithValue("@is_done", incidencia.IsDone);
                    command.Parameters.AddWithValue("@tarea_id", tareaId);
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

        public void CambiarEstado(int id, bool isDone)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Eliminar incidencia");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"UPDATE incidencias SET is_done={isDone} WHERE id = {id};";
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

        public void Eliminar(int id)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Eliminar incidencia");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"DELETE FROM incidencias WHERE id = {id};";
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

        public void Modificar(Incidencia incidencia)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Eliminar incidencia");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"UPDATE incidencias SET descripcion=@descripcion WHERE id = {incidencia.ID};";
                    command.Parameters.AddWithValue("@descripcion", incidencia.Descripcion);
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
