namespace Incubator.Desktop.Services
{
    public interface IDialogService
    {
        // Para alertas simples o errores
        void ShowMessage(string title, string message);

        // Para preguntas de Sí/No
        bool ConfirmationMessage(string title, string message);
    }
}
