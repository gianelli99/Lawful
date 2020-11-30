using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Modelo
{
    public class Usuario
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool Estado { get; set; }
        public List<Grupo> Grupos { get; set; }
        public List<Tema> Temas { get; set; }

        public Usuario()
        {
            Grupos = new List<Grupo>();
            Temas = new List<Tema>();
        }
        public string GetNombreCompleto()
        {
            return Nombre + " " + Apellido;
        }
        public override string ToString()
        {
            return GetNombreCompleto();
        }
    }
}
