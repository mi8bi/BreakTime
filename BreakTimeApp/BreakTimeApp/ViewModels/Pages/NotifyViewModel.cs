using BreakTimeApp.Models;
using BreakTimeApp.Services;
using BreakTimeApp.Views.Windows;
using System.Collections.ObjectModel;

namespace BreakTimeApp.ViewModels.Pages
{
    public partial class NotifyViewModel : ObservableObject
    {
        private readonly WindowsProviderService _windowsProviderService;

        //「1時間後」を表すTimeSpanオブジェクトを作成
        private readonly TimeSpan DEFAULT_TIMESPAN = new TimeSpan(1, 0, 0);

        [ObservableProperty]
        private ObservableCollection<TimeStoreItem> _items = new ObservableCollection<TimeStoreItem>();

        [ObservableProperty]
        private TimeStoreItem _selectedItem;

        public NotifyViewModel(WindowsProviderService windowsProviderService)
        {
            _windowsProviderService = windowsProviderService;
        }

        [RelayCommand]
        private void OnAdd()
        {
            // 新しいアイテムを追加
            Items.Add(new TimeStoreItem
            {
                Start = DateTime.Now,
                End = DateTime.Now + DEFAULT_TIMESPAN,
                Span = DEFAULT_TIMESPAN
            });
        }

        [RelayCommand]
        private void OnRemoveItem(TimeStoreItem item)
        {
            if (item != null && Items.Contains(item))
                Items.Remove(item);
        }

        [RelayCommand]
        private void OnEditItem(TimeStoreItem item)
        {
            if (item != null && Items.Contains(item))
            {
                var window = _windowsProviderService.GetWindow<NotifyDetailsWindow>();
                window.Owner = Application.Current.MainWindow;
                window.ViewModel.Item = item;
                window.ShowDialog();
            }
        }
    }
}
