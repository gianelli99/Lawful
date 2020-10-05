using System;
using System.Collections.Generic;
using System.Text;
using Lawful.Core.Modelo.Iniciativas;
using Lawful.Core.Modelo;

namespace Lawful.Core.Helpers
{
    public class IniciativaEspecificaCreator
    {
        public static Iniciativa CrearIniciativaEspecifica(string[] campos)
        {
            Usuario owner = new Usuario() { ID = Convert.ToInt32(campos[6]) };
            Iniciativa iniciativa;
            switch (Convert.ToInt32(campos[7]))
            {
                case 1:
                    iniciativa = new Asistire(owner);
                    break;
                case 2:
                    iniciativa = new DoDont(owner);
                    ((DoDont)iniciativa).Tipo = "Do";
                    break;
                case 3:
                    iniciativa = new DoDont(owner);
                    ((DoDont)iniciativa).Tipo = "Don't";
                    break;
                case 4:
                    iniciativa = new FAQ(owner);
                    break;
                case 5:
                    iniciativa = new PropuestaGenerica(owner);
                    break;
                case 6:
                    iniciativa = new Regla(owner);
                    break;
                case 7:
                    iniciativa = new Votacion(owner);
                    break;
                case 8:
                    iniciativa = new VotacionMultiple(owner);
                    break;
                default:
                    break;
            }
            return null;
        }
        private static void RellenarCamposGenerales(Modelo.Iniciativa iniciativa)
        {

        } 
    }
}
