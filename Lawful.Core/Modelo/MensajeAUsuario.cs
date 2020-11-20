using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class MensajeAUsuario : Mensaje
    {
        public Usuario Receptor { get; set; }

        public override string EmisorFecha()
        {
            return Fecha.ToString();
        }
    }
}
