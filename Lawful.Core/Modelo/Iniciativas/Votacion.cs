using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Iniciativas
{
    public class Votacion : Iniciativa
    {
        public DateTime FechaLimite { get; set; }
        public List<Opcion> Opciones { get; set; }
        public Votacion(Usuario owner)
        : base(owner)
        {
            Opciones = new List<Opcion>();
        }
    }
}
