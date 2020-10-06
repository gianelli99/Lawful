using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.Interfaces
{
    interface IIniciativaDAO
    {
        List<Modelo.Iniciativa> ListarPorTema(int temaId);
        List<Modelo.Iniciativa> ListarPorUsuario(int userId);
        void Insertar(QueryMiddleware.IniciativaMiddleware iniciativaMiddle);
        void Eliminar(int id);
        void Modificar(QueryMiddleware.IniciativaMiddleware iniciativaMiddle);
        Modelo.Iniciativa Consultar(int id);
    }
}
