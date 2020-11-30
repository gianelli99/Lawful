using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class GrupoInforme
    {
        public Grupo Grupo { get; set; }
        public string DescripcionGrupo { get { return Grupo.Descripcion; } }
        public int CantUsers { get; set; }
        public double MinutosTotales { get; set; }
        public int Dias { get; set; }
        public double PromedioPorDiaEnHoras { get { return Math.Round((MinutosTotales / 60 / CantUsers / Dias), 2); } }
    }
}
