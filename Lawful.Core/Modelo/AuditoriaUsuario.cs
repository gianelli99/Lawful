using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Modelo
{
    public class AuditoriaUsuario
    {
        public Usuario Actor { get; set; }
        public Usuario RegistroViejo { get; set; }
        public DateTime FechaHora { get; set; }
        public string Accion { get; set; }
    }
}
