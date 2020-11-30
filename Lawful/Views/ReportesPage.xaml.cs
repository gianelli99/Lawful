using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace Lawful.Views
{
    public sealed partial class ReportesPage : Page, INotifyPropertyChanged
    {
        private ReporteBL reporteBL;
        private UsuarioBL usuarioBL;
        public List<Usuario> Usuarios { get; set; }
        public List<AuditoriaUsuario> Auditoria { get; set; }
        public ReportesPage()
        {
            reporteBL = new ReporteBL();
            usuarioBL = new UsuarioBL();
            Usuarios = usuarioBL.Listar();
           
            InitializeComponent();
            LoadAuditoria();
            LoadSesionesPorUsuario();
            LoadSesionesPorGrupo();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void LoadSesionesPorUsuario()
        {
            if  (cbUsuarios.SelectedItem != null)
            {
                var sesiones = reporteBL.ObtenerUltimasSesiones(((Usuario)cbUsuarios.SelectedItem).ID);
                if (sesiones != null && sesiones.Count>0)
                {
                    (lcSesionesUsuario.Series[0] as LineSeries).ItemsSource = sesiones;
                    lcSesionesUsuario.Series[0].LegendItems.Clear();
                    ((LineSeries)lcSesionesUsuario.Series[0]).DependentRangeAxis = new LinearAxis()
                    {
                        Minimum = 0,
                        Orientation = AxisOrientation.Y,
                        Interval = 1,
                        ShowGridLines = true,
                    };
                    tbSesionesUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    lcSesionesUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    tbSesionesUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    lcSesionesUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }

            }
            else
            {
                tbSesionesUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
                lcSesionesUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }         
        }
        private void LoadSesionesPorGrupo()
        {
                var sesiones = reporteBL.ObtenerUltimasSesionesGrupos();
                if (sesiones != null && sesiones.Count > 0)
                {
                    (ccSesionesGrupos.Series[0] as ColumnSeries).ItemsSource = sesiones;
                    ccSesionesGrupos.Series[0].LegendItems.Clear();
                    ((ColumnSeries)ccSesionesGrupos.Series[0]).DependentRangeAxis = new LinearAxis()
                    {
                        Minimum = 0,
                        Orientation = AxisOrientation.Y,
                        Interval = 1,
                        ShowGridLines = true,
                    };
                    tbSesionesGrupo.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    ccSesionesGrupos.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    tbSesionesGrupo.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    ccSesionesGrupos.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
        }
        private void LoadAuditoria()
        {
            Auditoria= new List<AuditoriaUsuario>();
            

            if (cbUsuariosAudit.SelectedItem != null)
            {
                Auditoria = reporteBL.ObtenerAuditoria(((Usuario)cbUsuariosAudit.SelectedItem).ID);
            }
            dgUsuarioAuditoria.ItemsSource = null;
            dgUsuarioAuditoria.ItemsSource = Auditoria;

        }
        private void LoadChartContents()
        {

            var sesiones = reporteBL.ObtenerUltimasSesiones(1);
            Random rand = new Random();
            List<Records> records = new List<Records>();
            records.Add(new Records()
            {
                Name = "Suresh",
                Amount = rand.Next(0, 200)
            });
            records.Add(new Records()
            {
                Name = "Juancho",
                Amount = rand.Next(0, 200)
            });
            records.Add(new Records()
            {
                Name = "Sam",
                Amount = rand.Next(0, 200)
            });
            records.Add(new Records()
            {
                Name = "Sri",
                Amount = rand.Next(0, 200)
            });
            (PieChart.Series[0] as PieSeries).ItemsSource = records;
            
            (ColumnChart2.Series[0] as ColumnSeries).ItemsSource = records;
        }

        private void cbUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSesionesPorUsuario();
        }

        private void cbUsuariosAudit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAuditoria();
        }
    }
    public class Records
    {
        public string Name
        {
            get;
            set;
        }
        public int Amount
        {
            get;
            set;
        }
    }
}
