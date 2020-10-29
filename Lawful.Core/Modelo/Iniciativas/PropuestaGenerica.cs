using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Modelo.Iniciativas
{
    public class PropuestaGenerica : Iniciativa
    {
        public PropuestaGenerica(Usuario owner)
        : base(owner)
        {
        }
        public override string GetIniciativaType()
        {
            return "Propuesta genérica";
        }
    }
}
