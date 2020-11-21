using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class SesionInforme : Sesion
    {
        public DateTime Fecha { get; set; }
        public string DiaMes { get { return Fecha.Day + "/" + Fecha.Month; } }
        public double MinutosActivos { get; set; }
        public double HorasActivas { get {return Math.Round((MinutosActivos/60),2); } }
        public SesionInforme() 
        { 
        
        }
    }
}
