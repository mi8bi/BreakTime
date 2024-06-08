using BreakTimeApp.Models;
using BreakTimeApp.Services;
using BreakTimeApp.ViewModels.Pages;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection.Metadata;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace BreakTimeApp.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string _applicationTitle;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private ObservableCollection<object> _menuItems;

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems;

        public MainWindowViewModel(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
                    Content = Properties.MainWindowResources.select_notify,
                    Icon = new SymbolIcon { Symbol = SymbolRegular.MultiselectLtr24 },
                    TargetPageType = typeof(Views.Pages.SelectNotifyPage)
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

            initializeDb();
        }

        private void initializeDb()
        {
            initializeSelectNotifyDb();
        }

        private async void initializeSelectNotifyDb()
        {
            ISelectNotifyDataService dataService = new SelectNotifyDataService(new SelectNotifyDbContext());
            SelectNotifyDb selectNotifyDb = 
                await dataService.GetSelectNotifyItemByIdAsync(SelectNotifyViewModel.NOTIFY_ID);
            if (selectNotifyDb == null)
            {
                var newSelectNotifyDb = _serviceProvider.GetService(typeof(SelectNotifyDb)) as SelectNotifyDb;
                newSelectNotifyDb.Id = SelectNotifyViewModel.NOTIFY_ID;
                newSelectNotifyDb.Mode = NotifyMode.Desktop;
                newSelectNotifyDb.FilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "Images", "default_wallpaper.jpg");
                await dataService.AddSelectNotifyItemAsync(newSelectNotifyDb);
            }
            else if(!File.Exists(selectNotifyDb.FilePath))
            {
                var newSelectNotifyDb = _serviceProvider.GetService(typeof(SelectNotifyDb)) as SelectNotifyDb;
                newSelectNotifyDb.Id = SelectNotifyViewModel.NOTIFY_ID;
                newSelectNotifyDb.Mode = NotifyMode.Desktop;
                newSelectNotifyDb.FilePath = Path.Combine(AppContext.BaseDirectory, "Assets", "Images", "default_wallpaper.jpg");
                await dataService.UpdateSelectNotifyItemAsync(newSelectNotifyDb);
            }

        }
    }
}
