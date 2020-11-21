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
        public DateTime LogIn { get; set; }
        public DateTime LogOut { get; set; }
        private SesionActiva()
        {
        }
        public TimeSpan CalcularTiempoSesion()
        {
            return LogOut - LogIn;
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
