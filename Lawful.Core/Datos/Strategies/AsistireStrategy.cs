using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.QueryMiddleware
{
    class AsistireStrategy : IQueryStrategy
    {
        public int Tipo { get; set; }
        public string IconName { get; set; }
        public AsistireStrategy()
        {
            IconName = "Calendar";
            Tipo = 1;
        }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            Asistire asistire = (Asistire)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo," +
                " descripcion," +
                " fecha_creacion," +
                " icon_name," +
                " usuario_id," +
                " iniciativa_tipo_id," +
                " fecha_evento," +
                " lugar," +
                " fecha_limite_confirmacion," +
                " tema_id, " +
                " fecha_cierre)" +
                " VALUES " +
                "(@titulo," +
                " @descripcion," +
                " @fecha_creacion," +
                " @icon_name," +
                " @usuario_id," +
                " @iniciativa_tipo_id," +
                " @fecha_evento," +
                " @lugar," +
                " @fecha_limite_confirmacion," +
                " @tema_id," +
                " @fecha_cierre);";

            command.Parameters.AddWithValue("@titulo", asistire.Titulo);
            command.Parameters.AddWithValue("@descripcion", asistire.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", asistire.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", IconName);
            command.Parameters.AddWithValue("@usuario_id", asistire.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);

            command.Parameters.AddWithValue("@fecha_evento", asistire.FechaEvento);
            command.Parameters.AddWithValue("@lugar", asistire.Lugar);
            command.Parameters.AddWithValue("@fecha_limite_confirmacion", asistire.FechaLimiteConfirmacion);
            command.Parameters.AddWithValue("@tema_id", asistire.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", asistire.FechaCierre);


            return command;
        }

        public SqlCommand SetUpdateCommand(SqlCommand command, Iniciativa iniciativa)
        {
            Asistire asistire = (Asistire)iniciativa;
            command.CommandText = "UPDATE iniciativas " +
                "titulo=@titulo, " +
                "descripcion=@descripcion, " +
                "fecha_creacion=@fecha_creacion, " +
                "icon_name=@icon_name, " +
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "fecha_evento=@fecha_evento, " +
                "lugar=@lugar, " +
                "fecha_limite_confirmacion=@fecha_limite_confirmacion, " +
                "tema_id=@tema_id, " +
                "icon_name=@icon_name, " +
                "fecha_cierre=@fecha_cierre " +
                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", asistire.Titulo);
            command.Parameters.AddWithValue("@descripcion", asistire.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", asistire.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", asistire.IconName);
            command.Parameters.AddWithValue("@usuario_id", asistire.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);

            command.Parameters.AddWithValue("@fecha_evento", asistire.FechaEvento);
            command.Parameters.AddWithValue("@lugar", asistire.Lugar);
            command.Parameters.AddWithValue("@fecha_limite_confirmacion", asistire.FechaLimiteConfirmacion);
            command.Parameters.AddWithValue("@tema_id", asistire.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", asistire.FechaCierre);


            return command;
        }
    }
}
