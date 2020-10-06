using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Iniciativas
{
    public class Regla : Iniciativa
    {
        public int Relevancia { get; set; }
        public List<Opcion> Opciones { get; set; }
        public Regla(Usuario owner)
        : base(owner)
        {
            Opciones = new List<Opcion>();
            Relevancia = 1;
        }
    }
}
