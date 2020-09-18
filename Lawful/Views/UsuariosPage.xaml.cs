using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Lawful.Core.Models;
using Lawful.Core.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

namespace Lawful.Views
{
    public sealed partial class UsuariosPage : Page, INotifyPropertyChanged
    {
        string accion;
        List<Core.Modelo.Grupo> grupos;
        Core.Logica.UsuarioBL usuarioBL;

        public event PropertyChangedEventHandler PropertyChanged;

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
            dgUsuarios.ItemsSource = data;
            dgUsuarios.AutoGeneratingColumn += Grid_AutoGeneratingColumn;
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
            Core.Modelo.Usuario userpadreee;
            accion = ((AppBarButton)sender).Name;
            if (dgUsuarios.SelectedItems.Count != 1 && accion != "1")
            {
                DisplayNoUserSelected();
                return;
            }

            switch (((AppBarButton)sender).Name)
            {
                case "1":
                    FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    CambiarContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    btnGuardar.IsEnabled = true;
                    dgUsuarios.MaxHeight = 100;
                    accion = "1";                   
                    break;
                case "2":
                    DisplayDeleteConfirmation();
                    break;
                case "3":
                    FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    CambiarContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    btnGuardar.IsEnabled = true;
                    dgUsuarios.MaxHeight = 100;
                    accion = "3";

                    userpadreee = usuarioBL.Consultar(((Core.Modelo.Usuario)dgUsuarios.SelectedItem).ID);
                    txtUsername.Text = userpadreee.Username;
                    txtPassword.IsEnabled = false;
                    txtConfirmPasswaord.IsEnabled = false;
                    txtEmail.Text = userpadreee.Email;
                    txtNombre.Text = userpadreee.Nombre;
                    txtApellido.Text = userpadreee.Apellido;

                    LvGrupos.Items.Clear();
                    foreach (var grupo in grupos)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Name = grupo.ID.ToString();
                        item.Content = grupo.Descripcion;
                        if (userpadreee.Grupos.FindIndex(x=>x.ID.ToString() == item.Name) != -1)
                        {
                            item.IsSelected = true;
                        }
                        LvGrupos.Items.Add(item);
                    }

                    break;
                case "4":
                    FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    CambiarContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    btnGuardar.IsEnabled = false;
                    dgUsuarios.MaxHeight = 100;
                    accion = "4";
                    userpadreee = usuarioBL.Consultar(((Core.Modelo.Usuario)dgUsuarios.SelectedItem).ID);
                    txtUsername.Text = userpadreee.Username;
                    txtPassword.IsEnabled = false;
                    txtConfirmPasswaord.IsEnabled = false;
                    txtEmail.Text = userpadreee.Email;
                    txtNombre.Text = userpadreee.Nombre;
                    txtApellido.Text = userpadreee.Apellido;

                    LvGrupos.Items.Clear();
                    foreach (var grupo in grupos)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Name = grupo.ID.ToString();
                        item.Content = grupo.Descripcion;
                        if (userpadreee.Grupos.FindIndex(x => x.ID.ToString() == item.Name) != -1)
                        {
                            item.IsSelected = true;
                        }
                        LvGrupos.Items.Add(item);
                    }
                    break;
                case "5":
                    FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    CambiarContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    dgUsuarios.MaxHeight = 100;
                    accion = "4";
                    break;
                default:
                    break;
            }
        }

        private void Grid_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Password" || e.PropertyName == "Grupos" || e.PropertyName == "Estado")
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
                    dgUsuarios.ItemsSource = usuarioBL.Listar();
                    FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    dgUsuarios.MaxHeight = double.PositiveInfinity;
                    break;
                case "3":
                    user = new Core.Modelo.Usuario();
                    user.ID = usuarioBL.Consultar(((Core.Modelo.Usuario)dgUsuarios.SelectedItem).ID).ID;
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
                    usuarioBL.Modificar(user, Core.Modelo.SesionActiva.ObtenerInstancia().Usuario.ID,true);
                    dgUsuarios.ItemsSource = usuarioBL.Listar();
                    FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    dgUsuarios.MaxHeight = double.PositiveInfinity;
                    break;
                case "4":
                    FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    dgUsuarios.MaxHeight = double.PositiveInfinity;
                    break;
                case "5":
                    CambiarContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    dgUsuarios.MaxHeight = double.PositiveInfinity;
                    break;
                default:
                    break;
            }
        }

        private void btnCancelar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            CambiarContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            dgUsuarios.MaxHeight = double.PositiveInfinity;
        }
        private async void DisplayNoUserSelected()
        {
            ContentDialog noUserSelected = new ContentDialog
            {
                Title = "Usuario no seleccionado",
                Content = "Seleccione uno e intente de nuevo.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noUserSelected.ShowAsync();
        }
        private async void DisplayDeleteConfirmation()
        {
            ContentDialog noUserSelected = new ContentDialog
            {
                Title = "Atención",
                Content = "¿Está seguro que desea eliminarlo?",
                PrimaryButtonText = "Si",
                SecondaryButtonText = "No"
            };

            ContentDialogResult result = await noUserSelected.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var user = usuarioBL.Consultar(((Core.Modelo.Usuario)dgUsuarios.SelectedItem).ID);
                usuarioBL.Eliminar(user.ID,1);
                dgUsuarios.ItemsSource = usuarioBL.Listar();
            }
        }
    }
}
