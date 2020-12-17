using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Interfaces
{
    public interface IEstadoTarea
    {
        int DBValue { get; }
        Tarea Tarea { get; set; }
        void Mover();
        void MoverAtras();
        void SetTarea(Tarea tarea);
    }
}
