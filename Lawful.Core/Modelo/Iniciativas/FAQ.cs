using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Iniciativas
{
    public class FAQ : Iniciativa
    {
        public Comentario RespuestaCorrecta { get; set; }
        public FAQ(Usuario owner)
        : base(owner)
        {
            RespuestaCorrecta = new Comentario();
        }
        public override string GetIniciativaType()
        {
            return "Frequently asked question";
        }
    }
}
