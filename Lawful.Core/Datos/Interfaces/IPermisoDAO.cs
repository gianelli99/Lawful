using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Datos.Interfaces
{
    interface IPermisoDAO
    {
        List<Modelo.Accion> ListarAcciones();
        List<Modelo.Vista> ListarVistas();
        List<int[]> ListarIDPermisos();
    }
}
