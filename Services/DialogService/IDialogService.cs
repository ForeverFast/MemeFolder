namespace MemeFolder.Services
{
    public interface IDialogService
    {
        void ShowMessage(string text);
        void ShowFolder(string path);
        string FileBrowserDialog(string Extension = "*.jpg;*.png");
        string FolderBrowserDialog();
    }
}
