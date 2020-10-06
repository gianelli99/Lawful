using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.QueryMiddleware
{
    class AsistireMiddleware : IniciativaMiddleware
    { 
        public override SqlCommand SetInsertCommand(SqlCommand command)
        {
            Asistire asistire = (Asistire)Iniciativa;

            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo, descripcion, fecha_creacion, iniciativa_icono_id, everyone_can_edit, usuario_id, iniciativa_tipo_id, fecha_evento, lugar, fecha_limite_confirmacion, tema_id)" +
                " VALUES " +
                "(@titulo, @descripcion, @fecha_creacion, @iniciativa_icono_id, @everyone_can_edit, @usuario_id, @iniciativa_tipo_id, @fecha_evento, @lugar, @fecha_limite_confirmacion, @tema_id);";

            command.Parameters.AddWithValue("@titulo", asistire.Titulo);
            command.Parameters.AddWithValue("@descripcion", asistire.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", asistire.FechaCreacion);
            command.Parameters.AddWithValue("@iniciativa_icono_id", asistire.IconName);
            command.Parameters.AddWithValue("@everyone_can_edit", asistire.EveryoneCanEdit);
            command.Parameters.AddWithValue("@usuario_id", asistire.Owner.ID);
            
            command.Parameters.AddWithValue("@fecha_evento", asistire.FechaEvento);
            command.Parameters.AddWithValue("@lugar", asistire.Lugar);
            command.Parameters.AddWithValue("@fecha_limite_confirmacion", asistire.FechaLimiteConfirmacion);
            command.Parameters.AddWithValue("@tema_id", asistire.Tema.ID);


            return command;
        }
    }
}
