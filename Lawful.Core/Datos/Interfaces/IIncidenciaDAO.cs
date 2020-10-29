using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    public interface IIncidenciaDAO
    {
        void CambiarEstado(int id, bool isDone);
        void Eliminar(int id);
        void Modificar(Incidencia incidencia);
        void Agregar(Incidencia incidencia, int tareaId);
    }
}
