using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.DAO
{
    public class VistaDAO_SqlServer : ConexionDB,Interfaces.IVistaDAO
    {
        public List<Vista> Listar()
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar Vistas");

                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = $"SELECT id, descripcion, icon_name, class_name FROM vistas;";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        var vistas = new List<Vista>();
                        while (response.Read())
                        {
                            var vista = new Vista();
                            vista.ID = response.GetInt32(0);
                            vista.Descripcion = response.GetString(1);
                            vista.IconName = response.GetString(2);
                            vista.AssociatedViewName = response.GetString(3);
                            vistas.Add(vista);
                        }
                        return vistas;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<Vista> ListarPorUsuario(int userId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar Vistas por Usuario");

                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = $"SELECT DISTINCT v.id, v.descripcion, v.icon_name, v.class_name FROM vistas as v INNER JOIN vistas_acciones ON v.id = vistas_acciones.vista_id INNER JOIN grupos_acciones ON vistas_acciones.accion_id = grupos_acciones.accion_id WHERE grupos_acciones.grupo_id IN (SELECT grupo_id FROM usuarios_grupos WHERE usuario_id = {userId});";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        var vistas = new List<Vista>();
                        while (response.Read())
                        {
                            var vista = new Vista();
                            vista.ID = response.GetInt32(0);
                            vista.Descripcion = response.GetString(1);
                            vista.IconName = response.GetString(2);
                            vista.AssociatedViewName = response.GetString(3);
                            vistas.Add(vista);
                        }
                        return vistas;
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
