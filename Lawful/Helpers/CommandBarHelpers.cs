using Lawful.Core.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Lawful.Helpers
{
    public class CommandBarHelpers
    {
        public static CommandBar CreateCommandBar(CommandBar commandBar, List<Accion> acciones, Windows.UI.Xaml.RoutedEventHandler Accion_Click)
        {
            foreach (var button in CreateAppBarButtons(acciones, Accion_Click))
            {
                commandBar.PrimaryCommands.Add(button);
            }
            return commandBar;
        }

        public static List<AppBarButton> CreateAppBarButtons(List<Accion> acciones, Windows.UI.Xaml.RoutedEventHandler Accion_Click)
        {
            List<AppBarButton> appBarButtons = new List<AppBarButton>();
            foreach (var accion in acciones)
            {
                AccionAppBarButton appBarButton = new AccionAppBarButton()
                {
                    Name = accion.ID.ToString(),
                    Label = accion.Descripcion,
                    Icon = new SymbolIcon((Symbol)Enum.Parse(typeof(Symbol), accion.IconName)),
                    Accion = accion
                };
                appBarButton.Click += Accion_Click;
                appBarButtons.Add(appBarButton);
            }
            return appBarButtons;

        }
    }
}
