using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Logica
{
    public class IniciativaBL
    {
        private Datos.Interfaces.IIniciativaDAO iniciativaDAO;
        public IniciativaBL()
        {
            iniciativaDAO = new Datos.DAO.IniciativaDAO_SqlServer();
        }
        public List<string[]> ListarTipos()
        {
            try
            {
                return iniciativaDAO.ListarTipos();
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void Insertar(Modelo.Iniciativa iniciativa)
        {
            try
            {
                iniciativaDAO.Insertar(iniciativa);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void Modificar(Modelo.Iniciativa iniciativa)
        {
            try
            {
                iniciativaDAO.Modificar(iniciativa);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void Eliminar(int id)
        {
            try
            {
                iniciativaDAO.Eliminar(id);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public Modelo.Iniciativa Consultar(int id)
        {
            try
            {
                return iniciativaDAO.Consultar(id);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }

    }
}
