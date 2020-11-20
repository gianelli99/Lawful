using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    public interface IMensajeDAO
    {
        List<Mensaje> ObtenerMensajesUsuarios(int userID1, int userID2);
        List<Mensaje> ObtenerMensajesTema(int temaID);
        void EnviarMensajeUsuario(MensajeAUsuario mensaje);
        void EnviarMensajeTema(MensajeATema mensaje);
    }
}
