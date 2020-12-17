using System;

namespace Lawful.Core.Modelo.TareaEstados
{
    public class Finalizada : Interfaces.IEstadoTarea
    {
        public int DBValue { get => 3; }
        public Tarea Tarea { get; set; }

        public void Mover()
        {
            //throw new Exception("La tarea esta finalizada y no puede ser movida.");
            Tarea.ChangeState(new PorHacer());
        }
        public void MoverAtras()
        {
            Tarea.ChangeState(new EnCurso());
        }

        public void SetTarea(Tarea tarea)
        {
            Tarea = tarea;
        }
    }
}
