using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.DAO
{
    public class AccionDAO_SqlServer : ConexionDB,Interfaces.IAccionDAO
    {
        public List<Accion> Listar()
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar Acciones");

                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = $"SELECT id, descripcion, icon_name FROM acciones;";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        var acciones = new List<Accion>();
                        while (response.Read())
                        {
                            var accion = new Accion();
                            accion.ID = response.GetInt32(0);
                            accion.Descripcion = response.GetString(1);
                            accion.IconName = response.GetString(2);
                            acciones.Add(accion);
                        }
                        return acciones;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<Accion> ListarPorGrupo(int groupId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar Acciones por grupo");

                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = $"SELECT acciones.id, descripcion, icon_name FROM acciones INNER JOIN grupos_acciones ON grupos_acciones.accion_id = acciones.id WHERE grupo_id = {groupId};";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        var acciones = new List<Accion>();
                        while (response.Read())
                        {
                            var accion = new Accion();
                            accion.ID = response.GetInt32(0);
                            accion.Descripcion = response.GetString(1);
                            accion.IconName = response.GetString(2);
                            acciones.Add(accion);
                        }
                        return acciones;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<Accion> ListarPorUsuario(int userId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar Acciones por grupo");

                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = $"SELECT DISTINCT acciones.id, descripcion, icon_name, usuario_id FROM acciones INNER JOIN grupos_acciones ON grupos_acciones.accion_id = acciones.id INNER JOIN usuarios_grupos ON usuarios_grupos.grupo_id = grupos_acciones.grupo_id WHERE usuario_id IN (SELECT usuario_id FROM usuarios_grupos WHERE usuario_id = {userId});";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        var acciones = new List<Accion>();
                        while (response.Read())
                        {
                            var accion = new Accion();
                            accion.ID = response.GetInt32(0);
                            accion.Descripcion = response.GetString(1);
                            accion.IconName = response.GetString(2);
                            acciones.Add(accion);
                        }
                        return acciones;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<Accion> ListarPorVistaYUsuario(int userId, int vistaId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar Acciones");

                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = $"SELECT DISTINCT acciones.id, descripcion, icon_name, usuario_id FROM acciones INNER JOIN vistas_acciones ON vistas_acciones.accion_id = acciones.id INNER JOIN grupos_acciones ON grupos_acciones.accion_id = acciones.id INNER JOIN usuarios_grupos ON usuarios_grupos.grupo_id = grupos_acciones.grupo_id WHERE usuario_id IN (SELECT usuario_id FROM usuarios_grupos WHERE usuario_id = {userId}) AND vistas_acciones.vista_id = {vistaId};";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        var acciones = new List<Accion>();
                        while (response.Read())
                        {
                            var accion = new Accion();
                            accion.ID = response.GetInt32(0);
                            accion.Descripcion = response.GetString(1);
                            accion.IconName = response.GetString(2);
                            acciones.Add(accion);
                        }
                        return acciones;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }
    }
}
