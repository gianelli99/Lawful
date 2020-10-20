using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Lawful.Core.Modelo.Iniciativas;
using Lawful.Core.Datos.QueryMiddleware;
using System.Diagnostics;

namespace Lawful.Core.Datos.DAO
{
    public class IniciativaDAO_SqlServer : ConexionDB, Interfaces.IIniciativaDAO
    {
        public IQueryStrategy Strategy { get; set; }
       
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
                    command.CommandText = $"SELECT iniciativas.id,titulo,descripcion,fecha_creacion,icon_name,usuario_id,iniciativa_tipo_id,fecha_evento,lugar,fecha_limite_confirmacion,respuesta_correcta_id,relevancia,fecha_limite,max_opciones_seleccionables,tema_id,is_open FROM iniciativas WHERE id = {id};";

                    command.CommandText = $"SELECT iniciativas.id," +
                        $" titulo," +
                        $" descripcion," +
                        $" fecha_creacion," +
                        $" icon_name," +
                        $" usuario_id," +
                        $" iniciativa_tipo_id," +
                        $" fecha_evento," +
                        $" lugar," +
                        $" fecha_limite_confirmacion," +
                        $" respuesta_correcta_id," +
                        $" relevancia," +
                        $" fecha_limite," +
                        $" max_opciones_seleccionables," +
                        $" tema_id," +
                        $" fecha_cierre " +
                        $"FROM iniciativas WHERE id = {id};";

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
                                opciones.Add(opt);
                            }
                            response.NextResult();
                            while (response.Read())
                            {
                                Usuario votante = new Usuario();
                                votante.Nombre = response.GetString(1);
                                votante.Apellido = response.GetString(2);
                                int opcionId = response.GetInt32(0);
                                foreach (var opcion in opciones)
                                {
                                    if (opcionId == opcion.ID)
                                    {
                                        opcion.Votantes.Add(votante);
                                        break;
                                    }
                                }
                            }

                            if (iniciativa.GetType() != typeof(PropuestaGenerica) || iniciativa.GetType() != typeof(FAQ))
                            {
                                switch (iniciativa.GetType().Name.ToString())
                                {
                                    case "Asistire":
                                        ((Asistire)iniciativa).Opciones = opciones;
                                        break;
                                    case "DoDont":
                                        ((DoDont)iniciativa).Opciones = opciones;
                                        break;
                                    case "Regla":
                                        ((Regla)iniciativa).Opciones = opciones;
                                        break;
                                    case "Votacion":
                                        ((Votacion)iniciativa).Opciones = opciones;
                                        break;
                                    case "VotacionMultiple":
                                        ((VotacionMultiple)iniciativa).Opciones = opciones;
                                        break;
                                    default:
                                        break;
                                }
                            }

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
        }//done

        public void Eliminar(int id)//done
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
                    command.CommandText = $"DELETE FROM comentarios WHERE iniciativa_id = {id}; DELETE FROM votos WHERE opcion_id IN(SELECT id FROM opciones WHERE opciones.iniciativa_id = {id}); DELETE FROM opciones WHERE opciones.iniciativa_id = {id}; DELETE FROM iniciativas WHERE id = {id}";
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
                transaction = connection.BeginTransaction("Insertar Iniciativa");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    
                    Strategy.SetInsertCommand(command, iniciativa);
                    
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
        }//done

        public List<Iniciativa> ListarPorTema(int temaId)//done
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
                    command.CommandText = $"SELECT iniciativas.id,titulo,descripcion,fecha_creacion,icon_name,usuario_id,iniciativa_tipo_id,fecha_evento,lugar,fecha_limite_confirmacion,respuesta_correcta_id,relevancia,fecha_limite,max_opciones_seleccionables,tema_id,is_open FROM iniciativas WHERE tema_id = {temaId};";
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

        public List<Iniciativa> ListarPorUsuario(int userId)//done
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
                    command.CommandText = $"SELECT iniciativas.id,titulo,descripcion,fecha_creacion,icon_name,iniciativas.usuario_id,iniciativa_tipo_id,fecha_evento,lugar,fecha_limite_confirmacion,respuesta_correcta_id,relevancia,fecha_limite,max_opciones_seleccionables,iniciativas.tema_id,iniciativas.is_open FROM iniciativas INNER JOIN usuarios_temas ON usuarios_temas.tema_id = iniciativas.tema_id WHERE usuarios_temas.usuario_id = {userId};";
                    command.CommandText = $"SELECT iniciativas.id,titulo,descripcion,fecha_creacion,icon_name,iniciativas.usuario_id,iniciativa_tipo_id,fecha_evento,lugar,fecha_limite_confirmacion,respuesta_correcta_id,relevancia,fecha_limite,max_opciones_seleccionables,iniciativas.tema_id,fecha_cierre FROM iniciativas INNER JOIN usuarios_temas ON usuarios_temas.tema_id = iniciativas.tema_id WHERE usuarios_temas.usuario_id = {userId};";
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
            using (SqlConnection connection = new SqlConnection(Conexion.ConnectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction("Modificar Iniciativa");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {

                    Strategy.SetUpdateCommand(command, iniciativa);

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

        public void SetStrategy(IQueryStrategy strategy)
        {
            this.Strategy = strategy;
        }

        private string[] ListarCamposTablaIniciativas(SqlDataReader response)
        {
            string[] campos = new string[16] {
                            response.IsDBNull(0) ? "0" :response.GetInt32(0).ToString(),
                            response.IsDBNull(1) ? "" :response.GetString(1).ToString(),
                            response.IsDBNull(2) ? "" :response.GetString(2).ToString(),
                            response.IsDBNull(3) ? "" :response.GetDateTime(3).ToString(),
                            response.IsDBNull(4) ? "" :response.GetString(4).ToString(),
                            response.IsDBNull(5) ? "" :response.GetInt32(5).ToString(),
                            response.IsDBNull(6) ? "" :response.GetInt32(6).ToString(),
                            response.IsDBNull(7) ? "" :response.GetDateTime(7).ToString(),
                            response.IsDBNull(8) ? "" :response.GetString(8).ToString(),
                            response.IsDBNull(9)? "" :response.GetDateTime(9).ToString(),
                            response.IsDBNull(10)? "" :response.GetInt32(10).ToString(),
                            response.IsDBNull(11)? "" :response.GetInt32(11).ToString(),
                            response.IsDBNull(12)? "" :response.GetDateTime(12).ToString(),
                            response.IsDBNull(13)? "" :response.GetInt32(13).ToString(),
                            response.IsDBNull(14)? "" :response.GetInt32(14).ToString(),
                            response.IsDBNull(15)? "" :response.GetDateTime(15).ToString()};
            return campos;
        }
    }
}
