using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Datos.Interfaces
{
    interface IGrupoDAO
    {
        List<Modelo.Grupo> Listar();
        void Insertar(Modelo.Grupo grupo, List<Modelo.Permiso> permisos);
        Modelo.Grupo Consultar(int id);
        void Modificar(Modelo.Grupo grupo, List<Modelo.Permiso> permisos);
        void Eliminar(int id);
        List<int[]> ListarIDPermisos(int id);
        int CantidadUsuarios(int id);
        bool DescripcionCodigoDisponible(string descripción, string codigo, string id);
    }
}
