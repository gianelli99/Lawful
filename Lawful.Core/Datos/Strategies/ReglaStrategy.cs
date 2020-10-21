using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using Microsoft.Data.SqlClient;
using System;
namespace Lawful.Core.Datos.Strategies
{
    public class ReglaStrategy : IQueryStrategy
    {
        public int Tipo { get; set; }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            Regla regla = (Regla)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo," +
                " descripcion," +
                " fecha_creacion," +
                " icon_name," +
                " usuario_id," +
                " iniciativa_tipo_id," +
                " relevancia, " +
                " tema_id, " +
                " fecha_cierre)" +

                " VALUES " +
                "(@titulo," +
                " @descripcion," +
                " @fecha_creacion," +
                " @icon_name," +
                " @usuario_id," +
                " @iniciativa_tipo_id," +
                " @relevancia, " +
                " @tema_id, " +
                " @fecha_cierre);";

            command.Parameters.AddWithValue("@titulo", regla.Titulo);
            command.Parameters.AddWithValue("@descripcion", regla.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", regla.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", regla.IconName);
            command.Parameters.AddWithValue("@usuario_id", regla.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@relevancia", regla.Relevancia);
            command.Parameters.AddWithValue("@tema_id", regla.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", regla.FechaCierre);



            return command;
        }

        public SqlCommand SetUpdateCommand(SqlCommand command, Iniciativa iniciativa)
        {
            Regla regla = (Regla)iniciativa;
            command.CommandText = "UPDATE iniciativas " +
                "titulo=@titulo, " +
                "descripcion=@descripcion, " +
                "fecha_creacion=@fecha_creacion, " +
                "icon_name=@icon_name, " +
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "relevancia=@relevancia, " +
                "tema_id=@tema_id, " +
                "fecha_cierre=@fecha_cierre " +

                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", regla.Titulo);
            command.Parameters.AddWithValue("@descripcion", regla.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", regla.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", regla.IconName);
            command.Parameters.AddWithValue("@usuario_id", regla.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@relevancia", regla.Relevancia);
            command.Parameters.AddWithValue("@tema_id", regla.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", regla.FechaCierre);


            return command;
        }

        public SqlCommand SetInsertOpciones(SqlCommand command, Iniciativa iniciativa)
        {
            throw new NotImplementedException();
        }
    }
}
