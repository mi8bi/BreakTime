using BreakTimeApp.Helpers;
using BreakTimeApp.Models;
using BreakTimeApp.Services;
using BreakTimeApp.ViewModels.Windows;
using BreakTimeApp.Views.Windows;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace BreakTimeApp.ViewModels.Pages
{
    public partial class NotifyViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotifyViewModel> _logger;
        private readonly ITimeStoreItemDataService _dataService;
        private readonly WindowsProviderService _windowsProviderService;

        //「1時間後」を表すTimeSpanオブジェクトを作成
        private readonly TimeSpan DEFAULT_TIMESPAN = new TimeSpan(1, 0, 0);

        [ObservableProperty]
        private ObservableCollection<TimeStoreItem> _items;

        public NotifyViewModel(
            IServiceProvider serviceProvider,
            ILogger<NotifyViewModel> logger,
            WindowsProviderService windowsProviderService)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            // Note: TimeStoreItemDataService(及びDbContext)はスコープが違うのでインジェクションができない
            _dataService = new TimeStoreItemDataService(new TimeStoreItemDbContext());
            _windowsProviderService = windowsProviderService;
            _ = LoadItemsAsync();
        }

        [LogAspect]
        private async Task LoadItemsAsync()
        {
            var items = await _dataService.GetAllTimeStoreItemsAsync();
            Items = new ObservableCollection<TimeStoreItem>();
            foreach (var item in items)
            {
                Items.Add(TimeStoreItemToDbConverter.ConvertBack(item));
            }
            _logger.LogInformation(AppLogEvents.UserAction, "items loaded");
        }

        [LogAspect]
        private bool canExecuteItem (TimeStoreItem item)
        {
            return item != null && Items.Contains(item);
        }

        [LogAspect]
        [RelayCommand]
        private async Task OnAddItem()
        {
            // 新しいアイテムを追加
            TimeStoreItem newItem = new TimeStoreItem
            {
                ID = Guid.NewGuid(),
                Start = DateTime.Now,
                End = DateTime.Now + DEFAULT_TIMESPAN,
                Span = DEFAULT_TIMESPAN,
                IsRunning = true,
                Icon = SymbolRegular.TriangleRight20
            };
            Items.Add(newItem);

            await _dataService.AddTimeStoreItemAsync(TimeStoreItemToDbConverter.Convert(newItem));
            _logger.LogInformation(AppLogEvents.UserAction, "item add");
        }

        [LogAspect]
        [RelayCommand(CanExecute = nameof(canExecuteItem))]
        private async Task OnRemoveItem(TimeStoreItem item)
        {
            Items.Remove(item);

            await _dataService.DeleteTimeStoreItemAsync(item.ID.ToString());
            _logger.LogInformation(AppLogEvents.UserAction, "item remove");
        }

        [LogAspect]
        [RelayCommand(CanExecute = nameof(canExecuteItem))]
        private void OnEditItem(TimeStoreItem item)
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

        [LogAspect]
        [RelayCommand(CanExecute = nameof(canExecuteItem))]
        private void OnToggleItem(TimeStoreItem item)
        {
            if (item.IsRunning)
            {
                // 停止
                item.Icon = SymbolRegular.TriangleRight20;
                item.StopTimer();
            }
            else
            {
                // 開始
                item.Icon = SymbolRegular.Stop20;
                item.StartTimer();
            }
        }
    }
}
