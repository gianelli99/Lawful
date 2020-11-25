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
        public ObservableCollection<Tema> Temas { get; set; }
        public List<Mensaje> Chat { get; set; }
        private Usuario _userSelected;
        private Tema _temaSelected;
        private bool inTema;
        public Tema TemaSelected
        {
            get { return _temaSelected; }
            set { Set(ref _temaSelected, value); }
        }
        public Usuario UserSelected
        {
            get { return _userSelected; }
            set { Set(ref _userSelected, value); }
        }
        public ChatsPage()
        {
            Usuarios = new ObservableCollection<Usuario>();
            Temas = new ObservableCollection<Tema>();
            InitializeComponent();
            usuarioBL = new UsuarioBL();
            var temas = usuarioBL.ListarTemasDisponibles(SesionActiva.ObtenerInstancia().Usuario.ID);
            foreach (var item in temas)
            {
                Temas.Add(item);
            }

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
            if (Temas.Count>0)
            {
                TemaSelected = Temas[0];
            }
            if (Usuarios.Count > 0)
            {
                UserSelected = Usuarios[0];
                Chat = chatBL.ObtenerChat(SesionActiva.ObtenerInstancia().Usuario.ID, UserSelected.ID);
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
            if (value.GetType().Name == "Tema")
            {
                inTema = true;
            }
            else
            {
                inTema = false;
            }
            CargarChat();
            
            OnPropertyChanged(propertyName);
        }
        private void CargarChat()
        {
            if (inTema)
            {
                if (Temas.Count > 0)
                {
                    tbTitulo.Text = TemaSelected.Titulo;
                    Chat = chatBL.ObtenerChat(TemaSelected.ID);
                    OnPropertyChanged("Chat");
                }
            }
            else
            {
                if (Usuarios.Count > 0)
                {
                    tbTitulo.Text = UserSelected.GetNombreCompleto();
                    Chat = chatBL.ObtenerChat(SesionActiva.ObtenerInstancia().Usuario.ID, UserSelected.ID);
                    OnPropertyChanged("Chat");
                }
            }
           
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void UsuariosListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var user = UserSelected;
            _userSelected = new Usuario();
            UserSelected = user;
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            CargarChat();
        }

        private void txtMensaje_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !String.IsNullOrWhiteSpace(txtMensaje.Text))
            {
                Mensaje mensaje;
                if (inTema)
                {
                    mensaje = new MensajeATema();
                    ((MensajeATema)mensaje).Receptor = TemaSelected;
                }
                else
                {
                    mensaje = new MensajeAUsuario();
                    ((MensajeAUsuario)mensaje).Receptor = UserSelected;
                }
                mensaje.Emisor = new Usuario() { ID = SesionActiva.ObtenerInstancia().Usuario.ID };
                mensaje.Fecha = DateTime.Now;
                mensaje.Texto = txtMensaje.Text;
                txtMensaje.Text = "";

                chatBL.EnviarMensaje(mensaje);

                if (inTema)
                {
                    Chat = chatBL.ObtenerChat(TemaSelected.ID);
                }
                else
                {
                    Chat = chatBL.ObtenerChat(SesionActiva.ObtenerInstancia().Usuario.ID, UserSelected.ID);
                }  
                OnPropertyChanged("Chat");
            }
        }

        private void TemasListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var tema = TemaSelected;
            _temaSelected = new Tema();
            TemaSelected = tema;
        }
    }
}
