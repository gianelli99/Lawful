using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Lawful.Core.Modelo;
namespace Lawful.Views
{
    public sealed partial class TemasDetailControl : UserControl
    {
        public Tema MasterMenuItem
        {
            get { return GetValue(MasterMenuItemProperty) as Tema; }
            set { SetValue(MasterMenuItemProperty, value); }
        }

        public static readonly DependencyProperty MasterMenuItemProperty = DependencyProperty.Register("MasterMenuItem", typeof(Tema), typeof(TemasDetailControl), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));

        public TemasDetailControl()
        {
            InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TemasDetailControl;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
