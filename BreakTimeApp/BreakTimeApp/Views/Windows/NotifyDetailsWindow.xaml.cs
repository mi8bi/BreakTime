using BreakTimeApp.ViewModels.Windows;

namespace BreakTimeApp.Views.Windows
{
    /// <summary>
    /// NotifyDetailsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class NotifyDetailsWindow
    {
        public NotifyDetailsWindowViewModel ViewModel { get; }

        public NotifyDetailsWindow(NotifyDetailsWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.CloseRequested += (s, e) => Close();
            DataContext = this;
            InitializeComponent();
        }

    }
}
