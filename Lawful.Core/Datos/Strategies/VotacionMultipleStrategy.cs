using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;
using System;
using Lawful.Core.Modelo.Iniciativas;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Strategies
{
    public class VotacionMultipleStrategy : IQueryStrategy
    {
        public int Tipo { get; set; }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            VotacionMultiple votacionMultiple = (VotacionMultiple)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo," +
                " descripcion," +
                " fecha_creacion," +
                " icon_name," +
                " everyone_can_edit," +
                " usuario_id," +
                " iniciativa_tipo_id," +
                " fecha_limite," +
                " max_opciones_seleccionables," +
                " tema_id, " +
                " icon_name)" +

                " VALUES " +
                "(@titulo," +
                " @descripcion," +
                " @fecha_creacion," +
                " @icon_name," +
                " @everyone_can_edit," +
                " @usuario_id," +
                " @iniciativa_tipo_id," +
                " @fecha_limite, " +
                " @max_opciones_seleccionables," +
                " @tema_id," +
                " @icon_name);";

            command.Parameters.AddWithValue("@titulo", votacionMultiple.Titulo);
            command.Parameters.AddWithValue("@descripcion", votacionMultiple.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", votacionMultiple.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", votacionMultiple.IconName);
            command.Parameters.AddWithValue("@everyone_can_edit", votacionMultiple.EveryoneCanEdit);
            command.Parameters.AddWithValue("@usuario_id", votacionMultiple.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@fecha_limite", votacionMultiple.FechaLimite);
            command.Parameters.AddWithValue("@max_opciones_seleccionables", votacionMultiple.MaxOpcionesSeleccionables);
            command.Parameters.AddWithValue("@tema_id", votacionMultiple.Tema.ID);
            command.Parameters.AddWithValue("@icon_name", votacionMultiple.IconName);


            return command;
        }

        public SqlCommand SetUpdateCommand(SqlCommand command, Iniciativa iniciativa)
        {
            VotacionMultiple votacionMultiple = (VotacionMultiple)iniciativa;
            command.CommandText = "UPDATE iniciativas " +
                "titulo=@titulo, " +
                "descripcion=@descripcion, " +
                "fecha_creacion=@fecha_creacion, " +
                "icon_name=@icon_name, " +
                "everyone_can_edit=@everyone_can_edit, " +
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "fecha_limite=@fecha_limite," +
                "max_opciones_seleccionables=@max_opciones_seleccionables," +
                "tema_id=@tema_id, " +
                "icon_name=@icon_name " +
                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", votacionMultiple.Titulo);
            command.Parameters.AddWithValue("@descripcion", votacionMultiple.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", votacionMultiple.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", votacionMultiple.IconName);
            command.Parameters.AddWithValue("@everyone_can_edit", votacionMultiple.EveryoneCanEdit);
            command.Parameters.AddWithValue("@usuario_id", votacionMultiple.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@fecha_limite", votacionMultiple.FechaLimite);
            command.Parameters.AddWithValue("@max_opciones_seleccionables", votacionMultiple.MaxOpcionesSeleccionables);
            command.Parameters.AddWithValue("@tema_id", votacionMultiple.Tema.ID);
            command.Parameters.AddWithValue("@icon_name", votacionMultiple.IconName);

            return command;
        }
    }
}
