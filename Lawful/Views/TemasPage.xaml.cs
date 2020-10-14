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
        private Tema crudTema;
        private TemaBL temaBL;
        private Accion accion;
        public Tema Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }
        public TemasPage()
        {
            crudTema = new Tema();
            usuarioBL = new UsuarioBL();
            temaBL = new TemaBL();
            InitializeComponent();
            RefreshTemasListView();
            cbSoloYo.IsChecked = true;
            Usuarios = new ObservableCollection<UsuarioListViewItem>();
            var data = usuarioBL.Listar();
            foreach (var item in data)
            {
                var userlb = new UsuarioListViewItem();
                userlb.Usuario = item;
                userlb.Content = userlb.Usuario.GetNombreCompleto();
                Usuarios.Add(userlb);
            }
            OnPropertyChanged("Usuarios");
            TemaMode();
            var acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 8);
            CreateCommandBar(this.AccionesBar, acciones);
        }
        public ObservableCollection<Tema> Temas { get; set; }
        public ObservableCollection<UsuarioListViewItem> Usuarios { get; set; }
        private string MyEveryoneCanEdit
        {
            get
            {
                if (TemasListView.SelectedItem != null && ((Tema)TemasListView.SelectedItem).EveryoneCanEdit)
                {
                    return "Si, todos los pertenecientes al tema";
                }
                else
                {
                    return "No, solo el creador del tema";
                }
            }
        }
        private string MyEstado { get
            {
                if (Selected.Estado)
                {
                    return "Activo";
                }
                else
                {
                    return "Cerrado";
                }
            } }
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
                        crudTema = temaBL.Consultar(Selected.ID);
                        FormularioMode();
                        FillFields();
                        OnPropertyChanged("Usuarios");
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
            }
        }

        private void FillFields()
        {
            txtTitulo.Text = crudTema.Titulo;
            txtDescripcion.Text = crudTema.Descripcion;
            dpFechaCierre.Date = crudTema.FechaCierre.Date;
            cbSoloYo.IsChecked = crudTema.EveryoneCanEdit ? false : true;
            cbTodos.IsChecked = crudTema.EveryoneCanEdit ? true : false;

            foreach (UsuarioListViewItem user in lvUsuarios.Items)
            {
                if (crudTema.Usuarios.FindIndex(x => x.ID == user.Usuario.ID) != -1)
                {
                    user.IsSelected = true;
                }
            }
        }

        private void FormularioMode()
        {
            spFormularioTema.Visibility = Visibility.Visible;
            tbTitulo.Visibility = Visibility.Collapsed;
            spDetails.Visibility = Visibility.Collapsed;
            txtTitulo.Text = "";
            txtDescripcion.Text = "";
            cbSoloYo.IsChecked = true;
            foreach (UsuarioListViewItem item in lvUsuarios.Items)
            {
                item.IsSelected = false;
            }
        }
        private void TemaMode()
        {
            spFormularioTema.Visibility = Visibility.Collapsed;
            tbTitulo.Visibility = Visibility.Visible;
            spDetails.Visibility = Visibility.Visible;
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

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            TemaMode();
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (accion.Descripcion)
                {
                    case "Agregar Tema":
                        if (AreFieldsFilled() && IsDateValid())
                        {
                            crudTema = new Tema(SesionActiva.ObtenerInstancia().Usuario);
                            crudTema.Titulo = txtTitulo.Text;
                            crudTema.Descripcion = txtDescripcion.Text;
                            crudTema.FechaCreacion = DateTime.Now.Date;
                            crudTema.FechaCierre = dpFechaCierre.Date.Date;
                            
                            if (cbTodos.IsChecked == true)
                            {
                                crudTema.EveryoneCanEdit = true;
                            }
                            else
                            {
                                crudTema.EveryoneCanEdit = false;
                            }
                            foreach (UsuarioListViewItem item in lvUsuarios.SelectedItems)
                            {
                                crudTema.Usuarios.Add(item.Usuario);
                            }
                            temaBL.Insertar(crudTema);
                            RefreshTemasListView();
                        }
                        break;
                    case "Modificar Tema":
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
        }
        private bool IsDateValid()
        {
            if (dpFechaCierre.Date.Date >= DateTime.Now.Date)
            {
                return true;
            }
            else
            {
                throw new Exception("La fecha de cierre debe ser mayor a la actual");
            }
            
        }
        private bool AreFieldsFilled()
        {
            if (String.IsNullOrWhiteSpace(txtTitulo.Text) ||
                String.IsNullOrWhiteSpace(txtDescripcion.Text))
            {

                throw new Exception("Debe completar todos los campos");
            }
            else
            {
                return true;
            }
        }
        private async void DisplayError(string errorM)
        {
            ContentDialog error = new ContentDialog
            {
                Title = "Error",
                Content = errorM,
                CloseButtonText = "Ok"
            };

            await error.ShowAsync();
        }
    }
}
