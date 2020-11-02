using Lawful.Core.Datos.Interfaces;
using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lawful.Core.Logica
{
    public class TareaBL
    {
        private Datos.Interfaces.ITareaDAO tareaDAO;
        private Datos.Interfaces.IIncidenciaDAO incidenciaDAO;
        public TareaBL()
        {
            tareaDAO = new Datos.DAO.TareaDAO_SqlServer();
            incidenciaDAO = new Datos.DAO.IncidenciaDAO_SqlServer();
        }
        public List<Tarea> ListarPorTema(int temaId)
        {
            try
            {
                return tareaDAO.ListarPorTema(temaId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Insertar(Tarea tarea, int temaId)
        {
            try
            {
                tareaDAO.Insertar(tarea, temaId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void ModificarDatos(Tarea tarea)
        {
            try
            {
                tareaDAO.Modificar(tarea);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void ModificarEstado(Tarea tarea)
        {
            try
            {
                tareaDAO.CambiarEstado(tarea);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Eliminar(int tareaId)
        {
            try
            {
                tareaDAO.Eliminar(tareaId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void InsertarIncidencia(Incidencia incidencia, int tareaId)
        {
            try
            {
                incidenciaDAO.Agregar(incidencia, tareaId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void EliminarIncidencia(int id)
        {
            try
            {
                incidenciaDAO.Eliminar(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void CambiarEstadoIncidencia(int id, bool isDone)
        {
            try
            {
                incidenciaDAO.CambiarEstado(id, isDone);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void ModificarIncidencia(Incidencia incidencia)
        {
            try
            {
                incidenciaDAO.Modificar(incidencia);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void InsertarComentario(int tareaId, Comentario comentario)
        {
            try
            {
                tareaDAO.InsertarComentario(tareaId, comentario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
