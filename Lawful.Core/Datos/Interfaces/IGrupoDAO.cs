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
        List<Modelo.Grupo> ListarPorUsuario(int userId); // New
        void Insertar(Modelo.Grupo grupo); // El grupo tiene una lista de acciones, por eso no pasa como parametro
        void Eliminar(int id);
        Modelo.Grupo Consultar(int id);
        void Modificar(Modelo.Grupo grupo); // El grupo tiene una lista de acciones, por eso no pasa como parametro
        int CantidadUsuarios(int groupId);
        bool DescripcionCodigoDisponible(string descripción, string codigo, string id);
    }
}
