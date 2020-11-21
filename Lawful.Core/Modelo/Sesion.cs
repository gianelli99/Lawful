using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public abstract class Sesion
    {
        public int ID { get; set; }
        public Usuario Usuario { get; set; }
    }
}
