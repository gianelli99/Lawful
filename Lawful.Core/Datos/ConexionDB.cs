using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Lawful.Core.Datos
{
    public class ConexionDB
    {

        protected SqlConnection Conexion = new SqlConnection(@"Server=(LocalDb)\MSSQLLocalDB;DataBase=ModuloDeSeguridad;Integrated Security=true");
    }
}
