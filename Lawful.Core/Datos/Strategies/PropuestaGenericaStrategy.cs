using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Lawful.Core.Modelo.Iniciativas;
using System.Text;

namespace Lawful.Core.Datos.Strategies
{
    public class PropuestaGenericaStrategy : IQueryStrategy
    {
        public int Tipo { get; set; }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            PropuestaGenerica propuestaGenerica = (PropuestaGenerica)iniciativa;
            command.CommandText = "INSERT INTO iniciativas " +
                "(titulo, descripcion," +
                " fecha_creacion," +
                " icon_name," +
                " usuario_id," +
                " iniciativa_tipo_id," +
                " tema_id," +
                " fecha_cierre)" +

                " VALUES " +
                "(@titulo," +
                "@descripcion," +
                "@fecha_creacion," +
                "@icon_name," +
                "@usuario_id," +
                "@iniciativa_tipo_id," +
                "@tema_id," +
                "@fecha_cierre);";

            command.Parameters.AddWithValue("@titulo", propuestaGenerica.Titulo);
            command.Parameters.AddWithValue("@descripcion", propuestaGenerica.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", propuestaGenerica.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", propuestaGenerica.IconName);
            command.Parameters.AddWithValue("@usuario_id", propuestaGenerica.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@tema_id", propuestaGenerica.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", propuestaGenerica.FechaCierre); 


            return command;
        }

        public SqlCommand SetUpdateCommand(SqlCommand command, Iniciativa iniciativa)
        {
            PropuestaGenerica propuestaGenerica = (PropuestaGenerica)iniciativa;
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

            command.Parameters.AddWithValue("@titulo", propuestaGenerica.Titulo);
            command.Parameters.AddWithValue("@descripcion", propuestaGenerica.Descripcion);
            command.Parameters.AddWithValue("@fecha_creacion", propuestaGenerica.FechaCreacion);
            command.Parameters.AddWithValue("@icon_name", propuestaGenerica.IconName);
            command.Parameters.AddWithValue("@usuario_id", propuestaGenerica.Owner.ID);
            command.Parameters.AddWithValue("@iniciativa_tipo_id", Tipo);
            command.Parameters.AddWithValue("@tema_id", propuestaGenerica.Tema.ID);
            command.Parameters.AddWithValue("@fecha_cierre", propuestaGenerica.FechaCierre);

            return command;
        }
    }
}
