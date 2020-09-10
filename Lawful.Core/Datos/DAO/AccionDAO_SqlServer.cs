using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.DAO
{
    public class AccionDAO_SqlServer : Interfaces.IAccionDAO
    {
        public List<Accion> Listar()
        {
            throw new NotImplementedException();
        }

        public List<Accion> ListarPorGrupo(int groupId)
        {
            throw new NotImplementedException();
        }

        public List<Accion> ListarPorUsuario(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Accion> ListarPorVistaYUsuario(int userId, int vistaId)
        {
            throw new NotImplementedException();
        }
    }
}
