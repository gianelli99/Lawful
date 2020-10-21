using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using Microsoft.Data.SqlClient;
using System;

namespace Lawful.Core.Datos.Strategies
{
    public class FAQStrategy : IQueryStrategy
    {
        public int Tipo { get; set; }
        public string IconName { get; set; }
        public FAQStrategy()
        {
            Tipo = 4;
            IconName = "Message";
        }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            FAQ faq = (FAQ)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo," +
                " descripcion," +
                " fecha_creacion," +
                " icon_name," +
                " usuario_id," +
                " iniciativa_tipo_id," +
                " tema_id," +
                " fecha_cierre)" +
                " VALUES " +

                "(@titulo," +
                " @descripcion," +
                " @fecha_creacion," +
                " @icon_name," +
                " @usuario_id," +
                " @iniciativa_tipo_id," +
                " @tema_id," +
                " @fecha_cierre);";

            command.Parameters.AddWithValue("@titulo", faq.Titulo);
            command.Parameters.AddWithValue("@descripcion", faq.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", faq.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", IconName);
            command.Parameters.AddWithValue("@usuario_id", faq.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@tema_id", faq.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", faq.FechaCierre);

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
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "respuesta_correcta_id=@respuesta_correcta_id, " +
                "tema_id=@tema_id, " +
                "fecha_cierre=@fecha_cierre " +
                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", faq.Titulo);
            command.Parameters.AddWithValue("@descripcion", faq.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", faq.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", faq.IconName);
            command.Parameters.AddWithValue("@usuario_id", faq.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@respuesta_correcta_id", faq.RespuestaCorrecta.ID);
            command.Parameters.AddWithValue("@tema_id", faq.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", faq.FechaCierre);


            return command;
        }

        public SqlCommand SetInsertOpciones(SqlCommand command, Iniciativa iniciativa)
        {
            command.CommandText = "";
            return command;
        }
    }
}
