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
        //public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on UsuariosPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public UsuariosPage()
        {
            InitializeComponent();
        }

        protected override  void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //Source.Clear();
            Core.Logica.UsuarioBL usuarioBL = new Core.Logica.UsuarioBL();
            Core.Datos.DAO.AccionDAO_SqlServer daoAcciones = new Core.Datos.DAO.AccionDAO_SqlServer();
            var acciones = daoAcciones.ListarPorVistaYUsuario(1, 1);
            // TODO WTS: Replace this with your actual data
            var data =  usuarioBL.Listar();
            grid.ItemsSource = data;
            grid.AutoGeneratingColumn += Grid_AutoGeneratingColumn;
            //foreach (var item in data)
            //{
            //    Source.Add(item);
            //}
            foreach (var accion in acciones)
            {
                AppBarButton nueva = new AppBarButton();
                nueva.Name = accion.ID.ToString();
                nueva.Label = accion.Descripcion;
                nueva.Icon = new SymbolIcon((Symbol) Enum.Parse(typeof(Symbol), accion.IconName));
                AccionesBar.PrimaryCommands.Add(nueva);
            }
        }

        private void Grid_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Password" || e.PropertyName == "Grupos")
            {
                e.Column.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
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
    }
}
