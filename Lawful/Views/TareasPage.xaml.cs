using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using Lawful.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lawful.Views
{
    public sealed partial class TareasPage : Page, INotifyPropertyChanged
    {
        private UsuarioBL usuarioBL;
        private TareaBL tareaBL;
        public ObservableCollection<Tema> Temas { get; set; }
        public ObservableCollection<Tarea> TareasPorHacer { get; set; }
        public ObservableCollection<Tarea> TareasEnCurso { get; set; }
        public ObservableCollection<Tarea> TareasFinalizadas { get; set; }
        private Tema _selectedTema;
        private Tarea _selectedTarea;
        public Tema SelectedTema
        {
            get { return _selectedTema; }
            set { Set(ref _selectedTema, value); }
        }
        public Tarea SelectedTarea
        {
            get { return _selectedTarea; }
            set { Set(ref _selectedTarea, value); }
        }
        public TareasPage()
        {
            usuarioBL = new UsuarioBL();
            tareaBL = new TareaBL();
            TareasPorHacer = new ObservableCollection<Tarea>();
            TareasEnCurso = new ObservableCollection<Tarea>();
            TareasFinalizadas = new ObservableCollection<Tarea>();
            InitializeComponent();

            RefreshTemasListView();

            var acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 10);
            CreateCommandBar(this.AccionesBar, acciones);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            if (value.GetType().Name == "Tema")
            {
                var tareas = tareaBL.ListarPorTema(_selectedTema.ID);
                TareasPorHacer.Clear();
                TareasEnCurso.Clear();
                TareasFinalizadas.Clear();
                foreach (var item in tareas)
                {
                    if (item.Estado.GetType().Name == "PorHacer")
                    {
                        TareasPorHacer.Add(item);
                    }
                    else if (item.Estado.GetType().Name == "EnCurso")
                    {
                        TareasEnCurso.Add(item);
                    }
                    else
                    {
                        TareasFinalizadas.Add(item);
                    }
                }
            }
            

            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private void RefreshTemasListView()
        {
            Temas = new ObservableCollection<Tema>();
            var data = usuarioBL.ListarTemasDisponibles(SesionActiva.ObtenerInstancia().Usuario.ID);
            if (data != null)
            {
                foreach (var item in data)
                {
                    Temas.Add(item);
                }
                if (Temas.Count > 0)
                {
                    SelectedTema = Temas[0];
                }
            }
            OnPropertyChanged("Temas");
        }
        private CommandBar CreateCommandBar(CommandBar commandBar, List<Accion> acciones)
        {
            foreach (var button in CreateAppBarButtons(acciones))
            {
                commandBar.PrimaryCommands.Add(button);
            }
            return commandBar;
        }
        private List<AppBarButton> CreateAppBarButtons(List<Accion> acciones)
        {
            List<AppBarButton> appBarButtons = new List<AppBarButton>();
            foreach (var accion in acciones)
            {
                AccionAppBarButton appBarButton = new AccionAppBarButton()
                {
                    Name = accion.ID.ToString(),
                    Label = accion.Descripcion,
                    Icon = new SymbolIcon((Symbol)Enum.Parse(typeof(Symbol), accion.IconName)),
                    Accion = accion
                };
                appBarButton.Click += Accion_Click;
                appBarButtons.Add(appBarButton);
            }
            return appBarButtons;

        }

        private void Accion_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void txtComentario_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {

        }

        private void lvPorHacer_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;

        }

        private void lvPorHacer_Drop(object sender, DragEventArgs e)
        {
        }
        private void lvPorHacer_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            Trace.WriteLine(((StackPanel)sender).Name);
        }
    }
}
