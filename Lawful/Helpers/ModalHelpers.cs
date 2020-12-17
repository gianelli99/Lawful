using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Lawful.Helpers
{
    public class ModalHelpers
    {
        public static async System.Threading.Tasks.Task<ContentDialogResult> DisplayDeleteConfirmation()
        {
            ContentDialog noItemSelected = new ContentDialog
            {
                Title = "Atención",
                Content = "¿Está seguro que desea eliminarlo?",
                PrimaryButtonText = "Si",
                SecondaryButtonText = "No"
            };

            ContentDialogResult result = await noItemSelected.ShowAsync();
            return result;
        }

        public static async void DisplayError(string errorM)
        {
            ContentDialog error = new ContentDialog
            {
                Title = "Error",
                Content = errorM,
                CloseButtonText = "Ok"
            };

            await error.ShowAsync();
        }

        public static async void DisplayModal(string message, string title = "Error")
        {
            ContentDialog error = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Ok"
            };

            await error.ShowAsync();
        }

        public static async System.Threading.Tasks.Task<ContentDialogResult> DisplayConfirmation(string title, string content)
        {
            ContentDialog confirmation = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "Si",
                SecondaryButtonText = "No"
            };

            ContentDialogResult result = await confirmation.ShowAsync();
            return result;
        }
    }
}
