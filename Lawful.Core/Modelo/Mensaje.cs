using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class Mensaje
    {
        public int ID { get; set; }
        public Usuario Emisor { get; set; }
        public Usuario Receptor { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }

    }
}
