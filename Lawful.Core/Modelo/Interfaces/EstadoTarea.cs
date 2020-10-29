using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Interfaces
{
    public interface EstadoTarea
    {
        int DBValue { get; }
        Tarea Tarea { get; set; }
        void Mover();
        void SetTarea(Tarea tarea);
    }
}
