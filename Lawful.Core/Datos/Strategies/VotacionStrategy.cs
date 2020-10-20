using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.Strategies
{
    public class VotacionStrategy : IQueryStrategy
    {
        public int Tipo { get; set; }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            Votacion votacion = (Votacion)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo," +
                " descripcion," +
                " fecha_creacion," +
                " icon_name," +
                " usuario_id," +
                " iniciativa_tipo_id," +
                " fecha_limite," +
                " tema_id," +
                " fecha_cierre)" +

                " VALUES " +
                "(@titulo," +
                " @descripcion," +
                " @fecha_creacion," +
                " @icon_name," +
                " @usuario_id," +
                " @iniciativa_tipo_id," +
                " @fecha_limite, " +
                " @tema_id," +
                " @fecha_cierre);";

            command.Parameters.AddWithValue("@titulo", votacion.Titulo);
            command.Parameters.AddWithValue("@descripcion", votacion.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", votacion.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", votacion.IconName);
            command.Parameters.AddWithValue("@usuario_id", votacion.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@fecha_limite", votacion.FechaLimite);
            command.Parameters.AddWithValue("@tema_id", votacion.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", votacion.FechaCierre);


            return command;
        }

        public SqlCommand SetUpdateCommand(SqlCommand command, Iniciativa iniciativa)
        {
            Votacion votacion = (Votacion)iniciativa;
            command.CommandText = "UPDATE iniciativas " +
                "titulo=@titulo, " +
                "descripcion=@descripcion, " +
                "fecha_creacion=@fecha_creacion, " +
                "icon_name=@icon_name, " +
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "fecha_limite=@fecha_limite," +
                "tema_id=@tema_id," +
                "icon_name=@icon_name, " +
                "fecha_cierre=@fecha_cierre " +
                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", votacion.Titulo);
            command.Parameters.AddWithValue("@descripcion", votacion.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", votacion.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", votacion.IconName);
            command.Parameters.AddWithValue("@usuario_id", votacion.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@fecha_limite", votacion.FechaLimite);
            command.Parameters.AddWithValue("@tema_id", votacion.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", votacion.FechaCierre);

            return command;
        }
    }
}
