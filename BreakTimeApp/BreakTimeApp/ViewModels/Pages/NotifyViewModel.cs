using BreakTimeApp.Models;
using BreakTimeApp.Services;
using BreakTimeApp.ViewModels.Windows;
using BreakTimeApp.Views.Windows;
using System.Collections.ObjectModel;

namespace BreakTimeApp.ViewModels.Pages
{
    public partial class NotifyViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WindowsProviderService _windowsProviderService;

        //「1時間後」を表すTimeSpanオブジェクトを作成
        private readonly TimeSpan DEFAULT_TIMESPAN = new TimeSpan(1, 0, 0);

        [ObservableProperty]
        private ObservableCollection<TimeStoreItem> _items = new ObservableCollection<TimeStoreItem>();

        [ObservableProperty]
        private TimeStoreItem _selectedItem;

        public NotifyViewModel(
            IServiceProvider serviceProvider,
            WindowsProviderService windowsProviderService)
        {
            _serviceProvider = serviceProvider;
            _windowsProviderService = windowsProviderService;
        }

        [RelayCommand]
        private void OnAdd()
        {
            // 新しいアイテムを追加
            Items.Add(new TimeStoreItem
            {
                Guid = Guid.NewGuid(),
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
                // 1. NotifyDetailsWindowのViewModelを取得してデータを設定
                var viewModel = _serviceProvider.GetService(typeof(NotifyDetailsWindowViewModel)) as NotifyDetailsWindowViewModel;
                viewModel.Item = item;
                viewModel.SelectedHours = item.Span.Hours;
                viewModel.SelectedMinutes = item.Span.Minutes;
                viewModel.SelectedSeconds = item.Span.Seconds;
                // 2. 1で設定したデータが入力されているのでウィンドウを表示
                _windowsProviderService.ShowDialog<NotifyDetailsWindow>();
            }
        }
    }
}
