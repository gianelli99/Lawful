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
        public List<SesionInforme> Listar(DateTime fechaDesde, DateTime fechaHasta)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar sesiones");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT sesiones.id AS id,usuarios.username,inicio,cierre FROM sesiones INNER JOIN usuarios ON usuarios.id = sesiones.usuario_id WHERE inicio > '{fechaDesde.ToString("yyyy-MM-dd HH:mm:ss")}' AND cierre < '{fechaHasta.ToString("yyyy-MM-dd HH:mm:ss")}'";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        var sesiones = new List<Modelo.SesionInforme>();
                        if (response.HasRows)
                        {
                            
                            while (response.Read())
                            {
                                var sesion = new Modelo.SesionInforme();
                                var usuario = new Modelo.Usuario();
                                sesion.Usuario = usuario;
                                sesion.ID = response.GetInt32(0);
                                usuario.Username = response.GetString(1);
                                sesion.LogIn = response.GetDateTime(2);
                                sesion.LogOut = response.GetDateTime(3);
                                sesiones.Add(sesion);
                            }
                        }
                        return sesiones;
                    }
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }
        public List<SesionInforme> ListarPorGrupo(int idGrupo, DateTime fechaDesde, DateTime fechaHasta)// devuelve sesiones de 1 grupo, cada sesion tiene un usuario con solo el username, el usuario de la primer sesion contiene 1 grupo con solo la descripcion del mismo
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar sesiones por grupo");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT sesiones.id AS id,usuarios.username,inicio,cierre FROM sesiones INNER JOIN usuarios ON usuarios.id = sesiones.usuario_id WHERE usuarios.id IN (SELECT usuario_id from usuarios_grupos WHERE grupo_id = {idGrupo}) AND inicio > '{fechaDesde.ToString("yyyy-MM-dd HH:mm:ss")}' AND cierre < '{fechaHasta.ToString("yyyy-MM-dd HH:mm:ss")}';SELECT TOP 1 descripcion from grupos WHERE id = {idGrupo}";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        var sesiones = new List<Modelo.SesionInforme>();
                        if (response.HasRows)
                        {
                           
                            while (response.Read())
                            {
                                var sesion = new Modelo.SesionInforme();
                                var usuario = new Modelo.Usuario();
                                sesion.Usuario = usuario;
                                sesion.ID = response.GetInt32(0);
                                usuario.Username = response.GetString(1);
                                sesion.LogIn = response.GetDateTime(2);
                                sesion.LogOut = response.GetDateTime(3);
                                sesiones.Add(sesion);
                            }
                        }
                        response.NextResult();
                        if (response.HasRows)
                        {
                            response.Read();
                            var grupo = new Modelo.Grupo() { Descripcion = response.GetString(0) };
                            if (sesiones.Count>0)
                            {
                                sesiones[0].Usuario.Grupos.Add(grupo);
                            }
                        }
                        return sesiones;
                    }
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public List<SesionInforme> ListarPorUsuario(int idUsuario, DateTime fechaDesde, DateTime fechaHasta)// en la primer sesion se encuentra 1 usuario con solo su Username
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar sesiones por usuarios");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT sesiones.id AS id,username,inicio,cierre FROM sesiones INNER JOIN usuarios ON usuarios.id = sesiones.usuario_id WHERE usuarios.id = {idUsuario} AND inicio > '{fechaDesde.ToString("yyyy-MM-dd HH:mm:ss")}' AND cierre < '{fechaHasta.ToString("yyyy-MM-dd HH:mm:ss")}';SELECT username FROM usuarios WHERE id = {idUsuario}";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        var sesiones = new List<Modelo.SesionInforme>();
                        if (response.HasRows)
                        {

                            while (response.Read())
                            {
                                var sesion = new Modelo.SesionInforme();
                                var usuario = new Modelo.Usuario();
                                sesion.Usuario = usuario;
                                sesion.ID = response.GetInt32(0);
                                usuario.Username = response.GetString(1);
                                sesion.LogIn = response.GetDateTime(2);
                                sesion.LogOut = response.GetDateTime(3);
                                sesiones.Add(sesion);
                            }
                            //return sesiones;
                        }
                        response.NextResult();
                        if (response.HasRows)
                        {
                            response.Read();
                            var usuario = new Modelo.Usuario() { Username = response.GetString(0) };

                            if (sesiones!= null && sesiones.Count>0)
                            {
                                sesiones[0].Usuario = usuario;
                            }
                        }
                        return sesiones;
                    }
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
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
    }
}
