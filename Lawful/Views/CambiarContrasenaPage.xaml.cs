using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Lawful.Core;

namespace Lawful.Views
{
    public sealed partial class CambiarContrasenaPage : Page
    {
        private Core.Logica.UsuarioBL usuarioBL;
        private Core.Modelo.Usuario usuario;
        private bool necesitaContrActual;
        int userId;
        public CambiarContrasenaPage()
        {
            InitializeComponent();
            usuarioBL = new Core.Logica.UsuarioBL();
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parametros = (ClassParameters.CambiarContrasenaParameters)e.Parameter;
            necesitaContrActual = parametros.NeedCurrentPassword;
            userId = parametros.UserID;
            usuario = usuarioBL.Consultar(userId);
            if (!necesitaContrActual)
            {
                txtCurrentPassword.IsEnabled = false;
            }
            base.OnNavigatedTo(e);
        }

        private void btnEnviarCorreo_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (necesitaContrActual && String.IsNullOrWhiteSpace(txtCurrentPassword.Password) ||
                String.IsNullOrWhiteSpace(txtNewPassword.Password) ||
                String.IsNullOrWhiteSpace(txtConfirmNewPassword.Password))
            {
                showUIContentDialog("Debe completar todos los campos", false);
                return;
            }

            if (txtNewPassword.Password != txtConfirmNewPassword.Password)
            {
                showUIContentDialog("Las contraseñas no coinciden", false);
                return;
            }

            if (necesitaContrActual)
            {
                if (!usuarioBL.ValidarContrasena(usuario, txtCurrentPassword.Password, txtNewPassword.Password))
                {
                    showUIContentDialog("La contraseña actual no es correcta o coincide con la nueva", false);
                    return;
                }
            }

            try
            {
                bool needNewPass = !necesitaContrActual;
                usuarioBL.CambiarContrasena(txtNewPassword.Password, userId, Core.Modelo.SesionActiva.ObtenerInstancia().Usuario.ID, needNewPass);
                this.Frame.Navigate(typeof(LoginPage));
            }
            catch (Exception ex)
            {

                showUIContentDialog(ex.Message, false);
                return;
            }

        }
        private async void showUIContentDialog(string msg, bool passwordChangedSuccessfully)
        {
            ContentDialog error = new ContentDialog
            {
                Title = passwordChangedSuccessfully ? "Contraseña cambiada con éxito" : "Error",
                Content = msg,
                CloseButtonText = "Ok"
            };

            await error.ShowAsync();
        }
    }
}
