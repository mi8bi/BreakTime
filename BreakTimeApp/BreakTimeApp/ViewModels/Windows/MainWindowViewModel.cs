using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace BreakTimeApp.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            ApplicationTitle = GetResource<string>("main_window_title");
            SearchText = GetResource<string>("main_window_search");
            MenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem()
                {
                    Content = GetResource<string>("main_window_notify"),
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                    TargetPageType = typeof(Views.Pages.NotifyPage)
                },
                new NavigationViewItem()
                {
                    Content = GetResource<string>("main_window_data"),
                    Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                    TargetPageType = typeof(Views.Pages.DataPage)
                },
            };

            FooterMenuItems = new ObservableCollection<object>
            {
                new NavigationViewItem()
                {
                    Content = GetResource<string>("main_window_settings"),
                    Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                    TargetPageType = typeof(Views.Pages.SettingsPage)
                }
            };

        }

        [ObservableProperty]
        private string _applicationTitle;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<object> _menuItems;

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems;

        private T GetResource<T>(string key)
        {
            return (T)Application.Current.Resources[key];
        }
    }
}
