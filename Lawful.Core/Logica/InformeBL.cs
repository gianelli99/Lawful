using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;

namespace Lawful.Core.Logica
{
    public class InformeBL
    {
        private Datos.Interfaces.ISesionDAO sesionDAO;
        private Datos.Interfaces.IUsuarioDAO usuarioDAO;
        private Datos.Interfaces.IGrupoDAO grupoDAO;
        public InformeBL()
        {
            sesionDAO = new Datos.DAO.SesionDAO_SqlServer();
            usuarioDAO = new Datos.DAO.UsuarioDAO_SqlServer();
            grupoDAO = new Datos.DAO.GrupoDAO_SqlServer();
        }
        public List<Modelo.Usuario> ListarUsuarios()
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
        public void GenerarInforme(DateTime fechaDesde, DateTime fechaHasta)//todos
        {
            try
            {
                fechaDesde = fechaDesde.Date;
                fechaHasta = fechaHasta.Date;

                List<Modelo.Sesion> sesiones = new List<Modelo.Sesion>();
                sesiones = sesionDAO.Listar(fechaDesde, fechaHasta);
                if (sesiones.Count> 0)
                {
                    GenerarExcel(TipoInforme.Todos, sesiones, fechaDesde, fechaHasta);
                }
                else
                {
                    throw new Exception("No se han encontrado sesiones.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void GenerarInforme(TipoInforme tipoInforme, int id, DateTime fechaDesde, DateTime fechaHasta)// usuario y grupo
        {
            try
            {
                fechaDesde = fechaDesde.Date;
                fechaHasta = fechaHasta.Date;

                List<Modelo.Sesion> sesiones = new List<Modelo.Sesion>();
                if (tipoInforme == TipoInforme.Grupo)
                {
                    sesiones = sesionDAO.ListarPorGrupo(id, fechaDesde, fechaHasta);
                }
                else
                {
                    sesiones = sesionDAO.ListarPorUsuario(id, fechaDesde, fechaHasta);
                }
                if (sesiones.Count>0)
                {
                    GenerarExcel(tipoInforme, sesiones, fechaDesde, fechaHasta);
                }
                else
                {
                    throw new Exception("No se han encontrado sesiones.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void GenerarExcel(TipoInforme tipoInforme, List<Modelo.Sesion> sesiones, DateTime fechaDesde, DateTime fechaHasta)
        {
            fechaDesde = fechaDesde.Date;
            fechaHasta = fechaHasta.Date;

            Excel.Application oXL = null;
            Excel._Workbook oWB = null;
            Excel._Worksheet oSheet = null;
            Excel.Range oRng = null;
            try
            {
                oXL = new Excel.Application();
                oXL.UserControl = false;
                oXL.Visible = true;

                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                string titulo = "";
                switch (tipoInforme)
                {
                    case TipoInforme.Todos:
                        titulo = $"Informe de Todas las sesiones desde {fechaDesde} hasta {fechaHasta}";
                        break;
                    case TipoInforme.Usuario:
                        titulo = $"Informe de Todas las sesiones del Usuario {sesiones[0].Usuario.Username} desde {fechaDesde} hasta {fechaHasta}";
                        break;
                    case TipoInforme.Grupo:
                        titulo = $"Informe de Todas las sesiones del Grupo {sesiones[0].Usuario.Grupos[0].Descripcion} desde {fechaDesde} hasta {fechaHasta}";
                        break;
                    default:
                        break;
                }
                oSheet.Cells[1, 1] = titulo;

                oSheet.Cells[3, 1] = "ID Sesión";
                oSheet.Cells[3, 2] = "Username";
                oSheet.Cells[3, 3] = "Fecha LogIn";
                oSheet.Cells[3, 4] = "Fecha LogOut";
                oSheet.Cells[3, 5] = "Tiempo de Sesión";


                string[,] ids = new string[sesiones.Count,5];
                for (int i = 0; i < sesiones.Count; i++)
                {
                    ids[i, 0] = sesiones[i].ID.ToString();
                    ids[i, 1] = sesiones[i].Usuario.Username;
                    ids[i, 2] = sesiones[i].LogIn.ToString();
                    ids[i, 3] = sesiones[i].LogOut.ToString();
                    ids[i, 4] = sesiones[i].CalcularTiempoSesion().ToString(@"hh\:mm\:ss");
                    
                }
                for (int i = 4; i <= sesiones.Count+3; i++)
                {
                    if (i % 2 != 0)
                    {
                        oSheet.get_Range("A" + i, "E" + i).Interior.Color =Excel.XlRgbColor.rgbLightSkyBlue;
                    }
                }
                oSheet.get_Range("A4", "E" + (sesiones.Count+3).ToString()).Value2 = ids;
                oRng = oSheet.get_Range("B1", "E1");
                oRng.EntireColumn.AutoFit();

                oSheet.get_Range("A3").EntireRow.Font.Bold = true;
                oSheet.get_Range("A1").EntireRow.Font.Size = 14;

                oSheet.get_Range("A3", "E3").Interior.Color = Excel.XlRgbColor.rgbBlue;

                if (tipoInforme == TipoInforme.Usuario)
                {
                    oSheet.get_Range("B1").EntireColumn.Delete();
                }
                oXL.Visible = true;
                oXL.UserControl = true;
                Marshal.ReleaseComObject(oSheet);
                Marshal.ReleaseComObject(oWB);
                Marshal.ReleaseComObject(oXL);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
