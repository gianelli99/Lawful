using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Datos.Interfaces
{
    interface IUsuarioDAO
    {
        List<Modelo.Usuario> Listar(); 
        void Insertar(Modelo.Usuario t, int idEditor);
        Modelo.Usuario Consultar(int id);
        Modelo.Usuario Consultar(string username, string email);
        void Eliminar(int id, int idEditor);
        void Modificar(Modelo.Usuario t, int idEditor, bool modificaGrupo);
        void CambiarContrasena(string pass, int userId, int editorId, bool needNewPass);
        bool UsernameEmailDisponibles(string username, string email, string id);
        bool NeedNewPassword(int userId);
        List<AuditoriaUsuario> ObtenerAuditoria(int userId);
        List<Usuario> ListarPorTema(int temaId);
    }
}
