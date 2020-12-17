using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Lawful.Core.Modelo.TareaEstados
{
    public class PorHacer : Interfaces.IEstadoTarea
    {
        public int DBValue { get => 1; }
        public Tarea Tarea { get; set; }

        public void Mover()
        {
            Tarea.ChangeState(new EnCurso());
        }

        public void MoverAtras()
        {
            throw new Exception("Una tarea por hacer debe pasar por el estado 'en curso' primero");
        }

        public void SetTarea(Tarea tarea)
        {
            Tarea = tarea;
        }
    }
}
