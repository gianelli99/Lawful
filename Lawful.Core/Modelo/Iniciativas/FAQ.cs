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
        }
    }
}
