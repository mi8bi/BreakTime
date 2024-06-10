using BreakTimeApp.ViewModels.Windows;

namespace BreakTimeApp.Views.Windows
{
    /// <summary>
    /// FullScreenWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FullScreenWindow
    {
        public FullScreenWindowViewModel ViewModel { get; }

        public FullScreenWindow(FullScreenWindowViewModel viewModel)
        {
            ViewModel = viewModel;
            ViewModel.CloseRequested += (s, e) => Close();
            DataContext = this;
            InitializeComponent();
        }

    }
}
