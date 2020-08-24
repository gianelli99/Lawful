using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lawful.Core.Modelo;
using System.Data.SqlClient;

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
                    //command.CommandText = $"DELETE FROM permisos WHERE grupo_id = {id};DELETE FROM grupos WHERE id = {id}";
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

        public void Insertar(Grupo t, List<Modelo.Permiso> permisos)
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
                    int bitEstado = t.Estado ? 1 : 0;

                    command.CommandText = $"INSERT INTO grupos VALUES(@codigo,@descripcion,{bitEstado.ToString()});SELECT CAST(scope_identity() AS int)";
                    command.Parameters.AddWithValue("@codigo", t.Codigo);
                    command.Parameters.AddWithValue("@descripcion", t.Descripcion);
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            t.ID = response.GetInt32(0);
                        }
                    }
                    string querypermisos = $"INSERT INTO permisos VALUES ";
                    foreach (var permiso in permisos)
                    {
                        int tienePermiso = permiso.TienePermiso ? 1 : 0;
                        querypermisos += $"('{t.ID.ToString()}','{permiso.Vista.ID.ToString()}','{permiso.Accion.ID.ToString()}','{tienePermiso.ToString()}'),";
                    }
                    querypermisos = querypermisos.TrimEnd(',');
                    command.CommandText = querypermisos;
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

        public List<int[]> ListarIDPermisos(int id)// mod y consulta
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
                    command.CommandText = $"SELECT id, vista_id,accion_id,tiene_permiso FROM permisos WHERE grupo_id = {id}";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.HasRows)
                        {
                            var permisos = new List<int[]>();
                            while (response.Read())
                            {
                                var permiso = new int[4];
                                permiso[0] = response.GetInt32(0);//id
                                permiso[1] = response.GetInt32(1);//vista
                                permiso[2] = response.GetInt32(2);//accion
                                permiso[3] = Convert.ToInt32(response.GetBoolean(3));//permiso
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

        public void Modificar(Grupo t, List<Modelo.Permiso> permisos)
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
                    int bitEstado = t.Estado ? 1 : 0;

                    command.CommandText = $"UPDATE grupos SET codigo=@codigo, descripcion=@descripcion, estado={bitEstado.ToString()} WHERE id = {t.ID}";
                    command.Parameters.AddWithValue("@codigo", t.Codigo);
                    command.Parameters.AddWithValue("@descripcion", t.Descripcion);
                    command.ExecuteNonQuery();

                    string querypermisos = "";
                    foreach (var permiso in permisos)
                    {
                        int tienePermiso = permiso.TienePermiso ? 1 : 0;
                        querypermisos += $"UPDATE permisos SET tiene_permiso = '{tienePermiso.ToString()}' WHERE id = '{permiso.ID.ToString()}';";
                    }

                    command.CommandText = querypermisos;
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
