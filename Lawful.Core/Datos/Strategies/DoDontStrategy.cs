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
                " everyone_can_edit," +
                " usuario_id," +
                " iniciativa_tipo_id," +
                " tema_id)" +

                " VALUES " +
                "(@titulo," +
                " @descripcion," +
                " @fecha_creacion," +
                " @icon_name," +
                " @everyone_can_edit," +
                " @usuario_id," +
                " @iniciativa_tipo_id," +
                " @tema_id);";

            command.Parameters.AddWithValue("@titulo", dodont.Titulo);
            command.Parameters.AddWithValue("@descripcion", dodont.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", dodont.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", dodont.IconName);
            command.Parameters.AddWithValue("@everyone_can_edit", dodont.EveryoneCanEdit);
            command.Parameters.AddWithValue("@usuario_id", dodont.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@tema_id", dodont.Tema.ID);


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
                "everyone_can_edit=@everyone_can_edit, " +
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "tema_id=@tema_id " +
                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", dodont.Titulo);
            command.Parameters.AddWithValue("@descripcion", dodont.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", dodont.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", dodont.IconName);
            command.Parameters.AddWithValue("@everyone_can_edit", dodont.EveryoneCanEdit);
            command.Parameters.AddWithValue("@usuario_id", dodont.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@tema_id", dodont.Tema.ID);

            return command;
        }
    }
}
