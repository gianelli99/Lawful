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
        // TODO: Optimizar

        Usuario usuarioLogueado;
        UsuarioBL usuarioBL;
        GrupoBL grupoBL;


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
            FillUserFormData(usuarioLogueado);
            CreateCommandBar(cbAcciones, usuarioBL.ListarAccionesDisponiblesEnVista(userId, 7));
            CreateGruposListView(lvGrupos, usuarioLogueado.Grupos);
            
        }

        private void FillUserFormData(Usuario usuarioLogueado)
        {
            txtApellido.Text = usuarioLogueado.Apellido;
            txtEmail.Text = usuarioLogueado.Email;
            txtNombre.Text = usuarioLogueado.Nombre;
            txtUsername.Text = usuarioLogueado.Username;
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
            return commandBar;
        }

        private async void Accion_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            switch (((AccionAppBarButton)sender).Accion.Descripcion)
            {

                case "Modificar Mis Datos":
                    CambiarDatosMode();
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
                        CambiarDatosMode();
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
                        //DisplayContentDialog("Su perfil se ha dado de baja correctamente", "Éxito");
                        //FrameGlobal.FrameEstatico.Navigate(typeof(LoginPage));

                        //Frame frame = FrameGlobal.ObtenerInstancia().UnicoFrame;
                        //FrameGlobal fg = FrameGlobal.ObtenerInstancia();
                        FrameGlobal.FrameEstatico.Navigate(typeof(LoginPage));
                        //frame.Navigate(typeof(LoginPage));

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
                AccionAppBarButton appBarButton = new AccionAppBarButton
                {
                    Name = $"btn{GenerateButtonName(accion.Descripcion)}",
                    Label = accion.Descripcion,
                    Icon = new SymbolIcon((Symbol)Enum.Parse(typeof(Symbol), accion.IconName)),
                    Accion = accion
                };
                appBarButton.Click += Accion_Click;
                appBarButtons.Add(appBarButton);
            }
            return appBarButtons;

        }

        private void CreateGruposListView(ListView listView, List<Grupo> Totalgrupos)
        {
            foreach (var grupo in Totalgrupos)
            {
                GrupoListViewItem item = new GrupoListViewItem
                {
                    Name = grupo.ID.ToString(),
                    Content = grupo.Descripcion,
                    Grupo = grupo,
                    FontWeight = new Windows.UI.Text.FontWeight { Weight = 600 },

                };
                listView.Items.Add(item);
            }
        }

        private void BtnCancelar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CambiarDatosMode();
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

        private void BtnCambiarContraseña_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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
                usuarioLogueado = usuarioBL.Consultar(usuarioLogueado.ID);
                DisplayContentDialog("Su contraseña se ha modificado correctamente", "Éxito 😊");
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
            spButtons.Visibility = Windows.UI.Xaml.Visibility.Visible;
            spUserFormGroupsContainer.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
        private void CambiarDatosMode()
        {
            txtPasswordCC.Password = "";
            txtConfirmPasswordCC.Password = "";
            txtCurrentPasswordCC.Password = "";

            spUserFormGroupsContainer.Visibility = Windows.UI.Xaml.Visibility.Visible;
            CambiarMiContraseñaUsuario.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            spButtons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

    }
}
