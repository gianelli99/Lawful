using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Lawful.Core.Models;
using Lawful.Core.Services;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lawful.Views
{
    public sealed partial class UsuariosPage : Page, INotifyPropertyChanged
    {
        public UsuariosPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override  void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Core.Logica.UsuarioBL usuarioBL = new Core.Logica.UsuarioBL();
            Core.Datos.DAO.AccionDAO_SqlServer daoAcciones = new Core.Datos.DAO.AccionDAO_SqlServer();
            var acciones = daoAcciones.ListarPorVistaYUsuario(1, 1);
            var data =  usuarioBL.Listar();
            grid.ItemsSource = data;
            grid.AutoGeneratingColumn += Grid_AutoGeneratingColumn;
            AppBarButton buscar = new AppBarButton();
            buscar.Name = "abbBuscar";
            buscar.Label = "Buscar";
            buscar.Icon = new SymbolIcon(Symbol.Find);
            AccionesBar.PrimaryCommands.Add(buscar);
            AccionesBar.PrimaryCommands.Add(new AppBarSeparator());
            foreach (var accion in acciones)
            {
                AppBarButton nueva = new AppBarButton();
                nueva.Name = accion.ID.ToString();
                nueva.Label = accion.Descripcion;
                nueva.Icon = new SymbolIcon((Symbol) Enum.Parse(typeof(Symbol), accion.IconName));
                AccionesBar.PrimaryCommands.Add(nueva);
            }
            var grupos = usuarioBL.ListarGrupos();
            foreach (var grupo in grupos)
            {
                ListViewItem item = new ListViewItem();
                Grupos.Items.Add(grupo);
            }
        }

        private void Grid_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Password" || e.PropertyName == "Grupos")
            {
                e.Column.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }
    }
}
