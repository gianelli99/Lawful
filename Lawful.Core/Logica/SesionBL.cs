using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lawful.Core.Logica.Interfaces;
using System.Timers;

namespace Lawful.Core.Logica
{
    public class SesionBL : Interfaces.ISesionPublisher
    {
        private Datos.Interfaces.ISesionDAO sesionDAO;
        private Datos.Interfaces.IUsuarioDAO usuarioDAO;        

        public List<ISesionObserver> Observadores { get; set; }

        private static SesionBL instancia;

        private static Timer sesionTime;
        public void IniciarSesion()
        {
            try
            {
                int id = sesionDAO.IniciarSesion(Modelo.Sesion.ObtenerInstancia());
                Modelo.Sesion.ObtenerInstancia().ID = id;
                sesionTime = new Timer(7200000);
                sesionTime.Elapsed += SesionTime_Elapsed;
                sesionTime.AutoReset = true;
                sesionTime.Enabled = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void SesionTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            FinalizarSesion();
        }
        public void Suscribir(ISesionObserver observer)
        {
            Observadores.Add(observer);
        }
        public void Desuscribir(ISesionObserver observer)
        {
            Observadores.Remove(observer);
        }
        public void FinalizarSesion()
        {
            if (sesionTime != null)
            {
                sesionTime.Dispose();
            }
            this.Notificar();
            Modelo.Sesion.ObtenerInstancia().LogOut = DateTime.Now;
            try
            {
                sesionDAO.CerrarSesion(Modelo.Sesion.ObtenerInstancia()); 
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Notificar()
        {
            if (Observadores.Count - 1>=0)
            {
                Observadores[Observadores.Count - 1].Actualizar(true);
            }
            for (int i = Observadores.Count-1; i >= 0; i--)
            {
                Observadores[i].Actualizar(false);
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
            Observadores = new List<ISesionObserver>();
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
        public List<Modelo.Vista> ListarVistasDisponibles(int userId)
        {
            try
            {
                return usuarioDAO.ListarVistasDisponibles(userId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}