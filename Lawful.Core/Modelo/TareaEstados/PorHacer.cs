using System;
using System.Collections.Generic;
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

        public void SetTarea(Tarea tarea)
        {
            Tarea = tarea;
        }
    }
}
