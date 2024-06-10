using BreakTimeApp.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace BreakTimeApp.Views.Pages
{
    public partial class NotifyPage : INavigableView<NotifyViewModel>
    {
        public NotifyViewModel ViewModel { get; }

        public NotifyPage(NotifyViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

    }
}
