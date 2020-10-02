using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.DAO
{
    public class GrupoDAO_SqlServer : ConexionDB, Interfaces.IGrupoDAO
    {
        public int CantidadUsuarios(int id)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Cantidad Usuarios en Grupo");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT COUNT(usuario_id) AS cantidad FROM usuarios_grupos INNER JOIN usuarios ON usuario_id = usuarios.id WHERE grupo_id = {id} AND usuarios.estado = 1";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            int cant = response.GetInt32(0);
                            Conexion.Close();
                            return cant;
                        }
                        else
                        {
                            throw new Exception("Ha ocurrido un error, contacte al administrador para más información");
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

        public Grupo Consultar(int id)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Consulta Grupo");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT * FROM grupos WHERE id = {id}";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            var grupo = new Modelo.Grupo();

                            grupo.ID = response.GetInt32(0);
                            grupo.Codigo = response.GetString(1);
                            grupo.Descripcion = response.GetString(2);
                            grupo.Estado = response.GetBoolean(3);
                            return grupo;
                        }
                    }
                    throw new Exception("No se ha podido encontrar el grupo");
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public bool DescripcionCodigoDisponible(string descripción, string codigo, string id)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Validacion descripción y código");

                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    string optionalQuery = id != null ? $" AND id <> {id}" : "";
                    command.CommandText = $"SELECT count(id) AS cantidad FROM grupos WHERE (descripcion = @descripcion OR codigo = @codigo)" + optionalQuery;
                    command.Parameters.AddWithValue("@descripcion", descripción);
                    command.Parameters.AddWithValue("@codigo", codigo);
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            return response.GetInt32(0) > 0 ? false : true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
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
                transaction = connection.BeginTransaction("Eliminar grupo");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"UPDATE grupos set estado=0 WHERE id = {id}";
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

        public void Insertar(Grupo grupo)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Insertar grupo");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"INSERT INTO grupos VALUES(@codigo, @descripcion, 1);";
                    command.Parameters.AddWithValue("@codigo", grupo.Codigo);
                    command.Parameters.AddWithValue("@descripcion", grupo.Descripcion);


                    string accionesQuery = "INSERT INTO grupos_acciones VALUES";
                    foreach (var accion in grupo.Acciones)
                    {
                        accionesQuery += $"({grupo.ID}, {accion.ID}),";
                    }
                    accionesQuery.TrimEnd(new char[] {','});
                    accionesQuery += ";";
                    command.CommandText += accionesQuery;

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

        public List<Grupo> Listar()
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar grupos");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT * FROM grupos WHERE estado = 1";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var grupos = new List<Modelo.Grupo>();
                            while (response.Read())
                            {
                                var grupo = new Modelo.Grupo();

                                grupo.ID = response.GetInt32(0);
                                grupo.Codigo = response.GetString(1);
                                grupo.Descripcion = response.GetString(2);
                                grupo.Estado = response.GetBoolean(3);
                                grupos.Add(grupo);
                            }
                            return grupos;
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

        public List<Grupo> ListarPorUsuario(int userId) 
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Listar grupos por usuario");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT grupos.id, grupos.codigo, grupos.descripcion, grupos.estado FROM grupos INNER JOIN usuarios_grupos ON usuarios_grupos.grupo_id = grupos.id WHERE estado = 1 AND usuarios_grupos.usuario_id = {userId.ToString()}";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var grupos = new List<Grupo>();
                            while (response.Read())
                            {
                                var grupo = new Grupo();

                                grupo.ID = response.GetInt32(0);
                                grupo.Codigo = response.GetString(1);
                                grupo.Descripcion = response.GetString(2);
                                grupo.Estado = response.GetBoolean(3);
                                grupos.Add(grupo);
                            }
                            return grupos;
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

        public void Modificar(Grupo grupo)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Modificar grupo");

                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    int bitEstado = grupo.Estado ? 1 : 0;

                    command.CommandText = $"UPDATE grupos SET codigo=@codigo, descripcion=@descripcion, estado={bitEstado.ToString()} WHERE id = {grupo.ID};";
                    command.Parameters.AddWithValue("@codigo", grupo.Codigo);
                    command.Parameters.AddWithValue("@descripcion", grupo.Descripcion);

                    command.CommandText += $"DELETE * FROM grupos_acciones WHERE grupo_id = {grupo.ID};";
                    string accionesQuery = "";
                    foreach (var accion in grupo.Acciones)
                    {
                        accionesQuery += $"INSERT INTO grupos_acciones VALUES({grupo.ID},{accion.ID});";
                    }
                    command.CommandText += accionesQuery;
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
