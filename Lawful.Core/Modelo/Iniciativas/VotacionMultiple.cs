using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Iniciativas
{
    public class VotacionMultiple : Iniciativa
    {
        public DateTime FechaLimite { get; set; }
        public int MaxOpcionesSeleccionables { get; set; }
        public List<Opcion> Opciones { get; set; }
        public VotacionMultiple(Usuario owner)
        : base(owner)
        {
            Opciones = new List<Opcion>();
        }
    }
}
