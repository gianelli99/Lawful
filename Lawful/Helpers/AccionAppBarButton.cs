using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Lawful.Helpers
{
    public class AccionAppBarButton : AppBarButton
    {
        public Accion Accion { get; set; }
    }
}
