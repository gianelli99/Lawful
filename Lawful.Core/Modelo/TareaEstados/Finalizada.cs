using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.TareaEstados
{
    public class Finalizada : Interfaces.EstadoTarea
    {
        public int DBValue { get => 3; }
        public Tarea Tarea { get; set; }
        public void Mover()
        {
            throw new Exception("La tarea esta finalizada y no puede ser movida.");
        }

        public void SetTarea(Tarea tarea)
        {
            Tarea = tarea;
        }
    }
}
