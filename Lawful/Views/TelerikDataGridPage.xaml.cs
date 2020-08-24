using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Lawful.Core.Models;
using Lawful.Core.Services;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Lawful.Views
{
    public sealed partial class TelerikDataGridPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on TelerikDataGridPage.xaml.
        // For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        // You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
        public TelerikDataGridPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Source.Clear();

            // TODO WTS: Replace this with your actual data
            var data = await SampleDataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
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
