using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
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
        private TemaBL temaBL;
        public List<Usuario> Usuarios { get; set; }
        public List<Tema> Temas { get; set; }
        public List<AuditoriaUsuario> Auditoria { get; set; }
        public List<IniciativaInforme> Iniciativas { get; set; }
        public List<ParticipacionInforme> Participacion { get; set; }
        public ReportesPage()
        {
            reporteBL = new ReporteBL();
            usuarioBL = new UsuarioBL();
            temaBL = new TemaBL();
            Usuarios = usuarioBL.Listar();
            Temas = temaBL.Listar();
            
            InitializeComponent();
            LoadAuditoria();
            LoadSesionesPorUsuario();
            LoadSesionesPorGrupo();
            LoadIniciativas();
            LoadParticipacion();
            LoadTareas();
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
        private void LoadTareas()
        {
            if (cbTemaTarea.SelectedItem != null)
            {
                var tareas = reporteBL.ObtenerEstadoTareasPorDia(((Tema)cbTemaTarea.SelectedItem).ID);
                if (tareas != null && tareas[0].Count > 0)
                {
                    (lcTareas.Series[0] as LineSeries).ItemsSource = tareas[0];
                    (lcTareas.Series[1] as LineSeries).ItemsSource = tareas[1];
                    (lcTareas.Series[2] as LineSeries).ItemsSource = tareas[2];

                    tbTareas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    lcTareas.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    tbTareas.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    lcTareas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
            else
            {
                tbTareas.Visibility = Windows.UI.Xaml.Visibility.Visible;
                lcTareas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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
        private void LoadIniciativas()
        {
            Iniciativas = new List<IniciativaInforme>();


            if (cbTema.SelectedItem != null)
            {
                Iniciativas = reporteBL.ObtenerCantIniciativas(((Tema)cbTema.SelectedItem).ID);
                (pcIniciativas.Series[0] as PieSeries).ItemsSource = Iniciativas;
            }

            if (cbTema.SelectedItem == null || Iniciativas == null || Iniciativas.Count <= 0 )
            {
                tbIniciativasTema.Visibility = Windows.UI.Xaml.Visibility.Visible;
                pcIniciativas.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                tbIniciativasTema.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                pcIniciativas.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }


        }
        private void LoadParticipacion()
        {
            Participacion = new List<ParticipacionInforme>();

            Participacion = reporteBL.ObtenerParticipacionTemas();
            if (Participacion != null && Participacion.Count>0)
            {
                (ccParticipacion.Series[0] as ColumnSeries).ItemsSource = Participacion;
                ccParticipacion.Series[0].LegendItems.Clear();
                tbIniciativasParticipacion.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                ccParticipacion.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                tbIniciativasParticipacion.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ccParticipacion.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void cbUsuarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadSesionesPorUsuario();
        }

        private void cbUsuariosAudit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAuditoria();
        }

        private void cbTema_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadIniciativas();
        }

        private void cbTemaTarea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTareas();
        }
    }
    
}
