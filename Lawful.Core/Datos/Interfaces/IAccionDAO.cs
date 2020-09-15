using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    interface IAccionDAO
    {
        List<Modelo.Accion> Listar();
        List<Modelo.Accion> ListarPorUsuario(int userId);
        List<Modelo.Accion> ListarPorGrupo(int groupId);
        List<Modelo.Accion> ListarPorVistaYUsuario(int userId, int vistaId);


    }
}
