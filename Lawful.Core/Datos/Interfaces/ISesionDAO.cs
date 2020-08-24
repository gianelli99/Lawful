using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Datos.Interfaces
{
    interface ISesionDAO
    {
        int ValidarUsuario(string username, string password);
        int IniciarSesion(Modelo.Sesion sesion);
        void CerrarSesion(Modelo.Sesion sesion);
        List<Modelo.Sesion> Listar(DateTime fechaDesde, DateTime fechaHasta);
        List<Modelo.Sesion> ListarPorGrupo(int idGrupo, DateTime fechaDesde, DateTime fechaHasta);
        List<Modelo.Sesion> ListarPorUsuario(int idUsuario, DateTime fechaDesde, DateTime fechaHasta);
    }
}
