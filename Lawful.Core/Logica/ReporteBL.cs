using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;

namespace Lawful.Core.Logica
{
    public class ReporteBL
    {
        private Datos.Interfaces.ISesionDAO sesionDAO;
        private Datos.Interfaces.IUsuarioDAO usuarioDAO;
        private Datos.Interfaces.IGrupoDAO grupoDAO;
        private Datos.Interfaces.IIniciativaDAO iniciativaDAO;
        private Datos.Interfaces.ITemaDAO temaDAO;
        public ReporteBL()
        {
            sesionDAO = new Datos.DAO.SesionDAO_SqlServer();
            usuarioDAO = new Datos.DAO.UsuarioDAO_SqlServer();
            grupoDAO = new Datos.DAO.GrupoDAO_SqlServer();
            iniciativaDAO = new Datos.DAO.IniciativaDAO_SqlServer();
            temaDAO = new Datos.DAO.TemaDAO_SqlServer();
        }
        public List<Modelo.SesionInforme> ObtenerUltimasSesiones(int userId)
        {
            try
            {
                return sesionDAO.ObtenerMinutosSesionUsuario(userId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Modelo.GrupoInforme> ObtenerUltimasSesionesGrupos()
        {
            try
            {
                return sesionDAO.ObtenerMinutosSesionGrupos();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Modelo.ParticipacionInforme> ObtenerParticipacionTemas()
        {
            try
            {
                return temaDAO.ObtenerParticipacionTemas();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Modelo.AuditoriaUsuario> ObtenerAuditoria(int userId)
        {
            try
            {
                return usuarioDAO.ObtenerAuditoria(userId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Modelo.IniciativaInforme> ObtenerCantIniciativas(int temaId)
        {
            try
            {
                return iniciativaDAO.ObtenerCantidadesPorTema(temaId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
