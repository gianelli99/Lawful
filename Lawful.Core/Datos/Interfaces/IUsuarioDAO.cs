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
        List<Modelo.Grupo> ListarGrupos(int id);
        void Insertar(Modelo.Usuario t, int idEditor);
        Modelo.Usuario Consultar(int id);
        Modelo.Usuario Consultar(string username, string email);
        void CambiarContrasena(string pass, int userId, int editorId, bool needNewPass);
        void Modificar(Modelo.Usuario t, int idEditor, bool modificaGrupo);
        void Eliminar(int id, int idEditor);
        List<Modelo.Vista> ListarVistasDisponibles(int id);
        List<Modelo.Accion> ListarAccionesDisponibles(int idUser, int idVista);
        bool UsernameEmailDisponibles(string username, string email, string id);
        bool NeedNewPassword(int userId);
    }
}
