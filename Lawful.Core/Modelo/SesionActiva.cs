using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Modelo
{
    public class SesionActiva : Sesion
    { 
        private static SesionActiva _instancia;
        private SesionActiva()
        {
        }
        public static SesionActiva ObtenerInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new SesionActiva();
            }
            return _instancia;
        }
    }
}
