using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Lawful.Core.Modelo;
using Windows.ApplicationModel.VoiceCommands;
using System.Collections.Generic;
using Lawful.Helpers;
using Lawful.Core.Logica;
using System.Diagnostics;

namespace Lawful.Views
{
    public sealed partial class TemasDetailControl : UserControl
    {
        UsuarioBL usuarioBL;
        Accion accion;
        TemaBL temaBL;
        public Tema MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as Tema; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(Tema), typeof(TemasDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public TemasDetailControl()
        {
            usuarioBL = new UsuarioBL();
            temaBL = new TemaBL();
            InitializeComponent();
            var acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 8);
            CreateCommandBar(AccionesBar, acciones);
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TemasDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
        private void FormularioMode()
        {
            spFormularioTema.Visibility = Visibility.Visible;
            tbTitulo.Visibility = Visibility.Collapsed;
            spDetails.Visibility = Visibility.Collapsed;
        }
        private void TemaMode()
        {
            spFormularioTema.Visibility = Visibility.Collapsed;
            tbTitulo.Visibility = Visibility.Visible;
            spDetails.Visibility = Visibility.Visible;
        }
        private CommandBar CreateCommandBar(CommandBar commandBar, List<Accion> acciones)
        {
            foreach (var button in CreateAppBarButtons(acciones))
            {
                commandBar.PrimaryCommands.Add(button);
            }
            return commandBar;
        }
        private List<AppBarButton> CreateAppBarButtons(List<Accion> acciones)
        {
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

        private void Accion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                accion = (sender as AccionAppBarButton).Accion;
                if (accion.Descripcion != "Agregar Tema" && MasterMenuItem == null)
                {
                    DisplayNoTopicSelected();
                }
                switch (accion.Descripcion)
                {
                    case "Agregar Tema":
                        FormularioMode();

                        break;
                    case "Eliminar Tema":
                        DisplayDeleteConfirmation();

                        break;
                    case "Modificar Tema":
                        //FormularioUsuarioMode(false);

                        //user = usuarioBL.Consultar(((Usuario)dgUsuarios.SelectedItem).ID);
                        //FillFormFields(user);

                        // LvGrupos.Items.Clear();
                        //CreateGruposListView(LvGrupos, grupos, user.Grupos);

                        break;
                    case "Ver Iniciativas":
                        //FormularioUsuarioMode(true);

                        //user = usuarioBL.Consultar(((Usuario)dgUsuarios.SelectedItem).ID);
                        //FillFormFields(user);

                        //LvGrupos.Items.Clear();
                        //CreateGruposListView(LvGrupos, grupos, user.Grupos);

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

                //ContentDialogResult result = await error.ShowAsync();
                //GridMode();
            }
        }
        private async void DisplayNoTopicSelected()
        {
            ContentDialog noUserSelected = new ContentDialog
            {
                Title = "Tema no seleccionado",
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
                temaBL.Eliminar(MasterMenuItem.ID);
                var data = usuarioBL.ListarTemasDisponibles(SesionActiva.ObtenerInstancia().Usuario.ID);
            }
        }
    }
}
