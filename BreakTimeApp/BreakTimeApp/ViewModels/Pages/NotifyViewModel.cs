using BreakTimeApp.Helpers;
using BreakTimeApp.Models;
using BreakTimeApp.Services;
using BreakTimeApp.ViewModels.Windows;
using BreakTimeApp.Views.Windows;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Uwp.Notifications;
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

        //「1分後」を表すTimeSpanオブジェクトを作成
        private readonly TimeSpan DEFAULT_TIMESPAN = new TimeSpan(0, 1, 0);

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
            _items = new ObservableCollection<TimeStoreItem>();
            LoadItemsAsync().Wait();
            ToastEventHandler();
        }

        private void ToastEventHandler()
        {
            // TODO: トースト処理を別クラスに記述
            // トースト通知がアクティブ化されたときに呼び出されるイベントハンドラを設定
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                // トースト通知のアクティブ化の詳細を取得
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                Application.Current.Dispatcher.Invoke(async () => {
                    // TimeStoreItemのguidからItemを取得
                    string guid = args[Properties.ToastResources.guid];
                    TimeStoreItem? item = Items.FirstOrDefault(findItem => findItem.ID.ToString() == guid);
                    if (item == null) return Task.CompletedTask;

                    // トースト通知がアクティブ化されたときの処理
                    try
                    {
                        string action = args[Properties.ToastResources.action];
                        if (action.Equals(Properties.ToastResources.accept))
                        {
                            await OnRemoveItem(item);
                        }
                        else if (action.Equals(Properties.ToastResources.snooze) ||
                        action.Equals(Properties.ToastResources.dismiss))
                        {
                            item.Progress = 0;
                            item.Span = TimeSpan.FromSeconds(item.MaxProgress);
                            item.Timer.Tick += item.Timer_Tick;
                            item.Timer.Tick += item.Toast_ElapsedEventHandler;
                            item.IsRunning = action == Properties.ToastResources.dismiss;
                            OnToggleItem(item);
                        }
                    }
                    catch
                    {
                        await OnRemoveItem(item);
                    }

                    return Task.CompletedTask;
                });
            };
        }

        [LogAspect]
        private async Task LoadItemsAsync()
        {
            var itemsDb = await _dataService.GetAllTimeStoreItemsAsync();
            foreach (var itemDb in itemsDb)
            {
                TimeStoreItem item = TimeStoreItemToDbConverter.ConvertBack(itemDb);
                item.Disposed += TimeStoreItem_Disposed;
                Items.Add(item);
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
        private void OnAddItem()
        {
            // 新しいアイテムを追加
            TimeStoreItem newItem = App.GetService<TimeStoreItem>();
            newItem.ID = Guid.NewGuid();
            newItem.Name = $"{Properties.NotifyPageResources.item_title} {Items.Count + 1}";
            newItem.Span = DEFAULT_TIMESPAN;
            newItem.IsRunning = true;
            newItem.Icon = SymbolRegular.TriangleRight20;
            newItem.Progress = 0;
            newItem.MaxProgress = DEFAULT_TIMESPAN.TotalSeconds;
            // 破棄イベントの処理内容を登録
            newItem.Disposed += TimeStoreItem_Disposed;

            AddItem(newItem);
        }

        [LogAspect]
        private async Task AddItem(TimeStoreItem newItem)
        {
            Items.Add(newItem);

            await _dataService.AddTimeStoreItemAsync(TimeStoreItemToDbConverter.Convert(newItem));
            _logger.LogInformation(AppLogEvents.UserAction, "item add");
        }

        [LogAspect]
        [RelayCommand(CanExecute = nameof(canExecuteItem))]
        private async Task OnRemoveItem(TimeStoreItem item)
        {
            // IDが同じものを調べる
            var findItem = Items.FirstOrDefault(findItem => findItem.ID == item.ID);
            if (findItem == null) return;
            // 見つかった場合は削除する
            Items.Remove(findItem);
            await _dataService.DeleteTimeStoreItemAsync(item.ID.ToString());

            _logger.LogInformation(AppLogEvents.UserAction, "item remove");
        }

        private void TimeStoreItem_Disposed(object sender, EventArgs e)
        {
            if (sender is TimeStoreItem item)
            {
                // UI表示からitemを削除する。(DBからは削除しない)
                // ToastEventHandlerメソッドでitemの処理を決める。
                // Items.Remove(item);
                // DBとUI表示から削除
                // OnRemoveItem(item);
            }
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
