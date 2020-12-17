using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class Tarea
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPorHacer { get; set; }
        public DateTime FechaEnCurso { get; set; }
        public DateTime FechaFinalizada { get; set; }
        public int Importancia { get; set; }
        public List<Incidencia> IncidenciasSecundarias { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public Usuario Responsable { get; set; }
        public Interfaces.IEstadoTarea Estado { get; set; }
        public Tarea()
        {
            IncidenciasSecundarias = new List<Incidencia>();
            Comentarios = new List<Comentario>();
        }
        public void ChangeState(Interfaces.IEstadoTarea estado)
        {
            Estado = estado;
            Estado.SetTarea(this);
        }

    }
}
