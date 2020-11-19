using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Lawful.Views
{
    public class MessageContainerStyleSelector : StyleSelector
    {
        public Style SentStyle { get; set; }

        public Style ReceivedStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var message = item as Mensaje;
            if (message != null)
            {
                return message.Emisor.ID == SesionActiva.ObtenerInstancia().Usuario.ID
                           ? this.SentStyle
                           : this.ReceivedStyle;
            }

            return base.SelectStyleCore(item, container);
        }
    }
}
