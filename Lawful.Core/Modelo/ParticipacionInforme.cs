using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class ParticipacionInforme
    {
        public string Tema { get; set; }
        public double Iniciativas { get; set; }
        public double Usuarios { get; set; }
        public double Votos { get; set; }
        public double Participacion { get { return Votos / (Iniciativas * Usuarios); } }
    }
}
