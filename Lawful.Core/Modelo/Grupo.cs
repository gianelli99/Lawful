using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Modelo
{
    public class Grupo
    {
        public int ID { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public List<Accion> Acciones { get; set; }
        public Grupo() { Acciones = new List<Accion>(); }
    }
}
