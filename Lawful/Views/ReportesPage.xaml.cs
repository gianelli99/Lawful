using Lawful.Core.Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace Lawful.Views
{
    public sealed partial class ReportesPage : Page, INotifyPropertyChanged
    {
        private ReporteBL reporteBL;
        public ReportesPage()
        {
            reporteBL = new ReporteBL();
            InitializeComponent();
            LoadChartContents();
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

        private void LoadChartContents()
        {

            var sesiones = reporteBL.ObtenerUltimasSesiones(1);
            Random rand = new Random();
            List<Records> records = new List<Records>();
            records.Add(new Records()
            {
                Name = "Suresh",
                Amount = rand.Next(0, 200)
            });
            records.Add(new Records()
            {
                Name = "Juancho",
                Amount = rand.Next(0, 200)
            });
            records.Add(new Records()
            {
                Name = "Sam",
                Amount = rand.Next(0, 200)
            });
            records.Add(new Records()
            {
                Name = "Sri",
                Amount = rand.Next(0, 200)
            });
            (PieChart.Series[0] as PieSeries).ItemsSource = records;
            (ColumnChart.Series[0] as ColumnSeries).ItemsSource = records;
            (lcSesionesUsuario.Series[0] as LineSeries).ItemsSource = sesiones;
            lcSesionesUsuario.Series[0].LegendItems.Clear();
            ((LineSeries)lcSesionesUsuario.Series[0]).DependentRangeAxis = new LinearAxis()
            {
                //Maximum = 9,
                Minimum = 0,
                Orientation = AxisOrientation.Y,
                Interval = 1,
                ShowGridLines = true,
            };
            (ColumnChart2.Series[0] as ColumnSeries).ItemsSource = records;
        }
    }
    public class Records
    {
        public string Name
        {
            get;
            set;
        }
        public int Amount
        {
            get;
            set;
        }
    }
}
