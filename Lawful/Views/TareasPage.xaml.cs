using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using Lawful.Core.Modelo.TareaEstados;
using Lawful.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace Lawful.Views
{
    public sealed partial class TareasPage : Page, INotifyPropertyChanged
    {
        private readonly UsuarioBL usuarioBL;

        private readonly TareaBL tareaBL;

        private readonly List<Usuario> usuarios;

        public ObservableCollection<Tema> Temas { get; set; }

        public ObservableCollection<Tarea> TareasPorHacer { get; set; }

        public ObservableCollection<Tarea> TareasEnCurso { get; set; }

        public ObservableCollection<Tarea> TareasFinalizadas { get; set; }

        public ObservableCollection<Tarea> TareasFrom { get; set; }

        public List<Comentario> comentariosOC;

        public ObservableCollection<Tarea> TareasTo { get; set; }
        bool isAlta = false;

        bool isDetails = true;

        private Tema _selectedTema;
        private Tarea _selectedTarea;
        public Tema SelectedTema
        {
            get { return _selectedTema; }
            set { Set(ref _selectedTema, value); }
        }
        public Tarea SelectedTarea
        {
            get { return _selectedTarea; }
            set { Set(ref _selectedTarea, value); }
        }


        public TareasPage()
        {

            usuarioBL = new UsuarioBL();
            tareaBL = new TareaBL();
            TareasPorHacer = new ObservableCollection<Tarea>();
            TareasEnCurso = new ObservableCollection<Tarea>();
            TareasFinalizadas = new ObservableCollection<Tarea>();
            comentariosOC = new List<Comentario>();

            InitializeComponent();


            var acciones = usuarioBL.ListarAccionesDisponiblesEnVista(SesionActiva.ObtenerInstancia().Usuario.ID, 10);
            CommandBarHelpers.CreateCommandBar(this.AccionesBar, acciones, Accion_Click);

            RefreshTemasListView();
            

            var tareas = tareaBL.ListarPorTema(_selectedTema.ID);
            OrganizeTareasByState(tareas);

            SelectedTema.Tareas = tareas;

            if (SelectedTema.Tareas.Count > 0) {
                SelectedTarea = SelectedTema.Tareas[0];
                DetailsMode(SelectedTarea);
            }
            usuarios = usuarioBL.ListarPorTema(SelectedTema.ID);
            loadUsersInListView(usuarios);


        }

        private void loadUsersInListView(List<Usuario> usuarios)
        {
            foreach (var item in usuarios)
            {
                UsuarioListViewItem usuarioListViewItem = new UsuarioListViewItem()
                {
                    Usuario = item,
                    Name = item.ID.ToString(),
                    Content = item.GetNombreCompleto()
                };
                lvResponsableFormulario.Items.Add(usuarioListViewItem);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            if (value != null && value.GetType().Name == "Tema")
            {
                var tareas = tareaBL.ListarPorTema(_selectedTema.ID);
                OrganizeTareasByState(tareas);
                //SelectedTarea = null;
                SelectedTema.Tareas = tareas;
                if (SelectedTema.Tareas != null && SelectedTema.Tareas.Count > 0)
                {
                    SelectedTarea = SelectedTema.Tareas[0];
                }
                DetailsMode(SelectedTarea);
            }
            if (value != null && value.GetType().Name == "Tarea")
            {
                comentariosOC = SelectedTarea.Comentarios;
                lvComentarios.ItemsSource = comentariosOC;
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void RefreshTemasListView() // Esta bien
        {
            Temas = new ObservableCollection<Tema>();
            var data = usuarioBL.ListarTemasDisponibles(SesionActiva.ObtenerInstancia().Usuario.ID);
            if (data != null)
            {
                foreach (var item in data)
                {
                    Temas.Add(item);
                }
                if (Temas.Count > 0)
                {
                    SelectedTema = Temas[0];
                }
            }
            OnPropertyChanged("Temas");
        }

        private async void Accion_Click(object sender, RoutedEventArgs e)
        {
            switch (((AccionAppBarButton)sender).Accion.Descripcion)
            {
                case "Agregar Tarea":
                    FormularioMode(false, null);
                    isAlta = true;
                    break;
                case "Modificar Tarea":
                    if (SelectedTarea != null)
                    {
                        FormularioMode(true, SelectedTarea);
                        isAlta = false;
                    }

                    break;
                case "Eliminar Tarea":
                    if (SelectedTarea != null)
                    {
                        ContentDialogResult result = await ModalHelpers.DisplayDeleteConfirmation();
                        if (result == ContentDialogResult.Primary)
                        {
                            tareaBL.Eliminar(((Tarea)SelectedTarea).ID);
                            RefreshSelectedTema();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void TxtComentario_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            try
            {
                if (e.Key == Windows.System.VirtualKey.Enter && !String.IsNullOrWhiteSpace(txtComentario.Text))
                {
                    var tarea = SelectedTarea;
                    var comentario = new Comentario()
                    {
                        Descripcion = txtComentario.Text,
                        Owner = SesionActiva.ObtenerInstancia().Usuario,
                        Fecha = DateTime.Now
                    };
                    txtComentario.Text = "";
                    tareaBL.InsertarComentario(SelectedTarea.ID, comentario);
                    comentariosOC.Add(comentario);
                    lvComentarios.ItemsSource = null;
                    lvComentarios.ItemsSource = comentariosOC;
                }
            }
            catch (Exception ex)
            {
                ModalHelpers.DisplayError(ex.Message);
            }

        } // Esta bien

        private void BtnAceptar_Click(object sender, RoutedEventArgs e) // Esta bien
        {
            try
            {
                if ((UsuarioListViewItem)lvResponsableFormulario.SelectedItem == null)
                {
                    ModalHelpers.DisplayModal("La tarea debe tener un responsable‍");
                    return;
                }
                if (String.IsNullOrWhiteSpace(txtDescripcion.Text) || String.IsNullOrWhiteSpace(txtTitulo.Text))
                {
                    ModalHelpers.DisplayModal("La tarea debe tener un título y una descripción ‍");
                    return;
                }

                if (isAlta)
                {
                    Tarea tarea = new Tarea()
                    {
                        Titulo = txtTitulo.Text,
                        Descripcion = txtDescripcion.Text,
                        Importancia = Convert.ToInt32(slImportancia.Value),
                        Responsable = ((UsuarioListViewItem)lvResponsableFormulario.SelectedItem).Usuario,
                        Estado = new PorHacer(),
                        FechaEnCurso = DateTime.Now.Date,
                        FechaPorHacer = DateTime.Now.Date,
                        FechaFinalizada = DateTime.Now.Date
                    };
                    SelectedTarea = tarea;
                    TareasPorHacer.Add(tarea);
                    tareaBL.Insertar(tarea, SelectedTema.ID);
                }
                else
                {
                    SelectedTarea.Titulo = txtTitulo.Text;
                    SelectedTarea.Descripcion = txtDescripcion.Text;
                    SelectedTarea.Importancia = Convert.ToInt32(slImportancia.Value);
                    SelectedTarea.Responsable = ((UsuarioListViewItem)lvResponsableFormulario.SelectedItem).Usuario;
                    SelectedTarea.FechaEnCurso = SelectedTarea.FechaEnCurso;
                    SelectedTarea.FechaPorHacer = SelectedTarea.FechaPorHacer;
                    SelectedTarea.FechaFinalizada = SelectedTarea.FechaFinalizada;

                    tareaBL.ModificarDatos(SelectedTarea);
                }
                RefreshSelectedTema();

            }
            catch (Exception ex)
            {
                ModalHelpers.DisplayModal(ex.Message);
                return;
            }

        }

        private void RefreshSelectedTema()
        {
            var aux = SelectedTema;
            SelectedTema = null;
            SelectedTema = aux;

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DetailsMode(SelectedTarea);
        }


        private void DetailsMode(Tarea tarea)
        {
            isDetails = true;
            if (tarea != null)
            {
                spTareasFormulario.MaxHeight = 0;
                ClearFormInputs();
                spTareasDetails.MaxHeight = double.PositiveInfinity;
                tbDescripcion.Text = tarea.Descripcion;
                tbImportancia.Text = tarea.Importancia.ToString();
                tbResponsable.Text = tarea.Responsable.GetNombreCompleto();
                tbTitulo.Text = tarea.Titulo;
            }
            spTareasFormulario.MaxHeight = 0;
        }

        private void FormularioMode(bool isEdit, Tarea tarea)
        {
            isDetails = false;
            txtDescripcion.Text = "";
            txtTitulo.Text = "";
            slImportancia.Value = 1;
            lvResponsableFormulario.SelectedItem = null;
            spTareasFormulario.MaxHeight = double.PositiveInfinity;
            spTareasDetails.MaxHeight = 0;
            
           
            if (isEdit)
            {               
                txtTitulo.Text = tarea.Titulo;
                txtDescripcion.Text = tarea.Descripcion;
                slImportancia.Value = tarea.Importancia;

                foreach (UsuarioListViewItem item in lvResponsableFormulario.Items)
                {
                    if (tarea.Responsable.ID == item.Usuario.ID)
                    {
                        item.IsSelected = true;
                    }
                }
            }
        }

        private void LvKanban_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            SelectedTarea = (Tarea)e.Items[0];

            if (TareasPorHacer.Contains(SelectedTarea))
            {
                TareasFrom = TareasPorHacer;
            }
            else if (TareasEnCurso.Contains(SelectedTarea))
            {
                TareasFrom = TareasEnCurso;
            }
            else
            {
                TareasFrom = TareasFinalizadas;
            }

            DetailsMode(SelectedTarea);
        }

        private void LvKanban_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
        }

        private void LvPorHacer_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (SelectedTarea.Estado.GetType().Name == "Finalizada")
                {
                    SelectedTarea.Estado.Mover();
                }
                else if (SelectedTarea.Estado.GetType().Name == "EnCurso")
                {
                    SelectedTarea.Estado.MoverAtras();
                }
            }
            catch (Exception ex)
            {
                ModalHelpers.DisplayError(ex.Message);
            }
            TareasPorHacer.Add(SelectedTarea);
            TareasFrom.Remove(SelectedTarea);
            SelectedTarea.FechaPorHacer = DateTime.Now;
            SelectedTarea.FechaEnCurso = DateTime.Now;
            SelectedTarea.FechaFinalizada = DateTime.Now;

            tareaBL.ModificarEstado(SelectedTarea);
            RefreshSelectedTema();
        }

        private void LvEnCurso_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (SelectedTarea.Estado.GetType().Name == "PorHacer")
                {

                    SelectedTarea.Estado.Mover();

                }
                else if (SelectedTarea.Estado.GetType().Name == "Finalizada")
                {
                    SelectedTarea.Estado.MoverAtras();
                }
            }
            catch (Exception ex)
            {
                ModalHelpers.DisplayError(ex.Message);
            }
            TareasEnCurso.Add(SelectedTarea);
            TareasFrom.Remove(SelectedTarea);
            SelectedTarea.FechaEnCurso = DateTime.Now;
            SelectedTarea.FechaFinalizada = DateTime.Now;
            tareaBL.ModificarEstado(SelectedTarea);
            RefreshSelectedTema();
        }

        private void LvFinalizadas_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (SelectedTarea.Estado.GetType().Name == "EnCurso")
                {
                    SelectedTarea.Estado.Mover();
                }
                else if (SelectedTarea.Estado.GetType().Name == "PorHacer")
                {
                    SelectedTarea.Estado.MoverAtras();
                }
            }
            catch (Exception ex)
            {
                ModalHelpers.DisplayError(ex.Message);
            }
            TareasFinalizadas.Add(SelectedTarea);
            TareasFrom.Remove(SelectedTarea);
            SelectedTarea.FechaFinalizada = DateTime.Now;
            tareaBL.ModificarEstado(SelectedTarea);
            RefreshSelectedTema();
        }

        private void LvKanban_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedTarea = (Tarea)e.ClickedItem;
            DetailsMode(SelectedTarea);
        }

        private void ClearFormInputs()
        {
            txtComentario.Text = "";
            txtDescripcion.Text = "";
            txtTitulo.Text = "";
            slImportancia.Value = 1;
        } // Esta bien

        private void OrganizeTareasByState(List<Tarea> tareas)
        {
            TareasPorHacer.Clear();
            TareasEnCurso.Clear();
            TareasFinalizadas.Clear();
            foreach (var item in tareas)
            {
                if (item.Estado.GetType().Name == "PorHacer")
                {
                    TareasPorHacer.Add(item);
                }
                else if (item.Estado.GetType().Name == "EnCurso")
                {
                    TareasEnCurso.Add(item);
                }
                else
                {
                    TareasFinalizadas.Add(item);
                }
            }
        }
    }
}
