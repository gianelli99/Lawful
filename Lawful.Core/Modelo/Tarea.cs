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
        public Usuario Responsable { get; set; }
        public Tarea()
        {
            IncidenciasSecundarias = new List<Incidencia>();
        }

    }
}
