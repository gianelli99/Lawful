using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class Opcion
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public List<Usuario> Votantes { get; set; }
        public Opcion()
        {
            Votantes = new List<Usuario>();
        }
    }
}
