using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public abstract class Mensaje
    {
        public int ID { get; set; }
        public Usuario Emisor { get; set; }
        public string Texto { get; set; }
        public DateTime Fecha { get; set; }
        public abstract string EmisorFecha();

    }
}
