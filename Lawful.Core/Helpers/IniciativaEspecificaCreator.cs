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
                    ((Asistire)iniciativa).FechaEvento = Convert.ToDateTime(campos[8]);
                    ((Asistire)iniciativa).Lugar = campos[9];
                    ((Asistire)iniciativa).FechaLimiteConfirmacion = Convert.ToDateTime(campos[10]);
                    RellenarCamposGenerales(iniciativa,campos);
                    return iniciativa;
                case 2:
                    iniciativa = new DoDont(owner);
                    ((DoDont)iniciativa).Tipo = "Do";
                    RellenarCamposGenerales(iniciativa,campos);
                    return iniciativa;
                case 3:
                    iniciativa = new DoDont(owner);
                    ((DoDont)iniciativa).Tipo = "Don't";
                    RellenarCamposGenerales(iniciativa,campos);
                    return iniciativa;
                case 4:
                    iniciativa = new FAQ(owner);
                    //no seteamos respuesta correcta porque se hace en el consultar
                    RellenarCamposGenerales(iniciativa,campos);
                    return iniciativa;
                case 5:
                    iniciativa = new PropuestaGenerica(owner);
                    RellenarCamposGenerales(iniciativa,campos);
                    return iniciativa;
                case 6:
                    iniciativa = new Regla(owner);
                    ((Regla)iniciativa).Relevancia = Convert.ToInt32(campos[12]);
                    RellenarCamposGenerales(iniciativa,campos);
                    return iniciativa;
                case 7:
                    iniciativa = new Votacion(owner);
                    ((Votacion)iniciativa).FechaLimite = Convert.ToDateTime(campos[13]);
                    RellenarCamposGenerales(iniciativa,campos);
                    return iniciativa;
                case 8:
                    iniciativa = new VotacionMultiple(owner);
                    ((VotacionMultiple)iniciativa).FechaLimite = Convert.ToDateTime(campos[13]);
                    ((VotacionMultiple)iniciativa).MaxOpcionesSeleccionables = Convert.ToInt32(campos[14]);
                    RellenarCamposGenerales(iniciativa,campos);
                    return iniciativa;
                default:
                    break;
            }
            return null;
        }
        private static void RellenarCamposGenerales(Iniciativa iniciativa, string[] campos)
        {
            iniciativa.ID = Convert.ToInt32(campos[0]);
            iniciativa.Titulo = campos[1];
            iniciativa.Descripcion = campos[2];
            iniciativa.FechaCreacion = Convert.ToDateTime(campos[3]);
            iniciativa.IconName = campos[4];
            iniciativa.IsOpen = Convert.ToBoolean(campos[15]);
        } 
    }
}
