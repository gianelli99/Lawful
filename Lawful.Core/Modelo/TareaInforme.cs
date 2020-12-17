using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class TareaInforme
    {
        public DateTime Fecha { get; set; }
        public string DiaMes { get { return Fecha.Day + "/" + Fecha.Month; } }
        public int Cantidad { get; set; }
    }
}
