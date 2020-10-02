using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    interface ITemaDAO
    {
        List<Modelo.Tema> Listar();
        void Insertar(Modelo.Tema tema);
        void Eliminar(int id);
        void Modificar(Modelo.Tema tema);
        Modelo.Tema Consultar(int id);
    }
}
