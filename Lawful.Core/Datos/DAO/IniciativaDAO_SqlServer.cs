using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Datos.DAO
{
    public class IniciativaDAO_SqlServer : ConexionDB, Interfaces.IIniciativaDAO
    {
        public Iniciativa Consultar(int id)
        {
            throw new NotImplementedException();
        }

        public void Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public void Insertar(Iniciativa iniciativa)
        {
            throw new NotImplementedException();
        }

        public List<Iniciativa> Listar()
        {
            throw new NotImplementedException();
        }

        public List<Iniciativa> ListarPorUsuario(int userId)
        {
            throw new NotImplementedException();
        }

        public void Modificar(Iniciativa iniciativa)
        {
            throw new NotImplementedException();
        }
    }
}
