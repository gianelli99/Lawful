using Microsoft.Data.SqlClient;

namespace Lawful.Core.Datos
{
    public class ConexionDB
    {

        //protected SqlConnection Conexion = new SqlConnection(@"Server=localhost\SQLEXPRESS;Initial Catalog=ModuloDeSeguridad;Integrated Security=SSPI");
        //protected SqlConnection Conexion = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=ModuloDeSeguridad;Integrated Security=SSPI");
        protected SqlConnection Conexion = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=ModuloDeSeguridad;Integrated Security=SSPI;");
        // @"Server=(LocalDb)\MSSQLLocalDB;DataBase=ModuloDeSeguridad;Integrated Security=true"
    }
}
