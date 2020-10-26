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
        ObservableCollection<OpcionListViewItem> opciones;
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

            btnGuardar.IsEnabled = false;

            iniciativaBL = new IniciativaBL();
            temaBL = new TemaBL();
            usuarioBL = new UsuarioBL();
            Iniciativas = new ObservableCollection<Iniciativa>();
            opciones = new ObservableCollection<OpcionListViewItem>();

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
            RefreshIniciativasListView();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            if (_selected != null)
            {
                Iniciativa consulta = iniciativaBL.Consultar(Selected.ID);
                _selected.Comentarios = consulta.Comentarios;
                lvComentarios.ItemsSource = _selected.Comentarios;
                switch (_selected.GetType().Name)
                {
                    case "Asistire":
                        ((Asistire)_selected).Opciones = ((Asistire)consulta).Opciones;
                        break;
                    case "DoDont":
                        ((DoDont)_selected).Opciones = ((DoDont)consulta).Opciones;
                        break;

                    case "Regla":
                        ((Regla)_selected).Opciones = ((Regla)consulta).Opciones;
                        break;
                    case "Votacion":
                        ((Votacion)_selected).Opciones = ((Votacion)consulta).Opciones;
                        break;
                    case "VotacionMultiple":
                        ((VotacionMultiple)_selected).Opciones = ((VotacionMultiple)consulta).Opciones;
                        break;
                    default:
                        break;
                }
            }
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

        private async void Accion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                accion = (sender as AccionAppBarButton).Accion;
                if (accion.Descripcion != "Agregar Iniciativa" && IniciativasListView.SelectedItem == null)
                {
                    DisplayNoIniciativaSelected();
                    return;
                }
                switch (accion.Descripcion)
                {
                    case "Agregar Iniciativa":
                        txtComentario.Text = "";
                        txtDescripcion.Text = "";
                        txtLugar.Text = "";
                        txtOpcion.Text = "";
                        txtTitulo.Text = "";
                        slFormularioRelevancia.Value = 0;
                        slFormularioMaxOpcionesSeleccionables.Value = 0;
                        FormularioMode();

                        break;
                    case "Eliminar Iniciativa":
                        ContentDialogResult result = await DisplayDeleteConfirmation();
                        if (result == ContentDialogResult.Primary)
                        {
                            iniciativaBL.Eliminar(Selected.ID);
                            RefreshIniciativasListView();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                var error = new ContentDialog
                {
                    Title = "Error",
                    Content = "Ocurrió un error inesperado, vuelva a intentarlo",
                    CloseButtonText = "Ok"
                };
                await error.ShowAsync();
            }
        }
        private async void DisplayNoIniciativaSelected()
        {
            ContentDialog noTopicSelected = new ContentDialog
            {
                Title = "Iniciativa no seleccionada",
                Content = "Seleccione uno e intente de nuevo.",
                CloseButtonText = "Ok"
            };

            await noTopicSelected.ShowAsync();
        }
        private async System.Threading.Tasks.Task<ContentDialogResult> DisplayDeleteConfirmation()
        {
            ContentDialog noIniciativaSelected = new ContentDialog
            {
                Title = "Atención",
                Content = "¿Está seguro que desea eliminarla?",
                PrimaryButtonText = "Si",
                SecondaryButtonText = "No"
            };

            ContentDialogResult result = await noIniciativaSelected.ShowAsync();
            return result;
        }
        private void RefreshIniciativasListView()
        {
            Iniciativas = new ObservableCollection<Iniciativa>();
            var data = temaBL.ListarIniciativas(temaID);
            if (data != null)
            {
                foreach (var item in data)
                {
                    Iniciativas.Add(item);
                }
                OnPropertyChanged("Iniciativas");
                if (Iniciativas.Count != 0 && Iniciativas[0] != null)
                {
                    lvComentarios.ItemsSource = null;

                    lvDetailOpciones.ItemsSource = null;
                    Iniciativas[0] = iniciativaBL.Consultar(Iniciativas[0].ID);
                    Iniciativa cero = Iniciativas[0];
                    Selected = cero;
                }
            }
            
            DetailsMode();
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

                    if (cbTipoIniciativa.SelectedItem.ToString() == "Do")
                    {
                        ((DoDont)crudIniciativa).Tipo = "Do";
                    }
                    else
                    {
                        ((DoDont)crudIniciativa).Tipo = "Don't";
                    }
                    

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
                    crudIniciativa = Regla.NuevaInstancia(owner);
                    lvFormularioOpciones.ItemsSource = ((Regla)crudIniciativa).Opciones;

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
            bool canVote = true;
            spFormulario.MaxHeight = 0;
            spDetails.MaxHeight = double.PositiveInfinity;
            spDetailAsistire.MaxHeight = 0;
            spDetailDoDont.MaxHeight = 0;
            spDetailRegla.MaxHeight = 0;
            spDetailVotacionMultiple.MaxHeight = 0;
            spDetailOpciones.MaxHeight = 0;
            lvDetailOpciones.SelectionMode = ListViewSelectionMode.Single;
            if (Selected!=null)
            {
                switch (_selected.GetType().Name)
                {
                    case "Asistire":
                        opciones = CreateListViewOpciones(((Asistire)_selected).Opciones);
                        lvDetailOpciones.ItemsSource = opciones;
                        canVote = CanVote(_selected);
                        btnVotar.IsEnabled = canVote;

                        if (!canVote)
                        {
                            ShowVotedOption();
                        }

                        tbFechaEvento.Text = ((Asistire)_selected).FechaEvento.ToString();
                        tbFechaLimiteConfirmacion.Text = ((Asistire)_selected).FechaLimiteConfirmacion.ToString();
                        tbLugar.Text = ((Asistire)_selected).Lugar;

                        spDetailAsistire.MaxHeight = double.PositiveInfinity;
                        spDetailOpciones.MaxHeight = double.PositiveInfinity;
                        spDetailDoDont.MaxHeight = 0;
                        spDetailRegla.MaxHeight = 0;
                        spDetailVotacionMultiple.MaxHeight = 0;

                        break;
                    case "DoDont":
                        opciones = CreateListViewOpciones(((DoDont)_selected).Opciones);
                        lvDetailOpciones.ItemsSource = opciones;
                        rbDo.IsChecked = ((DoDont)_selected).Tipo == "Do" ? true : false;
                        rbDont.IsChecked = ((DoDont)_selected).Tipo == "Don't" ? true : false;
                        canVote = CanVote(_selected);
                        btnVotar.IsEnabled = canVote;

                        if (!canVote)
                        {
                            ShowVotedOption();
                        }

                        spDetailDoDont.MaxHeight = double.PositiveInfinity;
                        spDetailOpciones.MaxHeight = double.PositiveInfinity;
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
                        canVote = false;
                        btnVotar.IsEnabled = canVote;

                        break;
                    case "PropuestaGenerica":
                        spDetailAsistire.MaxHeight = 0;
                        spDetailDoDont.MaxHeight = 0;
                        spDetailRegla.MaxHeight = 0;
                        spDetailVotacionMultiple.MaxHeight = 0;
                        canVote = false;
                        btnVotar.IsEnabled = canVote;

                        break;
                    case "Regla":
                        opciones = CreateListViewOpciones(((Regla)_selected).Opciones);
                        lvDetailOpciones.ItemsSource = opciones;
                        slDetailRelevancia.Value = ((Regla)_selected).Relevancia;
                        canVote = CanVote(_selected);
                        btnVotar.IsEnabled = canVote;

                        if (!canVote)
                        {
                            ShowVotedOption();
                        }

                        spDetailAsistire.MaxHeight = 0;
                        spDetailDoDont.MaxHeight = 0;
                        spDetailOpciones.MaxHeight = double.PositiveInfinity;
                        spDetailRegla.MaxHeight = double.PositiveInfinity;
                        spDetailVotacionMultiple.MaxHeight = 0;
                        break;
                    case "Votacion":
                        opciones = CreateListViewOpciones(((Votacion)_selected).Opciones);
                        lvDetailOpciones.ItemsSource = opciones;
                        canVote = CanVote(_selected);
                        btnVotar.IsEnabled = canVote;

                        if (!canVote)
                        {
                            ShowVotedOption();
                        }

                        spDetailAsistire.MaxHeight = 0;
                        spDetailDoDont.MaxHeight = 0;
                        spDetailRegla.MaxHeight = 0;
                        spDetailVotacionMultiple.MaxHeight = 0;
                        spDetailOpciones.MaxHeight = double.PositiveInfinity;
                        break;
                    case "VotacionMultiple":
                        opciones = CreateListViewOpciones(((VotacionMultiple)_selected).Opciones);
                        lvDetailOpciones.ItemsSource = opciones;
                        tbDetailMaxOpcionesSeleccionables.Text = ((VotacionMultiple)_selected).MaxOpcionesSeleccionables.ToString();
                        canVote = CanVote(_selected);
                        btnVotar.IsEnabled = canVote;

                        lvDetailOpciones.SelectionMode = ListViewSelectionMode.Multiple;
                        if (!canVote)
                        {
                            foreach (var item in lvDetailOpciones.Items)
                            {
                                if (((OpcionListViewItem)item).Opcion.Votantes.FindIndex(x => x.ID == SesionActiva.ObtenerInstancia().Usuario.ID) != -1)
                                {
                                    lvDetailOpciones.SelectedItems.Add(item);
                                }

                            }
                        }

                        spDetailAsistire.MaxHeight = 0;
                        spDetailDoDont.MaxHeight = 0;
                        spDetailRegla.MaxHeight = 0;
                        spDetailVotacionMultiple.MaxHeight = double.PositiveInfinity;
                        spDetailOpciones.MaxHeight = double.PositiveInfinity;

                        break;
                    default:
                        break;
                }
            }
            
        }

        private void ShowVotedOption()
        {
            foreach (OpcionListViewItem item in lvDetailOpciones.Items)
            {
                if (item.Opcion.Votantes.FindIndex(x => x.ID == SesionActiva.ObtenerInstancia().Usuario.ID) != -1)
                {
                    lvDetailOpciones.SelectedItem = item;
                    break;
                }
            }
        }

        private ObservableCollection<OpcionListViewItem> CreateListViewOpciones(List<Opcion> IniciativaOpciones)
        {
            ObservableCollection<OpcionListViewItem> opciones = new ObservableCollection<OpcionListViewItem>();
            foreach (var item in IniciativaOpciones)
            {
                var opcion = new OpcionListViewItem();
                opcion.Opcion = item;
                opcion.Content = opcion.Opcion.Descripcion;
                opciones.Add(opcion);
            }

            return opciones;
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
        private void btnGuardar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                crudIniciativa.Tema = new Tema() { ID = temaID };
                crudIniciativa.FechaCreacion = DateTime.Now.Date;
                if (String.IsNullOrWhiteSpace(txtTitulo.Text)
                    || String.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    throw new Exception("El título y la descripción no deben estas vacíos.");
                }
                else
                {
                    crudIniciativa.Titulo = txtTitulo.Text;
                    crudIniciativa.Descripcion = txtDescripcion.Text;
                }
                
                if (dpFechaCierre.SelectedDate.Value.Date < DateTime.Now.Date)
                {
                    throw new Exception("La fecha de cierre debe ser mayor o igual a hoy.");
                }
                else
                {
                    crudIniciativa.FechaCierre = dpFechaCierre.SelectedDate.Value.Date;
                }
               
                
                switch (crudIniciativa.GetType().Name)
                {
                    case "Asistire":
                        if (String.IsNullOrWhiteSpace(txtLugar.Text))
                        {
                            throw new Exception("El campo lugar debe estar completo.");
                        }
                        if (dpFechaLimiteConfirmacion.SelectedDate.Value.Date < DateTime.Now.Date
                            || dpFechaEvento.SelectedDate.Value.Date < DateTime.Now.Date)
                        {
                            throw new Exception("Las fechas deben ser mayores o iguales a hoy.");
                        }
                        else
                        {
                            ((Asistire)crudIniciativa).Lugar = txtLugar.Text;
                            ((Asistire)crudIniciativa).FechaLimiteConfirmacion = dpFechaLimiteConfirmacion.SelectedDate.Value.Date;
                            ((Asistire)crudIniciativa).FechaEvento = dpFechaEvento.SelectedDate.Value.Date;
                            
                        }
                        break;
                    case "DoDont":
                        ((DoDont)crudIniciativa).Tipo = ((KeyValuePair<string,string>)cbTipoIniciativa.SelectedItem).Value;
                        break;
                    case "FAQ":

                        break;
                    case "PropuestaGenerica":
                        break;
                    case "Regla":
                        ((Regla)crudIniciativa).Relevancia = Convert.ToInt32(slFormularioRelevancia.Value);
                        break;
                    case "Votacion":
                        if (lvFormularioOpciones.Items.Count>0)
                        {
                            foreach (Opcion item in lvFormularioOpciones.Items)
                            {
                                ((Votacion)crudIniciativa).Opciones.Add(item);
                            }
                        }
                        else
                        {
                            throw new Exception("Debe crear al menos una opción");
                        }

                        break;
                    case "VotacionMultiple":
                        ((VotacionMultiple)crudIniciativa).MaxOpcionesSeleccionables = Convert.ToInt32(slFormularioMaxOpcionesSeleccionables.Value);
                        if (((VotacionMultiple)crudIniciativa).MaxOpcionesSeleccionables > lvFormularioOpciones.Items.Count)
                        {
                            throw new Exception("La cantidad de opciones debe ser mayor o igual al máximo de opciones seleccionables");
                        }
                        else
                        {
                            if (lvFormularioOpciones.Items.Count > 0)
                            {
                                foreach (Opcion item in lvFormularioOpciones.Items)
                                {
                                    ((VotacionMultiple)crudIniciativa).Opciones.Add(item);
                                }
                            }
                            else
                            {
                                throw new Exception("Debe crear al menos una opción");
                            }
                        }     
                        break;
                    default:
                        break;
                }


                iniciativaBL.Insertar(crudIniciativa);
                RefreshIniciativasListView();
                DetailsMode();

            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
            }
            
        }

        private void btnCancelar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DetailsMode();
        }

        private void btnVotar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                List<Opcion> opcionesSelected = new List<Opcion>();

                foreach (var item in opciones)
                {
                    if (item.IsSelected)
                    {
                        opcionesSelected.Add(item.Opcion);
                    }
                }
                if (_selected.GetType().Name == "VotacionMultiple")
                {
                    if (opcionesSelected.Count > ((VotacionMultiple)_selected).MaxOpcionesSeleccionables)
                    {
                        throw new Exception("El máximo de opciones seleccionables es " + ((VotacionMultiple)_selected).MaxOpcionesSeleccionables + ".");
                    }
                }

                if (opcionesSelected.Count > 0)
                {
                    iniciativaBL.InsertarVoto(SesionActiva.ObtenerInstancia().Usuario.ID, opcionesSelected);
                    btnVotar.IsEnabled = false;
                }
                else
                {
                    throw new Exception("Debe seleccionar al menos una opción.");
                }
            }
            catch (Exception ex)
            {

                DisplayError(ex.Message);
            }
            
        }

        private void cbTipoIniciativa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTipoIniciativa.SelectedValue != null)
            {
                btnGuardar.IsEnabled = true;
            }
            FormularioMode();
        }

        private void txtOpcion_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !String.IsNullOrWhiteSpace(txtOpcion.Text))
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

        private void txtComentario_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == Windows.System.VirtualKey.Enter && !String.IsNullOrWhiteSpace(txtComentario.Text))
                {
                    var comentario = new Comentario()
                    {
                        Descripcion = txtComentario.Text,
                        Owner = SesionActiva.ObtenerInstancia().Usuario,
                        Fecha = DateTime.Now
                    };
                    txtComentario.Text = "";
                    iniciativaBL.InsertarComentario(Selected.ID, comentario);
                    var iniciativa = iniciativaBL.Consultar(Selected.ID);
                    for (int i = 0; i < Iniciativas.Count; i++)
                    {
                        if (Iniciativas[i].ID == iniciativa.ID)
                        {
                            Iniciativas[i] = iniciativa;
                            Selected = Iniciativas[i];
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanVote(Iniciativa iniciativa)
        {
            return !iniciativa.UserHasVoted(SesionActiva.ObtenerInstancia().Usuario.ID);
        }
    }
}
