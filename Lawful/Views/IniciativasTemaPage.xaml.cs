using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using Lawful.Core.Modelo.Iniciativas;
using Lawful.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lawful.Views
{
    public sealed partial class IniciativasTemaPage : Page, INotifyPropertyChanged
    {
        private IniciativaBL iniciativaBL;
        private TemaBL temaBL;
        private Iniciativa _selected;
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
            Iniciativas = new ObservableCollection<Iniciativa>();
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
            lvOpciones.ItemsSource = null;
            switch (_selected.GetType().Name)
            {
                case "Asistire":
                    lvOpciones.ItemsSource = ((Asistire)_selected).Opciones;
                    tbFechaEvento.Text = ((Asistire)_selected).FechaEvento.ToString();
                    tbFechaLimiteConfirmacion.Text = ((Asistire)_selected).FechaLimiteConfirmacion.ToString();
                    tbLugar.Text = ((Asistire)_selected).Lugar;
                    spFormulario.MaxHeight = 0;
                    spDetailDoDont.MaxHeight = 0;
                    spDetailRegla.MaxHeight = 0;
                    spDetailVotacionMultiple.MaxHeight = 0;
                    break;
                default:
                    break;
            }
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void btnGuardar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void btnVotar_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
