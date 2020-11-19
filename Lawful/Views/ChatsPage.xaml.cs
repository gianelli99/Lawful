using Lawful.Core.Logica;
using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lawful.Views
{
    public sealed partial class ChatsPage : Page, INotifyPropertyChanged
    {
        private UsuarioBL usuarioBL;
        private ChatBL chatBL;
        public ObservableCollection<Usuario> Usuarios { get; set; }
        public List<Mensaje> Chat { get; set; }
        private Usuario _selected;
        public Usuario Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }
        public ChatsPage()
        {
            Usuarios = new ObservableCollection<Usuario>();
            InitializeComponent();
            usuarioBL = new UsuarioBL();
            chatBL = new ChatBL();
            var data = usuarioBL.Listar();
            int myId = SesionActiva.ObtenerInstancia().Usuario.ID;
            foreach (var item in data)
            {
                if (item.ID != myId)
                {
                    Usuarios.Add(item);
                }
            }
            if (Usuarios.Count > 0)
            {
                Selected = Usuarios[0];
                Chat = chatBL.ObtenerChat(SesionActiva.ObtenerInstancia().Usuario.ID, Selected.ID);
                OnPropertyChanged("Chat");
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
            if (Usuarios.Count > 0)
            {
                Chat = chatBL.ObtenerChat(SesionActiva.ObtenerInstancia().Usuario.ID, Selected.ID);
                OnPropertyChanged("Chat");
            }
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void UsuariosListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Chat = chatBL.ObtenerChat(SesionActiva.ObtenerInstancia().Usuario.ID, Selected.ID);
            OnPropertyChanged("Chat");
        }

        private void txtMensaje_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !String.IsNullOrWhiteSpace(txtMensaje.Text))
            {
                var mensaje = new Mensaje()
                {
                    Emisor = new Usuario() { ID = SesionActiva.ObtenerInstancia().Usuario.ID },
                    Receptor = Selected,
                    Fecha = DateTime.Now,
                    Texto = txtMensaje.Text
                };
                txtMensaje.Text = "";
                chatBL.EnviarMensaje(mensaje);

                Chat = chatBL.ObtenerChat(SesionActiva.ObtenerInstancia().Usuario.ID, Selected.ID);
                OnPropertyChanged("Chat");
            }
        }
    }
}
