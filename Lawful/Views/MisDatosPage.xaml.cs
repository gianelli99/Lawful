using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using Lawful.Helpers;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lawful.Views
{
    public sealed partial class MisDatosPage : Page
    {
        /*To Do:
            - Sacar el multi select del list view
            - Listar los grupos del usuario en ese list view
            - Optimizar :S
         */
        private Usuario usuarioLogueado;
        private UsuarioBL usuarioBL;
        private GrupoBL grupoBL;
        List<Grupo> grupos;
        public MisDatosPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int userId = SesionActiva.ObtenerInstancia().Usuario.ID;
            usuarioBL = new UsuarioBL();
            grupoBL = new GrupoBL();
            usuarioLogueado = usuarioBL.Consultar(userId);
            CambiarDatosMode();
            var accionesDisponibles = usuarioBL.ListarAccionesDisponiblesEnVista(userId, 7); //TODO: ¿Cambiar ese 7 por algo más dinámico?
            grupos = grupoBL.Listar();
            CreateCommandBar(cbAcciones, accionesDisponibles);
            CreateGruposListView(lvGrupos, grupos);
        }

        private string GenerateButtonName(string actionName)
        {
            string name = "";
            foreach (var item in actionName.Split()) { name += item; };
            return name;
        }

        private CommandBar CreateCommandBar(CommandBar commandBar, List<Accion> acciones)
        {
            foreach (var button in CreateAppBarButtons(acciones))
            {
                commandBar.PrimaryCommands.Add(button);
            }
            //Trace.WriteLine(commandBar.PrimaryCommands.Count);
            return commandBar;
        }

        private async void Accion_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Cuando cliqueo en cada acción ¿Qué hago?
            switch (((AccionAppBarButton)sender).Accion.Descripcion)
            {
                case "Modificar Mis Datos":
                    if (String.IsNullOrWhiteSpace(txtUsername.Text) ||
                        String.IsNullOrWhiteSpace(txtEmail.Text) ||
                        String.IsNullOrWhiteSpace(txtNombre.Text) ||
                        String.IsNullOrWhiteSpace(txtApellido.Text))
                    {
                        DisplayContentDialog("Debe completar todos los campos");
                        return;
                    }
                    try
                    {
                        var addr = new System.Net.Mail.MailAddress(txtEmail.Text);
                        if (addr.Address != txtEmail.Text)
                        {
                            DisplayContentDialog("El email no es válido");
                            return;
                        }
                    }
                    catch
                    {
                        DisplayContentDialog("El email no es válido");
                        return;
                    }

                    ContentDialog seguroDeModificar = new ContentDialog
                    {
                        Title = "Atención",
                        Content = "¿Está seguro que desea modificar sus datos?",
                        PrimaryButtonText = "Si",
                        SecondaryButtonText = "No"
                    };

                    ContentDialogResult result = await seguroDeModificar.ShowAsync();
                    if (result == ContentDialogResult.Primary)
                    {
                        usuarioLogueado.Username = txtUsername.Text;
                        usuarioLogueado.Email = txtEmail.Text;
                        usuarioLogueado.Nombre = txtNombre.Text;
                        usuarioLogueado.Apellido = txtApellido.Text;
                        usuarioBL.Modificar(usuarioLogueado, usuarioLogueado.ID, false);
                        DisplayContentDialog("Su perfil se ha actualizado correctamente", "Éxito");
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "Eliminar Mi Cuenta":
                    ContentDialog seguroDeEliminar = new ContentDialog
                    {
                        Title = "Atención",
                        Content = "¿Está seguro que desea dar de baja su cuenta?",
                        PrimaryButtonText = "Si",
                        SecondaryButtonText = "No"
                    };

                    ContentDialogResult resultEliminar = await seguroDeEliminar.ShowAsync();
                    if (resultEliminar == ContentDialogResult.Primary)
                    {
                        // Dar de baja
                        usuarioBL.Eliminar(usuarioLogueado.ID, usuarioLogueado.ID);
                        SesionBL.ObtenerInstancia().FinalizarSesion();
                        DisplayContentDialog("Su perfil se ha dado de baja correctamente", "Éxito");
                        Frame.Navigate(typeof(LoginPage));
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "Cambiar Mi Contraseña":
                    CambiarContraseñaMode();
                    break;
                default:
                    break;
            }
        }

        private List<AppBarButton> CreateAppBarButtons(List<Accion> acciones)
        {
            List<AppBarButton> appBarButtons = new List<AppBarButton>();
            foreach (var accion in acciones)
            {
                AccionAppBarButton appBarButton = new AccionAppBarButton();
                appBarButton.Name = $"btn{GenerateButtonName(accion.Descripcion)}";
                appBarButton.Label = accion.Descripcion;
                appBarButton.Icon = new SymbolIcon((Symbol)Enum.Parse(typeof(Symbol), accion.IconName));
                appBarButton.Accion = accion;
                appBarButton.Click += Accion_Click;
                appBarButtons.Add(appBarButton);
            }
            return appBarButtons;

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

        private void btnCancelar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CambiarMiContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void DisplayContentDialog(string msg, string title = "Error")
        {
            ContentDialog noUserSelected = new ContentDialog
            {
                Title = title,
                Content = msg,
                CloseButtonText = "Ok"
            };

            await noUserSelected.ShowAsync();
        }

        private void btnCambiarContraseña_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(txtCurrentPasswordCC.Password) ||
                String.IsNullOrWhiteSpace(txtConfirmPasswordCC.Password) ||
                String.IsNullOrWhiteSpace(txtPasswordCC.Password))
                {
                    DisplayContentDialog("Debe completar todos los campos");
                    return;
                }

                if (txtPasswordCC.Password != txtConfirmPasswordCC.Password)
                {
                    DisplayContentDialog("Las contraseñas no coinciden");
                    return;
                }

                if (!usuarioBL.ValidarContrasena(usuarioLogueado, txtCurrentPasswordCC.Password, txtPasswordCC.Password))
                {
                    DisplayContentDialog("La contraseña actual no es correcta o coincide con la nueva");
                    return;
                }

                usuarioBL.CambiarContrasena(txtPasswordCC.Password, usuarioLogueado.ID, usuarioLogueado.ID, false);
                txtConfirmPasswordCC.Password = "";
                txtCurrentPasswordCC.Password = "";
                txtPasswordCC.Password = "";
                usuarioLogueado = usuarioBL.Consultar(usuarioLogueado.ID);
                CambiarDatosMode();
            }
            catch (Exception ex)
            {
                DisplayContentDialog(ex.Message);
                return;
            }
        }

        private void CambiarContraseñaMode()
        {
            CambiarMiContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Visible;
            btnCambiarContraseña.Visibility = Windows.UI.Xaml.Visibility.Visible;
            btnCancelar.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }
        private void CambiarDatosMode()
        {
            txtApellido.Text = usuarioLogueado.Apellido;
            txtEmail.Text = usuarioLogueado.Email;
            txtNombre.Text = usuarioLogueado.Nombre;
            txtUsername.Text = usuarioLogueado.Username;
            CambiarMiContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            btnCambiarContraseña.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            btnCancelar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
