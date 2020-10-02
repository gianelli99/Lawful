using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    interface IIniciativaDAO
    {
        List<Modelo.Iniciativa> Listar();
        List<Modelo.Iniciativa> ListarPorUsuario(int userId);
        void Insertar(Modelo.Iniciativa iniciativa);
        void Eliminar(int id);
        void Modificar(Modelo.Iniciativa iniciativa);
        Modelo.Iniciativa Consultar(int id);
    }
}
