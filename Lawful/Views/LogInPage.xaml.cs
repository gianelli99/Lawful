using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Lawful.Views
{
    public sealed partial class LoginPage : Page
    {
        Core.Logica.SesionBL sesionBL;

        public LoginPage()
        {
            this.InitializeComponent();
            sesionBL = Core.Logica.SesionBL.ObtenerInstancia();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrWhiteSpace(txtUsername.Text) || String.IsNullOrWhiteSpace(txtPassword.Password))
            {
                lblError.Text = "Debe completar todos los campos";
                return;
            }
            try
            {
                int userId = sesionBL.ValidarUsuario(txtUsername.Text, txtPassword.Password);
                if (userId != -1)
                {

                    var sesion = Core.Modelo.SesionActiva.ObtenerInstancia();
                    sesion.Usuario = sesionBL.ConsultarUsuario(userId);
                    sesion.LogIn = DateTime.Now;
                    sesionBL.IniciarSesion();
                    if (sesionBL.NeedNewPassword(userId))
                    {
                        var parameters = new ClassParameters.CambiarContrasenaParameters(sesion.Usuario.ID, true);
                        this.Frame.Navigate(typeof(CambiarContrasenaPage),parameters);
                        return;
                    }
                    else
                    {
                        this.Frame.Navigate(typeof(ShellPage));
                    }

                }
                else
                {
                    lblError.Text = "No se ha encontrado un usuario con los datos especificados.";
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void hlbOlvidasteTuContrasena_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RecuperarContrasenaPage));
        }
    }
}
