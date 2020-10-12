using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using Lawful.Helpers;
using Lawful.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Lawful.Views
{
    public sealed partial class TemasPage : Page, INotifyPropertyChanged
    {
        private UsuarioBL usuarioBL;
        private Tema _selected;
        private TemaBL temaBL;
        private Accion accion;
        public Tema Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }
        public TemasPage()
        {
            usuarioBL = new UsuarioBL();
            temaBL = new TemaBL();
            InitializeComponent();
            RefreshTemasListView();
            Usuarios = new ObservableCollection<Usuario>();
            var data = usuarioBL.Listar();
            foreach (var item in data)
            {
                Usuarios.Add(item);
            }
            OnPropertyChanged("Usuarios");
            TemaMode();
            var acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 8);
            CreateCommandBar(this.AccionesBar, acciones);
        }
        public  ObservableCollection<Tema> Temas { get; set; }
        public ObservableCollection<Usuario> Usuarios { get; set; }

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
        private void RefreshTemasListView()
        {
            Temas = new ObservableCollection<Tema>();
            var data = usuarioBL.ListarTemasDisponibles(SesionActiva.ObtenerInstancia().Usuario.ID);
            if (data != null)
            {
                foreach (var item in data)
                {
                    Temas.Add(item);
                }
                if (Temas.Count>0)
                {
                    Selected = Temas[0];
                }
            }
            OnPropertyChanged("Temas");
        }

        private async void Accion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                accion = (sender as AccionAppBarButton).Accion;
                if (accion.Descripcion != "Agregar Tema" && TemasListView.SelectedItem == null)
                {
                    DisplayNoTopicSelected();
                    return;
                }
                switch (accion.Descripcion)
                {
                    case "Agregar Tema":
                        FormularioMode();
                        break;
                    case "Eliminar Tema":
                        ContentDialogResult result = await DisplayDeleteConfirmation();
                        if (result == ContentDialogResult.Primary)
                        {
                            temaBL.Eliminar(((Tema)TemasListView.SelectedItem).ID);
                            RefreshTemasListView();
                        }

                        break;
                    case "Modificar Tema":
                        //FormularioUsuarioMode(false);

                        //user = usuarioBL.Consultar(((Usuario)dgUsuarios.SelectedItem).ID);
                        //FillFormFields(user);

                        // LvGrupos.Items.Clear();
                        //CreateGruposListView(LvGrupos, grupos, user.Grupos);

                        break;
                    case "Ver Iniciativas":
                        this.Frame.Navigate(typeof(IniciativasTemaPage),Selected.ID);

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
            txtTitulo.Text = "";
            txtDescripcion.Text = "";

        }
        private async void DisplayNoTopicSelected()
        {
            ContentDialog noTopicSelected = new ContentDialog
            {
                Title = "Tema no seleccionado",
                Content = "Seleccione uno e intente de nuevo.",
                CloseButtonText = "Ok"
            };

            ContentDialogResult result = await noTopicSelected.ShowAsync();
        }
        private async System.Threading.Tasks.Task<ContentDialogResult> DisplayDeleteConfirmation()
        {
            ContentDialog noUserSelected = new ContentDialog
            {
                Title = "Atención",
                Content = "¿Está seguro que desea eliminarlo?",
                PrimaryButtonText = "Si",
                SecondaryButtonText = "No"
            };

            ContentDialogResult result = await noUserSelected.ShowAsync();
            return result;            
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

        private void TemasListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            TemaMode();
        }
    }
}
