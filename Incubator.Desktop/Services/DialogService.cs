using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Incubator.Desktop.Services
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string title, string message) 
        {
            // Usamos el MessageBox nativo de WPF
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool ConfirmationMessage(string title, string message)
        {
            // Mostramos un diálogo con botones Sí/No y un ícono de pregunta
            var resultado = MessageBox.Show(
                message,
                title,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            // Retorna true si el usuario hizo clic en "Sí"
            return resultado == MessageBoxResult.Yes;
        }
    }
}
