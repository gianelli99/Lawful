using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Logica
{
    public class TemaBL
    {
        private Datos.Interfaces.ITemaDAO temaDAO;
        private Datos.Interfaces.IIniciativaDAO iniciativaDAO;
        public TemaBL()
        {
            temaDAO = new Datos.DAO.TemaDAO_SqlServer();
            iniciativaDAO = new Datos.DAO.IniciativaDAO_SqlServer();
        }
        public void Insertar(Modelo.Tema tema)
        {
            try
            {
                temaDAO.Insertar(tema);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public void Modificar(Modelo.Tema tema)
        {
            try
            {
                temaDAO.Modificar(tema);
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
                temaDAO.Eliminar(id);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public Modelo.Tema Consultar(int id)
        {
            try
            {
                 return temaDAO.Consultar(id);
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public List<Modelo.Iniciativa> ListarIniciativas(int temaId)
        {
            try
            {
                var iniciativas = iniciativaDAO.ListarPorTema(temaId);
                return iniciativas;
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }
        public List<Modelo.Tema> Listar()
        {
            try
            {
                return temaDAO.Listar();
            }
            catch (Exception)
            {

                throw new Exception("Ha ocurrido un error");
            }
        }

    }
}
