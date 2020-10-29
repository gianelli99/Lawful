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
        public string IconName { get; set; }
        public VotacionMultipleStrategy()
        {
            IconName = "People";
            Tipo = 8;
        }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            VotacionMultiple votacionMultiple = (VotacionMultiple)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo," +
                " descripcion," +
                " fecha_creacion," +
                " icon_name," +
                " usuario_id," +
                " iniciativa_tipo_id," +
                " max_opciones_seleccionables," +
                " tema_id, " +
                " fecha_cierre)" +

                " VALUES " +
                "(@titulo," +
                " @descripcion," +
                " @fecha_creacion," +
                " @icon_name," +
                " @usuario_id," +
                " @iniciativa_tipo_id," +
                " @max_opciones_seleccionables," +
                " @tema_id," +
                " @fecha_cierre);";

            command.Parameters.AddWithValue("@titulo", votacionMultiple.Titulo);
            command.Parameters.AddWithValue("@descripcion", votacionMultiple.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", votacionMultiple.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", IconName);
            command.Parameters.AddWithValue("@usuario_id", votacionMultiple.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@max_opciones_seleccionables", votacionMultiple.MaxOpcionesSeleccionables);
            command.Parameters.AddWithValue("@tema_id", votacionMultiple.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", votacionMultiple.FechaCierre);

            command.CommandText += "SELECT CAST(scope_identity() AS int);";

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
                "usuario_id=@usuario_id, " +
                "iniciativa_tipo_id=@iniciativa_tipo_id, " +
                "max_opciones_seleccionables=@max_opciones_seleccionables," +
                "tema_id=@tema_id, " +
                "fecha_cierre=@fecha_cierre " +
                $"WHERE id = {iniciativa.ID};";

            command.Parameters.AddWithValue("@titulo", votacionMultiple.Titulo);
            command.Parameters.AddWithValue("@descripcion", votacionMultiple.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", votacionMultiple.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", votacionMultiple.IconName);
            command.Parameters.AddWithValue("@usuario_id", votacionMultiple.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@max_opciones_seleccionables", votacionMultiple.MaxOpcionesSeleccionables);
            command.Parameters.AddWithValue("@tema_id", votacionMultiple.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", votacionMultiple.FechaCierre);

            return command;
        }

        public SqlCommand SetInsertOpciones(SqlCommand command, Iniciativa iniciativa)
        {
            VotacionMultiple votacion = (VotacionMultiple)iniciativa;

            command.CommandText = "INSERT INTO opciones VALUES ";
            foreach (Opcion item in votacion.Opciones)
            {
                command.CommandText += $"('{item.Descripcion}',{votacion.ID}),";
            }
            command.CommandText = command.CommandText.Remove(command.CommandText.Length - 1);
            return command;
        }
    }
}
