using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using Lawful.Helpers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Lawful.Views
{
    public sealed partial class GruposPage : Page, INotifyPropertyChanged
    {
        private GrupoBL grupoBL;
        private UsuarioBL usuarioBL;
        private Grupo _selected;
        private Grupo crudGrupo;
        private Accion accion;
        public Grupo Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }
        public GruposPage()
        {
            usuarioBL = new UsuarioBL();
            grupoBL = new GrupoBL();
            InitializeComponent();
            RefreshGroups();
            Acciones = new ObservableCollection<AccionListViewItem>();
            var accionesGrupo = grupoBL.ListarAcciones();
            foreach (var item in accionesGrupo)
            {
                var accionlb = new AccionListViewItem();
                accionlb.Accion = item;
                accionlb.Content = accionlb.Accion.Descripcion;
                Acciones.Add(accionlb);
            }
            GruposMode();
            foreach (ListViewItem item in lvAcciones.Items)
            {
                item.IsSelected = true;
            }

            var acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 2);
            CreateCommandBar(AccionesBar, acciones);

        }
        public ObservableCollection<Grupo> Grupos { get; set; }
        public ObservableCollection<AccionListViewItem> Acciones { get; set; }

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

        private void RefreshGroups()
        {
            
            Grupos = new ObservableCollection<Grupo>();
            var data = grupoBL.Listar();
            foreach (var item in data)
            {
                Grupos.Add(item);
            }
            dgGrupos.ItemsSource = null;
            dgGrupos.ItemsSource = Grupos;
        }

        private async void Accion_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                accion = (sender as AccionAppBarButton).Accion;
                if (accion.Descripcion != "Agregar Grupo" && dgGrupos.SelectedItem == null)
                {
                    DisplayNoGroupSelected();
                    return;
                }
                switch (accion.Descripcion)
                {
                    case "Agregar Grupo":
                        FormularioMode(false);

                        break;
                    case "Eliminar Grupo":
                        ContentDialogResult result = await DisplayDeleteConfirmation();
                        if (result == ContentDialogResult.Primary)
                        {
                            grupoBL.Eliminar(((Grupo)dgGrupos.SelectedItem).ID);
                            RefreshGroups();
                        }

                        break;
                    case "Modificar Grupo":
                        FormularioMode(false);
                        crudGrupo = grupoBL.Consultar(Selected.ID);
                        FillFields();

                        break;
                    case "Consultar Grupo":
                        FormularioMode(true);
                        crudGrupo = grupoBL.Consultar(Selected.ID);
                        FillFields();

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ContentDialog error = new ContentDialog
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Ok"
                };
                await error.ShowAsync();
            }
        }
        private void FillFields()
        {
            txtCodigo.Text = crudGrupo.Codigo;
            txtDescripcion.Text = crudGrupo.Descripcion;
            foreach (AccionListViewItem accion in lvAcciones.Items)
            {
                if (crudGrupo.Acciones.FindIndex(x => x.ID == accion.Accion.ID) != -1)
                {
                    accion.IsSelected = true;
                }
            }
        }
        private async void DisplayNoGroupSelected()
        {
            ContentDialog noTopicSelected = new ContentDialog
            {
                Title = "Grupo no seleccionado",
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

        private AppBarButton CreateFindAppBarButton()
        {
            AppBarButton buscar = new AppBarButton();
            buscar.Name = "abbBuscar";
            buscar.Label = "Buscar";
            buscar.Icon = new SymbolIcon(Symbol.Find);
            return buscar;
        }

        private CommandBar CreateCommandBar(CommandBar commandBar, List<Accion> acciones)
        {
            commandBar.PrimaryCommands.Add(CreateFindAppBarButton());
            foreach (var button in CreateAppBarButtons(acciones))
            {
                commandBar.PrimaryCommands.Add(button);
            }
            return commandBar;
        }
        private bool AreFieldsFilled()
        {
            if (String.IsNullOrWhiteSpace(txtCodigo.Text) ||
                String.IsNullOrWhiteSpace(txtDescripcion.Text))
            {

                throw new Exception("Debe completar todos los campos");
            }
            else
            {
                return true;
            }
        }
        private void FillUserFields()
        {
            crudGrupo.Codigo = txtCodigo.Text;
            crudGrupo.Descripcion = txtDescripcion.Text;
            crudGrupo.Estado = true;
            foreach (AccionListViewItem item in lvAcciones.Items)
            {
                if (item.IsSelected)
                {
                    crudGrupo.Acciones.Add(item.Accion);
                }
            }
        }

        private async void btnGuardar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                switch (accion.Descripcion)
                {
                    case "Agregar Grupo":
                        if (AreFieldsFilled())
                        {
                            crudGrupo = new Grupo();
                            FillUserFields();

                            grupoBL.Insertar(crudGrupo);

                            RefreshGroups();
                            GruposMode();
                        }
                        break;
                    case "Modificar Grupo":
                        if (AreFieldsFilled())
                        {
                            crudGrupo = new Grupo();
                            crudGrupo.ID = Selected.ID;
                            FillUserFields();

                            grupoBL.Modificar(crudGrupo);

                            RefreshGroups();
                            if (Grupos.Count>0)
                            {
                                Selected = Grupos[0];
                            }
                            else
                            {
                                Selected = null;
                            }
                            GruposMode();
                        }
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                ContentDialog error = new ContentDialog
                {
                    Title = "Error",
                    Content = ex.Message,
                    CloseButtonText = "Ok"
                };
                await error.ShowAsync();
            }
        }

        private void btnCancelar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            GruposMode();
        }
        private void GruposMode()
        {
            ListaGupos.MaxHeight = double.PositiveInfinity;
            FormularioGrupo.MaxHeight = 0;
        }
        private void FormularioMode(bool isConsultar)
        {
            ListaGupos.MaxHeight = 100;
            FormularioGrupo.Visibility = Windows.UI.Xaml.Visibility.Visible;
            FormularioGrupo.MaxHeight = double.PositiveInfinity;
            txtCodigo.Text = "";
            txtDescripcion.Text = "";
            btnGuardar.IsEnabled = !isConsultar;
            foreach (ListViewItem item in lvAcciones.Items)
            {
                item.IsSelected = false;
            }
        }
    }
}
