using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Modelo
{
    // Clase "Singleton"
    public class Sesion
    {
        public int ID { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime LogIn { get; set; }
        public DateTime LogOut { get; set; }
        public TimeSpan CalcularTiempoSesion()
        {
            return LogOut - LogIn;
        }
        // Variable de clase privada que referencia a la única instancia 
        private static Sesion _instancia;
        // Constructor privado
        private Sesion()
        {
        }
        // Método de clase para crear u obtener la instancia
        public static Sesion ObtenerInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new Sesion();
            }
            return _instancia;
        }
        public static Sesion SesionInforme()
        {
            return new Sesion();
        }
    }
}
