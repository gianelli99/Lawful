using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Lawful.Core.Modelo.Iniciativas;
using Microsoft.Office.Interop.Excel;

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
                    transaction.Commit();
                    using (SqlDataReader response = command.ExecuteReader())
                    {
                        if (response.Read())
                        {
                            string[] campos = ListarCamposTablaIniciativas(response);
                            Iniciativa iniciativa = Helpers.IniciativaEspecificaCreator.CrearIniciativaEspecifica(campos);
                            return iniciativa;
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
            throw new NotImplementedException();
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
                    throw new Exception("No se ha podido encontrar el tema");
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
