using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using Lawful.Helpers;
using Lawful.Core.Models;
using Lawful.Core.Services;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Lawful.Core.Modelo;
using Lawful.Core.Logica;
using System.Threading.Tasks;

namespace Lawful.Views
{
    public sealed partial class UsuariosPage : Page, INotifyPropertyChanged
    {
        Accion accion;
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

            List<Accion> acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 1); 

            
            dgUsuarios.ItemsSource = usuarioBL.Listar();
            dgUsuarios.AutoGeneratingColumn += Grid_AutoGeneratingColumn;


            CreateCommandBar(AccionesBar, acciones);

            CreateGruposListView(LvGrupos,grupos);
        }

        private async void Accion_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                accion = (sender as AccionAppBarButton).Accion;
                if (dgUsuarios.SelectedItems.Count != 1 && accion.Descripcion != "Agregar Usuario")
                {
                    DisplayNoUserSelected();
                    return;
                }

                switch (accion.Descripcion)
                {
                    case "Agregar Usuario":
                        ClearFields();
                        FormularioUsuarioMode(false);

                        break;
                    case "Eliminar Usuario":
                        DisplayDeleteConfirmation();

                        break;
                    case "Modificar Usuario":
                        FormularioUsuarioMode(false);

                        user = usuarioBL.Consultar(((Usuario)dgUsuarios.SelectedItem).ID);
                        FillFormFields(user);

                        LvGrupos.Items.Clear();
                        CreateGruposListView(LvGrupos, grupos, user.Grupos);

                        break;
                    case "Consultar Usuario":
                        FormularioUsuarioMode(true);

                        user = usuarioBL.Consultar(((Usuario)dgUsuarios.SelectedItem).ID);
                        FillFormFields(user);

                        LvGrupos.Items.Clear();
                        CreateGruposListView(LvGrupos, grupos, user.Grupos);

                        break;
                    case "Cambiar Contraseña":
                        ClearFields();
                        user = usuarioBL.Consultar(((Usuario)dgUsuarios.SelectedItem).ID);
                        CambiarContrasenaMode();

                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                ContentDialog error = new ContentDialog
                {
                    Title = "Error",
                    Content = "Ocurrió un error inesperado, vuelva a intentarlo",
                    CloseButtonText = "Ok"
                };

                ContentDialogResult result = await error.ShowAsync();
                GridMode();
            }
        }

        private void FillFormFields(Usuario user)
        {
            txtUsername.Text = user.Username;
            txtPassword.IsEnabled = false;
            txtPassword.Password = user.Password;
            txtConfirmPassword.IsEnabled = false;
            txtConfirmPassword.Password = user.Password;
            txtEmail.Text = user.Email;
            txtNombre.Text = user.Nombre;
            txtApellido.Text = user.Apellido;
        }

        private void FillUserFields(Usuario user)
        {
            user.Username = txtUsername.Text;
            user.Password = txtPassword.Password;
            user.Email = txtEmail.Text;
            user.Nombre = txtNombre.Text;
            user.Apellido = txtApellido.Text;
            user.Estado = true;
        }

        private void ClearFields()
        {
            txtUsername.Text = "";
            txtPassword.IsEnabled = true;
            txtPassword.Password = "";
            txtConfirmPassword.IsEnabled = true;
            txtConfirmPassword.Password = "";
            txtEmail.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtPasswordCC.Password = "";
            txtConfirmPasswordCC.Password = "";
        }

        public List<Grupo> ObtainSelectedGroups(ListView listView)
        {
            var grupos = new List<Grupo>();
            foreach (GrupoListViewItem item in listView.Items)
            {
                if (item.IsSelected)
                {
                    grupos.Add(item.Grupo);
                }
            }
            return grupos;
        }

        private void Grid_AutoGeneratingColumn(object sender, Microsoft.Toolkit.Uwp.UI.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Password" || e.PropertyName == "Grupos" || e.PropertyName == "Estado")
            {
                e.Column.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private bool AreFieldsFilled()
        {
            if (String.IsNullOrWhiteSpace(txtUsername.Text) ||
                String.IsNullOrWhiteSpace(txtEmail.Text) ||
                String.IsNullOrWhiteSpace(txtNombre.Text) ||
                String.IsNullOrWhiteSpace(txtApellido.Text) ||
                String.IsNullOrWhiteSpace(txtPassword.Password) ||
                String.IsNullOrWhiteSpace(txtConfirmPassword.Password))
            {

                throw new Exception("Debe completar todos los campos");
            }
            else
            {
                return true;
            }
        }

        private bool ArePasswordsEqual(string pass1, string pass2)
        {
            if (pass1 != pass2)
            {
                throw new Exception("Las contraseñas deben ser iguales.");
            }
            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                throw new Exception("Debe ingresar un email válido");
            }
        }


        private void btnGuardar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                switch (accion.Descripcion)
                {
                    case "Agregar Usuario":
                        if (AreFieldsFilled() && ArePasswordsEqual(txtPassword.Password,txtConfirmPassword.Password) && IsValidEmail(txtEmail.Text))
                        {
                            user = new Usuario();
                            FillUserFields(user);
                            user.Grupos = ObtainSelectedGroups(LvGrupos);

                            usuarioBL.Insertar(user, SesionActiva.ObtenerInstancia().Usuario.ID);

                            dgUsuarios.ItemsSource = usuarioBL.Listar();
                            GridMode();
                        }

                        break;
                    case "Modificar Usuario":
                        if (AreFieldsFilled() && ArePasswordsEqual(txtPassword.Password, txtConfirmPassword.Password) && IsValidEmail(txtEmail.Text))
                        {
                            FillUserFields(user);
                            user.Grupos = ObtainSelectedGroups(LvGrupos);

                            usuarioBL.Modificar(user, SesionActiva.ObtenerInstancia().Usuario.ID, true);

                            dgUsuarios.ItemsSource = usuarioBL.Listar();
                            GridMode();
                        }

                        break;
                    case "Cambiar Contraseña":

                        if (ArePasswordsEqual(txtConfirmPasswordCC.Password, txtPasswordCC.Password))
                        {
                            user.Password = txtPasswordCC.Password;

                            usuarioBL.CambiarContrasena(user.Password, user.ID, SesionActiva.ObtenerInstancia().Usuario.ID, true);

                            GridMode();
                        }

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }

        private async void DisplayError(string errorM)
        {
            ContentDialog error = new ContentDialog
            {
                Title = "Error",
                Content = errorM,
                CloseButtonText = "Ok"
            };

            await error.ShowAsync();
        }

        private void btnCancelar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ClearFields();
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
                GrupoListViewItem item = new GrupoListViewItem();
                item.Name = grupo.ID.ToString();
                item.Content = grupo.Descripcion;
                item.Grupo = grupo;
                listView.Items.Add(item);
            }
        }

        private void CreateGruposListView(ListView listView, List<Grupo> Totalgrupos, List<Grupo> userGrupos)
        {
            foreach (var grupo in Totalgrupos)
            {
                GrupoListViewItem item = new GrupoListViewItem();
                item.Name = grupo.ID.ToString();
                item.Content = grupo.Descripcion;
                item.Grupo = grupo;
                if (userGrupos.Exists(x => x.ID == grupo.ID))
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

        private AppBarButton CreateFindAppBarButton() {
            AppBarButton buscar = new AppBarButton();
            buscar.Name = "abbBuscar";
            buscar.Label = "Buscar";
            buscar.Icon = new SymbolIcon(Symbol.Find);
            return buscar;
        }

        private CommandBar CreateCommandBar(CommandBar commandBar ,List<Accion> acciones) {
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
