using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    public interface IMensajeDAO
    {
        List<Mensaje> ObtenerMensajes(int userID1, int userID2);
        void EnviarMensaje(Mensaje mensaje);
    }
}
