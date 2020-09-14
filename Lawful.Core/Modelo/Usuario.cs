using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Modelo
{
    public class Usuario
    {
        private string username;
        public int ID { get; set; }
        public string Username
        {
            get { return username; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("El Username es inválido");
                }
            }
        }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool Estado { get; set; }
        public List<Grupo> Grupos { get; set; }

        public Usuario()
        {
            Grupos = new List<Grupo>();
        }
    }
}
