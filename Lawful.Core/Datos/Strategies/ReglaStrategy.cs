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
        public string IconName { get; set; }
        public ReglaStrategy()
        {
            IconName = "Important";
            Tipo = 6;
        }

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
            command.Parameters.AddWithValue("@icon_name", IconName);
            command.Parameters.AddWithValue("@usuario_id", regla.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@relevancia", regla.Relevancia);
            command.Parameters.AddWithValue("@tema_id", regla.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", regla.FechaCierre);

            command.CommandText += "SELECT CAST(scope_identity() AS int);";

            return command;
        }

        public SqlCommand SetInsertOpciones(SqlCommand command, Iniciativa iniciativa)
        {
            Regla regla = (Regla)iniciativa;

            command.CommandText = "INSERT INTO opciones VALUES ";
            foreach (Opcion item in regla.Opciones)
            {
                command.CommandText += $"('{item.Descripcion}',{regla.ID}),";
            }
            command.CommandText = command.CommandText.Remove(command.CommandText.Length - 1);
            return command;
        }
    }
}
