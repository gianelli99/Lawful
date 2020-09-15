using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    interface IVistaDAO
    {
        List<Modelo.Vista> Listar();
        List<Modelo.Vista> ListarPorUsuario(int userId);

    }
}
