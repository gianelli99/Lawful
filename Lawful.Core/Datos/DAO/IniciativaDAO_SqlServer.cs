using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Lawful.Core.Modelo.Iniciativas;
using Microsoft.Office.Interop.Excel;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Lawful.Core.Datos.DAO
{
    public class IniciativaDAO_SqlServer : ConexionDB, Interfaces.IIniciativaDAO
    {
        public Iniciativa Consultar(int id)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Consulta Iniciativa");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT iniciativas.id,titulo,descripcion,fecha_creacion,iniciativas_iconos.icon_name,everyone_can_edit,usuario_id,iniciativa_tipo_id,fecha_evento,lugar,fecha_limite_confirmacion,respuesta_correcta_id,relevancia,fecha_limite,max_opciones_seleccionables,tema_id FROM iniciativas INNER JOIN iniciativas_iconos ON iniciativa_icono_id = iniciativas_iconos.id WHERE id = {id};";
                    command.CommandText += $"SELECT comentarios.id, descripcion, usuario_id,usuarios.nombre, usuarios.apellido FROM comentarios INNER JOIN usuarios ON comentarios.usuario_id = usuarios.id WHERE comentarios.iniciativa_id = {id};";
                    command.CommandText += $"SELECT opciones.id, opciones.descripcion FROM opciones WHERE iniciativa_id = {id};";
                    command.CommandText += $"SELECT opciones.id,usuarios.nombre, usuarios.apellido FROM opciones INNER JOIN votos ON opciones.id =votos.opcion_id INNER JOIN usuarios ON usuarios.id = votos.usuario_id WHERE iniciativa_id = {id};";
                    
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        Iniciativa iniciativa;
                        
                        if (response.Read())
                        {
                            string[] campos = ListarCamposTablaIniciativas(response);
                            iniciativa = Helpers.IniciativaEspecificaCreator.CrearIniciativaEspecifica(campos);

                            List<Comentario> comentarios = new List<Comentario>();
                            response.NextResult();
                            while (response.Read())
                            {
                                Comentario comment = new Comentario();
                                comment.ID = response.GetInt32(0);
                                comment.Descripcion = response.GetString(1);
                                Usuario owner = new Usuario();
                                owner.ID = response.GetInt32(2); //traigo el id por si en el futuro quiero editar mis comentarios
                                owner.Nombre = response.GetString(3);
                                owner.Apellido = response.GetString(4);
                                comment.Owner = owner;
                                comentarios.Add(comment);
                            }
                            iniciativa.Comentarios = comentarios;

                            List<Opcion> opciones = new List<Opcion>();
                            response.NextResult();
                            while (response.Read())
                            {
                                Opcion opt = new Opcion();
                                opt.ID = response.GetInt32(0);
                                opt.Descripcion = response.GetString(1);
                            }
                            if (iniciativa.GetType() != typeof(PropuestaGenerica) || iniciativa.GetType() != typeof(FAQ))
                            {
                                switch (iniciativa.GetType().Name.ToString())
                                {
                                    case "Asistire":
                                        ((Asistire)iniciativa).Opciones = opciones;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            

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
                transaction = connection.BeginTransaction("Eliminar iniciativa");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"DELETE FROM comentarios WHERE iniciativa_id = {id}; DELETE FROM votos WHERE opcion_id IN(SELECT id FROM opciones WHERE opciones.iniciativa_id = {id}); DELETE FROM opciones WHERE opciones.iniciativa_id = {id};";
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

        public void Insertar(Iniciativa iniciativa)
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

        public List<Iniciativa> ListarPorTema(int temaId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("listar Iniciativas");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT iniciativas.id,titulo,descripcion,fecha_creacion,iniciativas_iconos.icon_name,everyone_can_edit,usuario_id,iniciativa_tipo_id,fecha_evento,lugar,fecha_limite_confirmacion,respuesta_correcta_id,relevancia,fecha_limite,max_opciones_seleccionables,tema_id FROM iniciativas INNER JOIN iniciativas_iconos ON iniciativa_icono_id = iniciativas_iconos.id WHERE tema_id = {temaId};";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        List<Iniciativa> iniciativas = new List<Iniciativa>();
                        while (response.Read())
                        {
                            string[] campos = ListarCamposTablaIniciativas(response);
                            Iniciativa iniciativa = Helpers.IniciativaEspecificaCreator.CrearIniciativaEspecifica(campos);
                            iniciativas.Add(iniciativa);
                        }
                        return iniciativas;
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

        public List<Iniciativa> ListarPorUsuario(int userId)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("listar Iniciativas");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = $"SELECT iniciativas.id,titulo,descripcion,fecha_creacion,iniciativas_iconos.icon_name,everyone_can_edit,usuario_id,iniciativa_tipo_id,fecha_evento,lugar,fecha_limite_confirmacion,respuesta_correcta_id,relevancia,fecha_limite,max_opciones_seleccionables,tema_id FROM iniciativas INNER JOIN iniciativas_iconos ON iniciativa_icono_id = iniciativas_iconos.id WHERE tema_id = {userId};";
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        List<Iniciativa> iniciativas = new List<Iniciativa>();
                        while (response.Read())
                        {
                            string[] campos = ListarCamposTablaIniciativas(response);
                            Iniciativa iniciativa = Helpers.IniciativaEspecificaCreator.CrearIniciativaEspecifica(campos);
                            iniciativas.Add(iniciativa);
                        }
                        return iniciativas;
                    }
                    throw new Exception("No se ha podido encontrar la iniciativa");
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }
            }
            throw new Exception("Ha ocurrido un error");
        }

        public void Modificar(Iniciativa iniciativa)
        {
            throw new NotImplementedException();
        }

        private string[] ListarCamposTablaIniciativas(SqlDataReader response)
        {
            string[] campos = new string[16] {
                            response.GetInt32(0).ToString(),
                            response.GetString(1).ToString(),
                            response.GetString(2).ToString(),
                            response.GetDateTime(3).ToString(),
                            response.GetString(4).ToString(),
                            response.GetBoolean(5).ToString(),
                            response.GetInt32(6).ToString(),
                            response.GetInt32(7).ToString(),
                            response.GetDateTime(8).ToString(),
                            response.GetString(9).ToString(),
                            response.GetDateTime(10).ToString(),
                            response.GetInt32(11).ToString(),
                            response.GetInt32(12).ToString(),
                            response.GetDateTime(13).ToString(),
                            response.GetInt32(14).ToString(),
                            response.GetInt32(15).ToString()};
            return campos;
        }
    }
}
