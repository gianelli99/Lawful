using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using Microsoft.Data.SqlClient;
using System;
namespace Lawful.Core.Datos.Strategies
{
    public class DoDontStrategy : IQueryStrategy
    {
        public int Tipo { get; set; }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            DoDont dodont = (DoDont)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo," +
                " descripcion," +
                " fecha_creacion," +
                " icon_name," +
                " usuario_id," +
                " iniciativa_tipo_id," +
                " tema_id," +
                " fecha_cierre);" +

                " VALUES " +
                "(@titulo," +
                " @descripcion," +
                " @fecha_creacion," +
                " @icon_name," +
                " @usuario_id," +
                " @iniciativa_tipo_id," +
                " @tema_id," +
                " @fecha_cierre);";

            command.Parameters.AddWithValue("@titulo", dodont.Titulo);
            command.Parameters.AddWithValue("@descripcion", dodont.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", dodont.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", dodont.IconName);
            command.Parameters.AddWithValue("@usuario_id", dodont.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@tema_id", dodont.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", dodont.FechaCierre); 


            return command;
        }

        public SqlCommand SetUpdateCommand(SqlCommand command, Iniciativa iniciativa)
        {
            DoDont dodont = (DoDont)iniciativa;
            command.CommandText = "UPDATE iniciativas " +
                "titulo=@titulo, " +
                "descripcion=@descripcion, " +
                "fecha_creacion=@fecha_creacion, " +
                "icon_name=@icon_name, " +
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "tema_id=@tema_id, " +
                "fecha_cierre=@fecha_cierre " +
                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", dodont.Titulo);
            command.Parameters.AddWithValue("@descripcion", dodont.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", dodont.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", dodont.IconName);
            command.Parameters.AddWithValue("@usuario_id", dodont.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@tema_id", dodont.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", dodont.FechaCierre);

            return command;
        }
    }
}
