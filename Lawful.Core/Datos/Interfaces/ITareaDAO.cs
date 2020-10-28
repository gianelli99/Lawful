using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    public interface ITareaDAO
    {
        void Insertar(Tarea tarea);
        void Modificar(Tarea tarea);
        void Eliminar(int tareaId);
        List<Tarea> ListarPorTema(int temaId);
    }
}
