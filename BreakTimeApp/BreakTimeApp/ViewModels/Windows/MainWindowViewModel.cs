using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace BreakTimeApp.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<object> _menuItems;

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems;

        public MainWindowViewModel()
        {
            ApplicationTitle = Properties.MainWindowResources.title;
            SearchText = Properties.MainWindowResources.search;
            MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem()
                {
                    Content = Properties.MainWindowResources.notify,
                    Icon = new SymbolIcon { Symbol = SymbolRegular.ClockAlarm24 },
                    TargetPageType = typeof(Views.Pages.NotifyPage)
                },
                new NavigationViewItem()
                {
                    Content = Properties.MainWindowResources.data,
                    Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                    TargetPageType = typeof(Views.Pages.DataPage)
                },
            };

            FooterMenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem()
                {
                    Content = Properties.MainWindowResources.settings,
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                    TargetPageType = typeof(Views.Pages.SettingsPage)
                }
            };

        }

    }
}
