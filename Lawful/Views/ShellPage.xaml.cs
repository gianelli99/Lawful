using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Lawful.Helpers;
using Lawful.Services;

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;
using Lawful.Core.Modelo;
using Lawful.Core.Logica;
using Windows.ApplicationModel.Email.DataProvider;
using System.Diagnostics;


namespace Lawful.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {

        private ShellPage instancia;
        public ShellPage ObtenerInstancia()
        {
            if (instancia == null)
            {
                instancia = new ShellPage();
            }
            return instancia;
        }

        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        SesionBL sesionBL;
        UsuarioBL usuarioBL;
        List<Vista> vistas;
        


        private bool _isBackEnabled;
        private WinUI.NavigationViewItem _selected;
        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }

        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = this;
            sesionBL = SesionBL.ObtenerInstancia();
            usuarioBL = new UsuarioBL();
            Initialize();
            shellFrame.Navigate(typeof(MainPage));

            vistas = usuarioBL.ListarVistasDisponibles(SesionActiva.ObtenerInstancia().Usuario.ID);
            GenerateNavigationViewItems(vistas);
            

        }
        private void GenerateNavigationViewItems(List<Vista> vistas)
        {

            foreach (var vista in vistas)
            {
                WinUI.NavigationViewItem item = new WinUI.NavigationViewItem()
                {
                    Content = vista.Descripcion,
                    Name = vista.ID.ToString(),
                    Icon = new SymbolIcon((Symbol)Enum.Parse(typeof(Symbol), vista.IconName)),
                };
                Type type = Type.GetType("Lawful.Views." + vista.AssociatedViewName);
                NavHelper.SetNavigateTo(item, type);
                navigationView.MenuItems.Add(item);
            }
        }
        private void Initialize()
        {
            NavigationService.Frame = shellFrame;
            NavigationService.NavigationFailed += Frame_NavigationFailed;
            NavigationService.Navigated += Frame_Navigated;
            navigationView.BackRequested += OnBackRequested;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            KeyboardAccelerators.Add(_altLeftKeyboardAccelerator);
            KeyboardAccelerators.Add(_backKeyboardAccelerator);
            await Task.CompletedTask;
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            

            IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            var selectedItem = GetSelectedItem(navigationView.MenuItems, e.SourcePageType);
            if (selectedItem != null)
            {
                if (e.SourcePageType.ToString() == "Lawful.Views.MisDatosPage") {
                    FrameGlobal.FrameEstatico = this.Frame;
                }
                Selected = selectedItem;
            }


        }

        private WinUI.NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (var item in menuItems.OfType<WinUI.NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }

                var selectedChild = GetSelectedItem(item.MenuItems, pageType);
                if (selectedChild != null)
                {
                    return selectedChild;
                }
            }

            return null;
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        private void OnItemInvoked(WinUI.NavigationView sender, WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(SettingsPage), null, args.RecommendedNavigationTransitionInfo);
                return;
            }

            if (args.InvokedItemContainer is WinUI.NavigationViewItem selectedItem)
            {
                var pageType = selectedItem.GetValue(NavHelper.NavigateToProperty) as Type;
                NavigationService.Navigate(pageType, null, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void Btn_CerrarSesion_Tapped(object sender, TappedRoutedEventArgs e)
        {
            sesionBL.FinalizarSesion();
            Frame.Navigate(typeof(LoginPage));
        }

    }
}
