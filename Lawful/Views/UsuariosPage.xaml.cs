using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

using Lawful.Core.Models;
using Lawful.Core.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Lawful.Core.Modelo;
using Lawful.Core.Logica;

namespace Lawful.Views
{
    public sealed partial class UsuariosPage : Page, INotifyPropertyChanged
    {
        string accion;
        List<Grupo> grupos;
        UsuarioBL usuarioBL;
        Usuario user;

        public event PropertyChangedEventHandler PropertyChanged;

        public UsuariosPage()
        {
            InitializeComponent();
            GridMode();
            usuarioBL = new UsuarioBL();
        }

        protected override  void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            grupos = usuarioBL.ListarGrupos();

            Core.Datos.DAO.AccionDAO_SqlServer daoAcciones = new Core.Datos.DAO.AccionDAO_SqlServer(); // Nos falta implementar el método ListarPorVistaYUsuario en AccionBL jeje
            var acciones = daoAcciones.ListarPorVistaYUsuario(1, 1); // List<Accion>

            
            dgUsuarios.ItemsSource = usuarioBL.Listar();
            dgUsuarios.AutoGeneratingColumn += Grid_AutoGeneratingColumn;


            CreateCommandBar(AccionesBar, acciones);

            CreateGruposListView(LvGrupos,grupos);
        }

        private void Accion_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            accion = ((AppBarButton)sender).Name;
            if (dgUsuarios.SelectedItems.Count != 1 && accion != "1")
            {             
                DisplayNoUserSelected();
                return;
            }

            switch (accion)
            {
                case "1":
                    FormularioUsuarioMode(false);
                    
                    break;
                case "2":
                    DisplayDeleteConfirmation();

                    break;
                case "3":
                    FormularioUsuarioMode(false);

                    user = usuarioBL.Consultar(((Usuario)dgUsuarios.SelectedItem).ID);
                    FillUserFields(user);

                    LvGrupos.Items.Clear();
                    CreateGruposListView(LvGrupos, grupos, user.Grupos);

                    break;
                case "4":
                    FormularioUsuarioMode(true);

                    user = usuarioBL.Consultar(((Usuario)dgUsuarios.SelectedItem).ID);
                    FillUserFields(user);

                    LvGrupos.Items.Clear();
                    CreateGruposListView(LvGrupos, grupos, user.Grupos);

                    break;
                case "5":
                    CambiarContrasenaMode();
                    
                    break;
                default:
                    break;
            }
        }

        private void FillUserFields(Usuario user)
        {
            txtUsername.Text = user.Username;
            txtPassword.IsEnabled = false;
            txtConfirmPasswaord.IsEnabled = false;
            txtEmail.Text = user.Email;
            txtNombre.Text = user.Nombre;
            txtApellido.Text = user.Apellido;
        }

        private void Grid_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Password" || e.PropertyName == "Grupos" || e.PropertyName == "Estado")
            {
                e.Column.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void btnGuardar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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

                    //Validaciones
                    user = usuarioBL.Consultar(((Core.Modelo.Usuario)dgUsuarios.SelectedItem).ID);
                    user.Password = txtPasswordCC.Password;
                    try
                    {
                        usuarioBL.CambiarContrasena(user.Password, user.ID, Core.Modelo.SesionActiva.ObtenerInstancia().Usuario.ID, true);
                    }
                    catch (Exception ex)
                    {
                        ContentDialog error = new ContentDialog
                        {
                            Title = "Error",
                            Content = ex.Message,
                            CloseButtonText = "Ok"
                        };

                        ContentDialogResult result = await error.ShowAsync();
                    }
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
            GridMode();
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

        private void CreateGruposListView(ListView listView, List<Grupo> Totalgrupos)
        {
            foreach (var grupo in Totalgrupos)
            {
                ListViewItem item = new ListViewItem();
                item.Name = grupo.ID.ToString();
                item.Content = grupo.Descripcion;
                listView.Items.Add(item);
            }
        }

        private void CreateGruposListView(ListView listView, List<Grupo> Totalgrupos, List<Grupo> userGrupos)
        {
            foreach (var grupo in Totalgrupos)
            {
                ListViewItem item = new ListViewItem();
                item.Name = grupo.ID.ToString();
                item.Content = grupo.Descripcion;
                if (userGrupos.FindIndex(x => x.ID.ToString() == item.Name) != -1)
                {
                    item.IsSelected = true;
                }
                listView.Items.Add(item);
            }
        }

        private void GridMode()
        {
            FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            CambiarContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            dgUsuarios.MaxHeight = double.PositiveInfinity;
        }

        private void FormularioUsuarioMode(bool isConsultar) {
            FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
            CambiarContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
            dgUsuarios.MaxHeight = 100;
            GuardarEnabled(!isConsultar);
        }

        private void CambiarContrasenaMode() {
            FormularioUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            CambiarContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
            Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
            dgUsuarios.MaxHeight = 100;
            GuardarEnabled(true);
        }

        private void GuardarEnabled(bool isEnabled) {
            btnGuardar.IsEnabled = isEnabled;
        }

        private List<AppBarButton> CreateAppBarButtons(List<Accion> acciones) {
            List<AppBarButton> appBarButtons = new List<AppBarButton>();
            foreach (var accion in acciones)
            {
                AppBarButton appBarButton = new AppBarButton()
                {
                    Name = accion.ID.ToString(),
                    Label = accion.Descripcion,
                    Icon = new SymbolIcon((Symbol)Enum.Parse(typeof(Symbol), accion.IconName)),
                };
                appBarButton.Click += Accion_Click;
                appBarButtons.Add(appBarButton);
            }
            return appBarButtons;
            
        }

        private AppBarButton CreateFindAppBarButton() {
            AppBarButton buscar = new AppBarButton();
            buscar.Name = "abbBuscar";
            buscar.Label = "Buscar";
            buscar.Icon = new SymbolIcon(Symbol.Find);
            return buscar;
        }

        private CommandBar CreateCommandBar(CommandBar commandBar ,List<Accion> acciones) {
            //CommandBar commandBar = new CommandBar();
            commandBar.PrimaryCommands.Add(CreateFindAppBarButton());
            foreach (var button in CreateAppBarButtons(acciones))
            {
                commandBar.PrimaryCommands.Add(button);
            }
            Trace.WriteLine(commandBar.PrimaryCommands.Count);
            return commandBar;
        }
    }
}
