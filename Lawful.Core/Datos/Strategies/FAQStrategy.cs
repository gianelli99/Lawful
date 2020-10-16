using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Strategies
{
    public class FAQStrategy : IQueryStrategy
    {
        public int Tipo { get; set; }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            FAQ faq = (FAQ)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo, descripcion, fecha_creacion, icon_name, everyone_can_edit, usuario_id, iniciativa_tipo_id, respuesta_correcta_id, tema_id)" +
                " VALUES " +
                "(@titulo, @descripcion, @fecha_creacion, @icon_name, @everyone_can_edit, @usuario_id, @iniciativa_tipo_id, @respuesta_correcta_id, @tema_id);";

            command.Parameters.AddWithValue("@titulo", faq.Titulo);
            command.Parameters.AddWithValue("@descripcion", faq.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", faq.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", faq.IconName);
            command.Parameters.AddWithValue("@everyone_can_edit", faq.EveryoneCanEdit);
            command.Parameters.AddWithValue("@usuario_id", faq.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@respuesta_correcta_id", faq.RespuestaCorrecta.ID);
            command.Parameters.AddWithValue("@tema_id", faq.Tema.ID);


            return command;
        }

        public SqlCommand SetUpdateCommand(SqlCommand command, Iniciativa iniciativa)
        {
            FAQ faq = (FAQ)iniciativa;
            command.CommandText = "UPDATE iniciativas " +
                "titulo=@titulo, " +
                "descripcion=@descripcion, " +
                "fecha_creacion=@fecha_creacion, " +
                "icon_name=@icon_name, " +
                "everyone_can_edit=@everyone_can_edit, " +
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "respuesta_correcta_id=@respuesta_correcta_id, " +
                "tema_id=@tema_id " +
                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", faq.Titulo);
            command.Parameters.AddWithValue("@descripcion", faq.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", faq.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", faq.IconName);
            command.Parameters.AddWithValue("@everyone_can_edit", faq.EveryoneCanEdit);
            command.Parameters.AddWithValue("@usuario_id", faq.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@respuesta_correcta_id", faq.RespuestaCorrecta.ID);
            command.Parameters.AddWithValue("@tema_id", faq.Tema.ID);

            return command;
        }
    }
}
