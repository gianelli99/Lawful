using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lawful.Core.Logica
{ 
    // Clase "Facade"
    public class GrupoBL
    {
        // Subsistemas
        private Datos.Interfaces.IGrupoDAO grupoDAO;
        private Datos.Interfaces.IAccionDAO accionDAO;
        private Datos.Interfaces.IUsuarioDAO usuarioDAO;
        public GrupoBL()
        {
            grupoDAO = new Datos.DAO.GrupoDAO_SqlServer();
            accionDAO = new Datos.DAO.AccionDAO_SqlServer();
            usuarioDAO = new Datos.DAO.UsuarioDAO_SqlServer();
        }

        public List<Modelo.Accion> ListarAcciones()
        {
            try
            {
                return accionDAO.Listar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        

        // Los siguientes métodos no muestran su implementación
        // debido a la extensión total del documento
        public void Insertar(Modelo.Grupo group)
        {
            try
            {
                if (DescripcionCodigoDisponible(group.Descripcion, group.Codigo, null))
                {
                    grupoDAO.Insertar(group);
                }
                else
                {
                    throw new Exception("La descripción o el código no están disponible");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Modificar(Modelo.Grupo group)
        {
            try
            {
                if (DescripcionCodigoDisponible(group.Descripcion,group.Codigo,group.ID.ToString()))
                {
                    grupoDAO.Modificar(group);
                }
                else
                {
                    throw new Exception("La descripción o el código no están disponible");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Eliminar(int id)
        {
            try
            {
                int users = grupoDAO.CantidadUsuarios(id);
                if (users == 0)
                {
                    grupoDAO.Eliminar(id);
                }
                else
                {
                    throw new Exception("Este grupo posee " + users + " usuarios, no puede ser eliminado.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Modelo.Grupo Consultar(int id)
        {
            try
            {
                return grupoDAO.Consultar(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<Modelo.Grupo> Listar()
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
        public List<Modelo.Grupo> Listar(List<Modelo.Grupo> grupos, string filtro)
        {
            try
            {
                return grupos.FindAll(x => x.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Codigo.ToUpper().Contains(filtro.ToUpper()));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private bool DescripcionCodigoDisponible(string descripción, string codigo, string id)
        {
            try
            {
                return grupoDAO.DescripcionCodigoDisponible(descripción, codigo, id);  
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
