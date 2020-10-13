using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using Lawful.Helpers;
using Windows.UI.Xaml.Controls;

namespace Lawful.Views
{
    public sealed partial class GruposPage : Page, INotifyPropertyChanged
    {
        private GrupoBL grupoBL;
        private UsuarioBL usuarioBL;
        public GruposPage()
        {
            usuarioBL = new UsuarioBL();
            grupoBL = new GrupoBL();
            InitializeComponent();
            Grupos = grupoBL.Listar();
            Usuarios = usuarioBL.Listar();

            var acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 2);
            CreateCommandBar(AccionesBar, acciones);
        }
        public List<Grupo> Grupos { get; set; }
        public List<Usuario> Usuarios { get; set; }

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
        private void CreateGruposListView(ListView listView, List<Grupo> Totalgrupos)
        {
            foreach (var grupo in Totalgrupos)
            {
                GrupoListViewItem item = new GrupoListViewItem();
                item.Name = grupo.ID.ToString();
                item.Content = grupo.Descripcion;
                item.Grupo = grupo;
                listView.Items.Add(item);
            }
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

        private void Accion_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private AppBarButton CreateFindAppBarButton()
        {
            AppBarButton buscar = new AppBarButton();
            buscar.Name = "abbBuscar";
            buscar.Label = "Buscar";
            buscar.Icon = new SymbolIcon(Symbol.Find);
            return buscar;
        }

        private CommandBar CreateCommandBar(CommandBar commandBar, List<Accion> acciones)
        {
            commandBar.PrimaryCommands.Add(CreateFindAppBarButton());
            foreach (var button in CreateAppBarButtons(acciones))
            {
                commandBar.PrimaryCommands.Add(button);
            }
            return commandBar;
        }
    }
}
