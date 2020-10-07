using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Logica
{
    public class UsuarioBL
    {
        private Datos.Interfaces.IUsuarioDAO usuarioDAO;
        private Datos.Interfaces.IGrupoDAO grupoDAO;
        private Datos.Interfaces.IVistaDAO vistaDAO;
        private Datos.Interfaces.IAccionDAO accionDAO;
        private Datos.Interfaces.ITemaDAO temaDAO;

        public UsuarioBL()
        {
            usuarioDAO = new Datos.DAO.UsuarioDAO_SqlServer();
            grupoDAO = new Datos.DAO.GrupoDAO_SqlServer();
            vistaDAO = new Datos.DAO.VistaDAO_SqlServer();
            accionDAO = new Datos.DAO.AccionDAO_SqlServer();
            temaDAO = new Datos.DAO.TemaDAO_SqlServer();
        }
        public Modelo.Usuario Consultar(int id)
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
        public Modelo.Usuario Consultar(string username, string email)
        {
            try
            {
                return usuarioDAO.Consultar(username,email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Modelo.Tema> ListarTemasDisponibles(int userId)
        {
            try
            {

                return temaDAO.ListarPorUsuario(userId);
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

                return vistaDAO.ListarPorUsuario(userId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Modelo.Accion> ListarAccionesDisponiblesEnVista(int userId, int vistaId)
        {
            try
            {
                return accionDAO.ListarPorVistaYUsuario(userId, vistaId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Modelo.Usuario> Listar()
        {
            try
            {
                return usuarioDAO.Listar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Modelo.Usuario> Listar(List<Modelo.Usuario> usuarios, string filtro)
        {
            try
            {
                return usuarios.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Apellido.ToUpper().Contains(filtro.ToUpper()) || x.Username.ToUpper().Contains(filtro.ToUpper()));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Eliminar(int id, int idEditor)
        {
            try
            {
                 usuarioDAO.Eliminar(id, idEditor);
            }
            catch (Exception)
            {
                throw new Exception("No se ha podido eliminar el usuario");
            }
        }
        public List<Modelo.Grupo> ListarGrupos()
        {
            try
            {
                return grupoDAO.Listar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Modelo.Grupo> ListarGrupos(int userId)
        {
            try
            {
                // Falta implementar, hay que llamar a GrupoDAO_SqlServer (ListarPorUsuario)
                // ¯\_(ツ)_/¯
                return null;
                //return usuarioDAO.ListarGrupos(userId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Insertar(Modelo.Usuario usuario, int idEditor)
        {
            try
            {
                if (usuario.Grupos.Count == 0)
                {
                    throw new Exception("Debe asignar al menos un grupo al usuario");
                }
                else
                {
                    if (UsernameEmailDisponibles(usuario.Username,usuario.Email, null))
                    {
                        usuario.Password = Hasheo.GetMd5Hash(usuario.Password);
                        usuarioDAO.Insertar(usuario, idEditor);
                    }
                    else
                    {
                        throw new Exception("Username o email no estan disponibles");
                    } 
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void EnviarEmail(string passDES, string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("gianjuanze@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Nueva contraseña Módulo de Seguridad";
                mail.Body = "Su nueva contraseña es: " + passDES;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("gianjuanze@gmail.com", "gianjuan2020");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private bool UsernameEmailDisponibles(string username, string email, string id)
        {
            try
            {
                return usuarioDAO.UsernameEmailDisponibles(username, email, id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Modificar(Modelo.Usuario usuario, int idEditor,bool modificaGrupo)
        {
            try
            {
                if (usuario.Grupos.Count == 0)
                {
                    throw new Exception("Debe asignar al menos un grupo al usuario");
                }
                else
                {
                    if (UsernameEmailDisponibles(usuario.Username, usuario.Email, usuario.ID.ToString()))
                    {
                        usuarioDAO.Modificar(usuario, idEditor, modificaGrupo);
                    }
                    else
                    {
                        throw new Exception("Username o email no estan disponibles");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool ValidarContrasena(Modelo.Usuario usuario,string actual,string nueva)
        {
            if (Hasheo.VerifyMd5Hash(actual, usuario.Password))
            {
                if (actual!=nueva)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public void CambiarContrasena(string pass, int userId, int editorId,bool needNewPass)
        {
            try
            {
                pass = Hasheo.GetMd5Hash(pass);
                usuarioDAO.CambiarContrasena(pass, userId, editorId, needNewPass);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
