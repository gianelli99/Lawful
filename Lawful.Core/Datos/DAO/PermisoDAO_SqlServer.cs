using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.DAO
{
    public class PermisoDAO_SqlServer : ConexionDB ,Interfaces.IPermisoDAO
    {
        public List<Accion> ListarAcciones()
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar acciones");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT * FROM acciones";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var acciones = new List<Modelo.Accion>();
                            while (response.Read())
                            {
                                var accion = new Modelo.Accion()
                                {
                                    ID = response.GetInt32(0),
                                    Descripcion = response.GetString(1)
                                };
                                acciones.Add(accion);
                            }
                            return acciones;
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
        public List<int[]> ListarIDPermisos()
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar ID permisos");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT DISTINCT accion_id, vista_id FROM permisos";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var permisos = new List<int[]>();
                            while (response.Read())
                            {
                                var permiso = new int[2];
                                permiso[0] = response.GetInt32(0);//accion id
                                permiso[1] = response.GetInt32(1);//vista id
                                permisos.Add(permiso);
                            }
                            return permisos;
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
        public List<Modelo.Vista> ListarVistas()
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar ID permisos");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT * FROM vistas";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var vistas = new List<Modelo.Vista>();
                            while (response.Read())
                            {
                                var vista = new Modelo.Vista()
                                {
                                    ID = response.GetInt32(0),
                                    Descripcion = response.GetString(1)
                                };
                                vistas.Add(vista);
                            }
                            Conexion.Close();
                            return vistas;
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
    }
}
