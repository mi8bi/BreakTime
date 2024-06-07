namespace BreakTimeApp.ViewModels.Windows
{
    public partial class FullScreenWindowViewModel
    {
        public string ImageSource { get; set; }

        public event EventHandler CloseRequested;

        [RelayCommand]
        private void OnClose()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
