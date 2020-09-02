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
        int IniciarSesion(Modelo.SesionActiva sesion);
        void CerrarSesion(Modelo.SesionActiva sesion);
        List<Modelo.SesionInforme> Listar(DateTime fechaDesde, DateTime fechaHasta);
        List<Modelo.SesionInforme> ListarPorGrupo(int idGrupo, DateTime fechaDesde, DateTime fechaHasta);
        List<Modelo.SesionInforme> ListarPorUsuario(int idUsuario, DateTime fechaDesde, DateTime fechaHasta);
    }
}
