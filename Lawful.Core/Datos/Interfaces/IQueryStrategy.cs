using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.QueryMiddleware
{
    public interface IQueryStrategy
    {
        int Tipo { get; set; }
        SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa);
        SqlCommand SetInsertOpciones(SqlCommand command, Iniciativa iniciativa);
        
    }
}
