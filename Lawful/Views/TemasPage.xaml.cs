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

namespace Lawful.Views
{
    public sealed partial class TemasPage : Page, INotifyPropertyChanged
    {
        private UsuarioBL usuarioBL;
        private Tema _selected;
        public Tema Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<Tema> Temas { get; private set; } = new ObservableCollection<Tema>();

        public TemasPage()
        {
            usuarioBL = new UsuarioBL();
            InitializeComponent();
            Loaded += TemasPage_Loaded;
        }

        private void TemasPage_Loaded(object sender, RoutedEventArgs e)
        {
            Temas.Clear();

            var data = usuarioBL.ListarTemasDisponibles(1);

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
