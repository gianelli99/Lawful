using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ObservableCollection<Tema> Temas { get; set; }
        private Tema _selectedTema;
        public Tema SelectedTema
        {
            get { return _selectedTema; }
            set { Set(ref _selectedTema, value); }
        }
        public TareasPage()
        {
            usuarioBL = new UsuarioBL();
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
    }
}
