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
        List<Modelo.SesionInforme> ObtenerMinutosSesionUsuario(int userID);
        List<Modelo.GrupoInforme> ObtenerMinutosSesionGrupos();
    }
}
