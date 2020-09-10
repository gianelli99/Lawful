using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Modelo
{
    public class Vista
    {
        public int ID { get; set; }
        public string Descripcion { get; set; }
        public List<Accion> Acciones { get; set; }

        public Vista() {
            Acciones = new List<Accion>();
        }
            
    }
}
