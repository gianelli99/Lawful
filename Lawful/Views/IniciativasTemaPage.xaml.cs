using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using Lawful.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lawful.Views
{
    public sealed partial class IniciativasTemaPage : Page, INotifyPropertyChanged
    {
        private IniciativaBL iniciativaBL;
        private UsuarioBL usuarioBL;
        private TemaBL temaBL;
        private Accion accion;
        private Iniciativa _selected;
        private Iniciativa crudIniciativa;
        private int temaID;
        public ObservableCollection<Iniciativa> Iniciativas { get; set; }
        public Iniciativa Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }
        public IniciativasTemaPage()
        {
            InitializeComponent();
            iniciativaBL = new IniciativaBL();
            temaBL = new TemaBL();
            usuarioBL = new UsuarioBL();
            Iniciativas = new ObservableCollection<Iniciativa>();

            var acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 9);
            CreateCommandBar(this.AccionesBar, acciones);

            var tipos = iniciativaBL.ListarTipos();
            foreach (var item in tipos)
            {
                this.cbTipoIniciativa.Items.Add(new KeyValuePair<string, string>(item[1], item[0]));
            }
            this.cbTipoIniciativa.SelectedValuePath = "Key";
            this.cbTipoIniciativa.DisplayMemberPath = "Value";

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            temaID = Convert.ToInt32(e.Parameter);
            base.OnNavigatedTo(e);
            var iniciativas = temaBL.ListarIniciativas(temaID);
            foreach (var item in iniciativas)
            {
                Iniciativas.Add(item);
            }
            Selected = Iniciativas[0];
            IniciativasListView.SelectedItem = Selected;
            OnPropertyChanged("Selected");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            _selected = iniciativaBL.Consultar(_selected.ID);
            lvComentarios.ItemsSource = null;
            lvComentarios.ItemsSource = _selected.Comentarios;
            lvDetailOpciones.ItemsSource = null;
            DetailsMode();
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
                if (accion.Descripcion != "Agregar Iniciativa" && IniciativasListView.SelectedItem == null)
                {
                    //DisplayNoTopicSelected();
                    return;
                }
                switch (accion.Descripcion)
                {
                    case "Agregar Iniciativa":
                        FormularioMode();

                        break;
                    case "Eliminar Iniciativa":
                        //ContentDialogResult result = await DisplayDeleteConfirmation();
                        //if (result == ContentDialogResult.Primary)
                        //{
                        //    temaBL.Eliminar(((Tema)TemasListView.SelectedItem).ID);
                        //    RefreshTemasListView();
                        //}
                        //if (Temas[0] != null)
                        //{
                        //    Selected = Temas[0];
                        //}

                        //OnPropertyChanged("Selected");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                new ContentDialog
                {
                    Title = "Error",
                    Content = "Ocurrió un error inesperado, vuelva a intentarlo",
                    CloseButtonText = "Ok"
                };
            }
        }

        private void FormularioMode()
        {
            Usuario owner = SesionActiva.ObtenerInstancia().Usuario;
            lvFormularioOpciones.ItemsSource = null;
            spFormulario.MaxHeight = double.PositiveInfinity;
            spDetails.MaxHeight = 0;
            spFormularioAsistire.MaxHeight = 0;
            spFormularioRegla.MaxHeight = 0;
            spFormularioVotacionMultiple.MaxHeight = 0;
            spFormularioOpciones.MaxHeight = 0;
            switch (cbTipoIniciativa.SelectedValue)
            {
                case "Asistire":
                    crudIniciativa = Asistire.NuevaInstancia(owner);
                    lvFormularioOpciones.ItemsSource = ((Asistire)crudIniciativa).Opciones;
                    tbFechaEvento.Text = "";
                    tbFechaLimiteConfirmacion.Text = "";
                    tbLugar.Text = "";

                    spFormularioAsistire.MaxHeight = double.PositiveInfinity;
                    spFormularioRegla.MaxHeight = 0;
                    spFormularioVotacionMultiple.MaxHeight = 0;
                    spFormularioOpciones.MaxHeight = 0;
                    break;
                case "DoDont":
                    crudIniciativa = DoDont.NuevaInstancia(owner);
                    lvFormularioOpciones.ItemsSource = ((DoDont)crudIniciativa).Opciones;
                    ((DoDont)crudIniciativa).Tipo = "Do";
                    rbDo.IsChecked = ((DoDont)crudIniciativa).Tipo == "Do" ? true : false;
                    rbDont.IsChecked = ((DoDont)crudIniciativa).Tipo == "Dont" ? true : false;

                    spFormularioAsistire.MaxHeight = 0;
                    spFormularioRegla.MaxHeight = 0;
                    spFormularioVotacionMultiple.MaxHeight = 0;
                    spFormularioOpciones.MaxHeight = 0;
                    break;
                case "FAQ":
                    crudIniciativa = new FAQ(owner);
                    //marcar comentario correcto
                    spFormularioAsistire.MaxHeight = 0;
                    spFormularioRegla.MaxHeight = 0;
                    spFormularioVotacionMultiple.MaxHeight = 0;
                    spFormularioOpciones.MaxHeight = 0;
                    break;
                case "PropuestaGenerica":
                    crudIniciativa = new PropuestaGenerica(owner);

                    spFormularioAsistire.MaxHeight = 0;
                    spFormularioRegla.MaxHeight = 0;
                    spFormularioVotacionMultiple.MaxHeight = 0;
                    spFormularioOpciones.MaxHeight = 0;
                    break;
                case "Regla":
                    crudIniciativa = new Regla(owner);
                    lvFormularioOpciones.ItemsSource = ((Regla)crudIniciativa).Opciones;
                    slFormularioRelevancia.Value = ((Regla)crudIniciativa).Relevancia;

                    spFormularioAsistire.MaxHeight = 0;
                    spFormularioRegla.MaxHeight = double.PositiveInfinity;
                    spFormularioVotacionMultiple.MaxHeight = 0;
                    spFormularioOpciones.MaxHeight = 0;
                    break;
                case "Votacion":
                    crudIniciativa = new Votacion(owner);
                    spFormularioOpciones.MaxHeight = double.PositiveInfinity;

                    spFormularioAsistire.MaxHeight = 0;
                    spFormularioRegla.MaxHeight = 0;
                    spFormularioVotacionMultiple.MaxHeight = 0;
                    break;
                case "VotacionMultiple":
                    crudIniciativa = new VotacionMultiple(owner);
                    slDetailMaxOpcionesSeleccionables.Value = ((VotacionMultiple)crudIniciativa).MaxOpcionesSeleccionables;
                    spFormularioOpciones.MaxHeight = double.PositiveInfinity;

                    spFormularioAsistire.MaxHeight = 0;
                    spFormularioRegla.MaxHeight = 0;
                    spFormularioVotacionMultiple.MaxHeight = double.PositiveInfinity;
                    break;
                default:
                    break;
            }
        }

        private void DetailsMode()
        {
            spFormulario.MaxHeight = 0;
            spDetails.MaxHeight = double.PositiveInfinity;
            switch (_selected.GetType().Name)
            {
                case "Asistire":
                    lvDetailOpciones.ItemsSource = ((Asistire)_selected).Opciones;
                    tbFechaEvento.Text = ((Asistire)_selected).FechaEvento.ToString();
                    tbFechaLimiteConfirmacion.Text = ((Asistire)_selected).FechaLimiteConfirmacion.ToString();
                    tbLugar.Text = ((Asistire)_selected).Lugar;

                    spDetailAsistire.MaxHeight = double.PositiveInfinity;
                    spDetailDoDont.MaxHeight = 0;
                    spDetailRegla.MaxHeight = 0;
                    spDetailVotacionMultiple.MaxHeight = 0;
                    break;
                case "DoDont":
                    lvDetailOpciones.ItemsSource = ((DoDont)_selected).Opciones;
                    rbDo.IsChecked = ((DoDont)_selected).Tipo == "Do" ? true : false;
                    rbDont.IsChecked = ((DoDont)_selected).Tipo == "Dont" ? true : false;

                    spDetailDoDont.MaxHeight = double.PositiveInfinity;
                    spDetailAsistire.MaxHeight = 0;
                    spDetailRegla.MaxHeight = 0;
                    spDetailVotacionMultiple.MaxHeight = 0;
                    break;
                case "FAQ":
                    //marcar comentario correcto
                    spDetailAsistire.MaxHeight = 0;
                    spDetailDoDont.MaxHeight = 0;
                    spDetailRegla.MaxHeight = 0;
                    spDetailVotacionMultiple.MaxHeight = 0;
                    break;
                case "PropuestaGenerica":
                    spDetailAsistire.MaxHeight = 0;
                    spDetailDoDont.MaxHeight = 0;
                    spDetailRegla.MaxHeight = 0;
                    spDetailVotacionMultiple.MaxHeight = 0;
                    break;
                case "Regla":
                    lvDetailOpciones.ItemsSource = ((Regla)_selected).Opciones;
                    slDetailRelevancia.Value = ((Regla)_selected).Relevancia;

                    spDetailAsistire.MaxHeight = 0;
                    spDetailDoDont.MaxHeight = 0;
                    spDetailRegla.MaxHeight = double.PositiveInfinity;
                    spDetailVotacionMultiple.MaxHeight = 0;
                    break;
                case "Votacion":
                    lvDetailOpciones.ItemsSource = ((Votacion)_selected).Opciones;

                    spDetailAsistire.MaxHeight = 0;
                    spDetailDoDont.MaxHeight = 0;
                    spDetailRegla.MaxHeight = 0;
                    spDetailVotacionMultiple.MaxHeight = 0;
                    break;
                case "VotacionMultiple":
                    lvDetailOpciones.ItemsSource = ((VotacionMultiple)_selected).Opciones;
                    slDetailMaxOpcionesSeleccionables.Value = ((VotacionMultiple)_selected).MaxOpcionesSeleccionables;

                    spDetailAsistire.MaxHeight = 0;
                    spDetailDoDont.MaxHeight = 0;
                    spDetailRegla.MaxHeight = 0;
                    spDetailVotacionMultiple.MaxHeight = double.PositiveInfinity;
                    break;
                default:
                    break;
            }
        }
        private void btnGuardar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void btnVotar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void cbTipoIniciativa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FormularioMode();
        }

        private void txtOpcion_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var opcion = new Opcion() { Descripcion = txtOpcion.Text };
                txtOpcion.Text = "";
                lvFormularioOpciones.Items.Add(opcion);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Opcion item in lvFormularioOpciones.Items)
            {
                if (((HyperlinkButton)sender).Name == item.Descripcion)
                {
                    lvFormularioOpciones.Items.Remove(item);
                    break;
                }
            }
        }
    }
}
