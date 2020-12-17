using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.DAO
{
    public class SesionDAO_SqlServer : ConexionDB, Interfaces.ISesionDAO
    {
        public int IniciarSesion(SesionActiva sesion)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Iniciar sesión");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"INSERT INTO sesiones (usuario_id,inicio) VALUES({sesion.Usuario.ID},@inicio);SELECT CAST(scope_identity() AS int)";
                    command.Parameters.AddWithValue("@inicio", sesion.LogIn);
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            return response.GetInt32(0);
                        }
                        return -1;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                throw new Exception("Ha ocurrido un error");
            }
        }

        public void CerrarSesion(SesionActiva sesion)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Cerrar sesión");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"UPDATE sesiones SET cierre=@cierre WHERE id = {sesion.ID}";
                    command.Parameters.AddWithValue("@cierre", sesion.LogOut);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int ValidarUsuario(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Validar Usuario");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT id FROM usuarios WHERE username = @username AND contrasena = @password AND estado = 1";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        int usuarioId = -1;
                        if (response.Read())
                        {
                            usuarioId = response.GetInt32(0);
                        }
                        return usuarioId;
                    }
                    throw new Exception("No se ha podido encontrar el usuario");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<SesionInforme> ObtenerMinutosSesionUsuario(int userID)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("obtener minutos");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText =  "SELECT usuario_id,dateadd(DAY,0, datediff(day,0, inicio)) as dia, SUM(datediff(minute, inicio, cierre)) as minutos FROM sesiones" +
                                          $" WHERE usuario_id = {userID}" +
                                           " GROUP BY dateadd(DAY, 0, datediff(day, 0, inicio)), usuario_id" + 
                                           " HAVING dateadd(DAY, 0, datediff(day, 0, inicio)) >= DATEADD(DAY, -10, GETDATE())"+
                                           " order by dateadd(DAY, 0, datediff(day, 0, inicio))";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        List<SesionInforme> sesiones = new List<SesionInforme>();
                        while (response.Read())
                        {
                            var sesion = new SesionInforme();
                            sesion.Usuario = new Usuario() { ID = response.GetInt32(0) };
                            sesion.Fecha = response.GetDateTime(1);
                            sesion.MinutosActivos = response.IsDBNull(2) ? 0 : response.GetInt32(2);
                            sesiones.Add(sesion);
                        }
                        return sesiones;
                    }
                    throw new Exception("No se ha podido encontrar resultados");
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<GrupoInforme> ObtenerMinutosSesionGrupos()
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("obtener minutos");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "SELECT grupos.descripcion as nombre,SUM(datediff(minute, inicio, cierre)) as tiempo, count(distinct sesiones.usuario_id) as usuarios FROM grupos"
                                            + " INNER JOIN usuarios_grupos on usuarios_grupos.grupo_id = grupos.id"
                                            + " INNER JOIN sesiones on sesiones.usuario_id = usuarios_grupos.usuario_id"
                                            + " WHERE dateadd(DAY, 0, datediff(day, 0, inicio)) >= DATEADD(DAY, -10, GETDATE())"
                                            + " GROUP BY grupos.descripcion";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        List<GrupoInforme> grupos = new List<GrupoInforme>();
                        while (response.Read())
                        {
                            var grupo = new GrupoInforme();
                            grupo.Grupo = new Grupo() { Descripcion = response.GetString(0) };
                            grupo.MinutosTotales = response.IsDBNull(1) ? 0 : response.GetInt32(1);
                            grupo.CantUsers = response.GetInt32(2);
                            grupo.Dias = 10;
                            grupos.Add(grupo);
                        }
                        return grupos;
                    }
                    throw new Exception("No se ha podido encontrar resultados");
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
