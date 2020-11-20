using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo
{
    public class MensajeATema : Mensaje
    {
        public Tema Receptor { get; set; }

        public override string EmisorFecha()
        {
            return Emisor.GetNombreCompleto() + " - " + Fecha.ToString();
        }
    }
}
