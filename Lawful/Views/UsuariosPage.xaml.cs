using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Lawful.Core.Models;
using Lawful.Core.Services;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lawful.Views
{
    public sealed partial class UsuariosPage : Page
    {
        string accion;
        List<Core.Modelo.Grupo> grupos;
        Core.Logica.UsuarioBL usuarioBL;
        public UsuariosPage()
        {
            InitializeComponent();
        }

        protected override  void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            usuarioBL = new Core.Logica.UsuarioBL();
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
                nueva.Click += Accion_Click;
                AccionesBar.PrimaryCommands.Add(nueva);
            }
            grupos = usuarioBL.ListarGrupos();
            foreach (var grupo in grupos)
            {
                ListViewItem item = new ListViewItem();
                item.Name = grupo.ID.ToString();
                item.Content = grupo.Descripcion;
                LvGrupos.Items.Add(item);
            }
        }

        private void Accion_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            switch (((AppBarButton)sender).Name)
            {
                case "1":
                    FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    grid.MaxHeight = 100;
                    accion = "1";
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                default:
                    break;
            }
        }

        private void Grid_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Password" || e.PropertyName == "Grupos")
            {
                e.Column.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void btnGuardar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Core.Modelo.Usuario user;
            switch (accion)
            {
                case "1":
                    user = new Core.Modelo.Usuario();
                    user.Username = txtUsername.Text;
                    user.Password = txtPassword.Password;
                    user.Email = txtEmail.Text;
                    user.Nombre = txtNombre.Text;
                    user.Apellido = txtApellido.Text;
                    user.Estado = true;

                    foreach (ListViewItem item in LvGrupos.Items)
                    {
                        if (item.IsSelected)
                        {
                            foreach (var grupo in grupos)
                            {
                                if (item.Name == grupo.ID.ToString())
                                {
                                    user.Grupos.Add(grupo);
                                    break;
                                }
                            }
                        }
                    }
                    usuarioBL.Insertar(user, Core.Modelo.SesionActiva.ObtenerInstancia().Usuario.ID);
                    grid.ItemsSource = usuarioBL.Listar();
                    FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    grid.MaxHeight = 500;
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                default:
                    break;
            }
        }
    }
}
