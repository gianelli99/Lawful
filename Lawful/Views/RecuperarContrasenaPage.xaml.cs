using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;
using System.Net.Mail;
using Lawful.Core;

namespace Lawful.Views
{
    public sealed partial class RecuperarContrasenaPage : Page
    {
        Core.Logica.UsuarioBL usuarioBL;
        public RecuperarContrasenaPage()
        {
            InitializeComponent();
            usuarioBL = new Core.Logica.UsuarioBL();
        }


        private void btnVolver_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        private void btnEnviarCorreo_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(txtUsername.Text) || String.IsNullOrWhiteSpace(txtEmail.Text))
            {
                showUIContentDialog("Debe completar todos los campos", false);
                return;
            }
            try
            {
                var addr = new MailAddress(txtEmail.Text);
                if (addr.Address != txtEmail.Text)
                {
                    showUIContentDialog("El email no es válido", false);
                    return;
                }
            }
            catch
            {

                showUIContentDialog("El email no es válido", false);
                return;
            }
            try
            {
                Core.Modelo.Usuario usuario = usuarioBL.Consultar(txtUsername.Text, txtEmail.Text);
                if (usuario == null)
                {
                    showUIContentDialog("Los datos ingresados son incorrectos", false);
                    return;
                }

                string passDES = Core.Logica.Hasheo.RandomString(6, true);
                usuario.Password = Core.Logica.Hasheo.GenerarContrasena(passDES);
                usuarioBL.CambiarContrasena(passDES, usuario.ID, usuario.ID, true);
                usuarioBL.EnviarEmail(passDES, usuario.Email);

                showUIContentDialog("Revise su correo electrónico para conocer su nueva contraseña",true);
            }
            catch (Exception)
            {
                showUIContentDialog("Ha ocurrido un error", false);
            }

            this.Frame.Navigate(typeof(LoginPage));
        }
        private async void showUIContentDialog(string err, bool emailSentSuccessfully)
        {
            ContentDialog error = new ContentDialog
            {
                Title = emailSentSuccessfully ? "Envío completado correctamente" : "Error",
                Content = err,
                CloseButtonText = "Ok"
            };

            await error.ShowAsync();
        }
    
    }
}
