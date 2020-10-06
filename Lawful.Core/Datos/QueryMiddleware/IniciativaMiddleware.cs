using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos.QueryMiddleware
{
    public abstract class IniciativaMiddleware
    {
        public int Tipo { get; set; }
        public Iniciativa Iniciativa { get; set; }

        public abstract SqlCommand SetInsertCommand(SqlCommand command);
        //public abstract string GetUpdateQuery();
    }
}
