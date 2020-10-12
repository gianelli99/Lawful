using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Lawful.Core.Modelo;
using Lawful.Core.Logica;
using Lawful.Helpers;

namespace Lawful.Views
{
    public sealed partial class TemasPageViejo : Page, INotifyPropertyChanged
    {
        private UsuarioBL usuarioBL;
        private TemaBL temaBL;
        private Tema _selected;
        private Accion accion;
        public Tema Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<Tema> Temas { get; private set; } = new ObservableCollection<Tema>();

        public TemasPageViejo()
        {
            usuarioBL = new UsuarioBL();
            temaBL = new TemaBL();
            InitializeComponent();
            Loaded += TemasPage_Loaded;

            var acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 8);
            var bar = new CommandBar();
            CreateCommandBar(bar, acciones);
            MasterDetailsViewControl.DetailsCommandBar = bar;
        }

        //private void FormularioMode()
        //{
        //    spFormularioTema.Visibility = Visibility.Visible;
        //    tbTitulo.Visibility = Visibility.Collapsed;
        //    spDetails.Visibility = Visibility.Collapsed;
        //}
        //private void TemaMode()
        //{
        //    spFormularioTema.Visibility = Visibility.Collapsed;
        //    tbTitulo.Visibility = Visibility.Visible;
        //    spDetails.Visibility = Visibility.Visible;
        //}
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
                if (accion.Descripcion != "Agregar Tema" && Selected == null)
                {
                    DisplayNoTopicSelected();
                }
                switch (accion.Descripcion)
                {
                    case "Agregar Tema":
                        //FormularioMode();

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
                //temaBL.Eliminar(MasterMenuItem.ID);
                var data = usuarioBL.ListarTemasDisponibles(SesionActiva.ObtenerInstancia().Usuario.ID);
            }
        }
        private void TemasPage_Loaded(object sender, RoutedEventArgs e)
        {
            Temas.Clear();

            var data = usuarioBL.ListarTemasDisponibles(SesionActiva.ObtenerInstancia().Usuario.ID);

            foreach (var item in data)
            {
                Temas.Add(item);
            }
            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                Selected = Temas.FirstOrDefault();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
