using System;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Lawful.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        Core.Logica.SesionBL sesionBL;

        public Login()
        {
            this.InitializeComponent();
            sesionBL = Core.Logica.SesionBL.ObtenerInstancia();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtUsername.Text) || String.IsNullOrWhiteSpace(txtPassword.Password))
            {
                lblError.Text = "Debe completar todos los campos";
                txtUsername.Text = "Hola";
                return;
            }
            try
            {
                int userId = sesionBL.ValidarUsuario(txtUsername.Text, txtPassword.Password);
                if (userId != -1)
                {
                    //sesionBL.Suscribir(this);

                    var sesion = Core.Modelo.SesionActiva.ObtenerInstancia();
                    sesion.Usuario = sesionBL.ConsultarUsuario(userId);
                    sesion.LogIn = DateTime.Now;
                    sesionBL.IniciarSesion();

                    //if (sesionBL.NeedNewPassword(userId))
                    //{
                    //    frmCambiarContrasena cContrasena = new frmCambiarContrasena(sesion.Usuario.ID, true);
                    //    cContrasena.ShowDialog();
                    //    sesion.Usuario = sesionBL.ConsultarUsuario(userId);
                    //}

                    //frmInicio inicio = new frmInicio();
                    //this.Hide();
                    //DialogResult result = inicio.ShowDialog();
                }
                else
                {
                    lblError.Text = "Ha ocurrido un error :(";
                    txtUsername.Text = "Hola";
                    return;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
