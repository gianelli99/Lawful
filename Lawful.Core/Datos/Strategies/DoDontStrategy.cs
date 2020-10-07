using Lawful.Core.Datos.QueryMiddleware;
using Lawful.Core.Modelo;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Strategies
{
    public class DoDontStrategy : IQueryStrategy
    {
        public int Tipo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public SqlCommand SetInsertCommand(SqlCommand command, Iniciativa iniciativa)
        {
            throw new NotImplementedException();
        }

        public SqlCommand SetUpdateCommand(SqlCommand command, Iniciativa iniciativa)
        {
            throw new NotImplementedException();
        }
    }
}
