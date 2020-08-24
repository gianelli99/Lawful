using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Modelo
{
    public class Permiso
    {
        public int ID { get; set; }
        public Grupo Grupo { get; set; }
        public Accion Accion { get; set; }
        public Vista Vista { get; set; }
        public bool TienePermiso { get; set; }

        public string ObtenerNombre()
        {
            return this.Vista.Descripcion + @" - " + this.Accion.Descripcion;
        }
    }
}
