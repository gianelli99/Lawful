using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Lawful.Core.Logica
{
    public class SesionBL
    {
        private Datos.Interfaces.ISesionDAO sesionDAO;
        private Datos.Interfaces.IUsuarioDAO usuarioDAO;        

        private static SesionBL instancia;

        public void IniciarSesion()
        {
            try
            {
                int id = sesionDAO.IniciarSesion(Modelo.SesionActiva.ObtenerInstancia());
                Modelo.SesionActiva.ObtenerInstancia().ID = id;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void FinalizarSesion()
        {
            Modelo.SesionActiva.ObtenerInstancia().LogOut = DateTime.Now;
            try
            {
                sesionDAO.CerrarSesion(Modelo.SesionActiva.ObtenerInstancia()); 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool NeedNewPassword(int userId)
        {
            try
            {
                if (usuarioDAO.NeedNewPassword(userId))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static SesionBL ObtenerInstancia()
        {
            if (instancia == null)
            {
                instancia = new SesionBL();
            }
            return instancia;
        }
        private SesionBL()
        {
            sesionDAO = new Datos.DAO.SesionDAO_SqlServer();
            usuarioDAO = new Datos.DAO.UsuarioDAO_SqlServer();
        }
        public int ValidarUsuario(string username, string password)
        {
            try
            {
                password = Hasheo.GetMd5Hash(password);
                return sesionDAO.ValidarUsuario(username, password);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public Modelo.Usuario ConsultarUsuario(int id)
        {
            try
            {
                return usuarioDAO.Consultar(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}