using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;
using Lawful.Core.Modelo;

namespace Lawful.Core.Logica
{
    public class ReporteBL
    {
        private Datos.Interfaces.ISesionDAO sesionDAO;
        private Datos.Interfaces.IUsuarioDAO usuarioDAO;
        private Datos.Interfaces.IIniciativaDAO iniciativaDAO;
        private Datos.Interfaces.ITemaDAO temaDAO;
        private Datos.Interfaces.ITareaDAO tareaDAO;
        public ReporteBL()
        {
            sesionDAO = new Datos.DAO.SesionDAO_SqlServer();
            usuarioDAO = new Datos.DAO.UsuarioDAO_SqlServer();
            iniciativaDAO = new Datos.DAO.IniciativaDAO_SqlServer();
            temaDAO = new Datos.DAO.TemaDAO_SqlServer();
            tareaDAO = new Datos.DAO.TareaDAO_SqlServer();
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
        public List<Modelo.TareaInforme>[] ObtenerEstadoTareasPorDia(int temaId)
        {
            try
            {
                var tareas = tareaDAO.ListarPorTema(temaId);
                if (tareas!=null && tareas.Count > 0)
                {
                    List<Modelo.TareaInforme>[] resultado = new List<Modelo.TareaInforme>[3];
                    resultado[0] = new List<TareaInforme>();
                    resultado[1] = new List<TareaInforme>();
                    resultado[2] = new List<TareaInforme>();
                    DateTime inicio = DateTime.MaxValue, fin = DateTime.MinValue;
                    foreach (var item in tareas)
                    {
                        if (item.FechaPorHacer < inicio)
                        {
                            inicio = item.FechaPorHacer;
                        }
                        if (item.FechaFinalizada > fin)
                        {
                            fin = item.FechaFinalizada;
                        }
                    }

                    for (DateTime date = inicio; date <= fin; date = date.AddDays(1))
                    {
                        var tareaToDo = new TareaInforme();
                        var tareaDoing = new TareaInforme();
                        var tareaDone = new TareaInforme();
                        tareaToDo.Fecha = date;
                        tareaDoing.Fecha = date;
                        tareaDone.Fecha = date;
                        foreach (var tarea in tareas)
                        {
                            // funcionamiento TODO
                            if (tarea.Estado.DBValue == 1)// con estado 1
                            {
                                if (tarea.FechaPorHacer.Date <= date.Date)
                                {
                                    tareaToDo.Cantidad++;
                                }
                            }
                            else
                            {
                                if (tarea.FechaPorHacer.Date <= date.Date && tarea.FechaEnCurso.Date > date.Date)// cuando tiene estado 2 o 3
                                {
                                    tareaToDo.Cantidad++;
                                }
                            }

                            //funcionamiento DOING
                            if (tarea.Estado.DBValue == 2)// con estado 2
                            {
                                if (tarea.FechaEnCurso.Date <= date.Date)
                                {
                                    tareaDoing.Cantidad++;
                                }
                            }
                            if(tarea.Estado.DBValue == 3 && tarea.FechaEnCurso.Date<=date.Date && tarea.FechaFinalizada.Date > date.Date)// con estado 3
                            {
                                tareaDoing.Cantidad++;
                            }

                            //funcionamiento DONE
                            if (tarea.Estado.DBValue == 3)// con estado 3
                            {
                                if (tarea.FechaFinalizada.Date <= date.Date)
                                {
                                    tareaDone.Cantidad++;
                                }
                            }
                        }
                        resultado[0].Add(tareaToDo);
                        resultado[1].Add(tareaDoing);
                        resultado[2].Add(tareaDone);
                    }
                    return resultado;
                }
                else
                {
                    List<Modelo.TareaInforme>[] nulo = new List<Modelo.TareaInforme>[3];
                    nulo[0] = new List<TareaInforme>();
                    nulo[1] = new List<TareaInforme>();
                    nulo[2] = new List<TareaInforme>();
                    return nulo;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
