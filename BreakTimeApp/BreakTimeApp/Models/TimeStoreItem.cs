using BreakTimeApp.Helpers;
using BreakTimeApp.Services;
using BreakTimeApp.ViewModels.Pages;
using BreakTimeApp.ViewModels.Windows;
using BreakTimeApp.Views.Windows;
using System.Windows.Threading;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

namespace BreakTimeApp.Models
{
    public partial class TimeStoreItem : ObservableObject, IDisposable
    {
        /**
         * <summary>
         * 1秒間隔
         * </summary>
         */
        private static readonly double INTERVAL = 1;

        private readonly IServiceProvider _serviceProvider;
        private readonly WindowsProviderService _windowsProviderService;

        public DispatcherTimer Timer { get; set; }

        public Guid ID { get; set; }

        [ObservableProperty]
        public string _name;

        [ObservableProperty]
        public TimeSpan _span;

        [ObservableProperty]
        private bool _isRunning;

        [ObservableProperty]
        private SymbolRegular _icon;

        [ObservableProperty]
        private double _progress;

        [ObservableProperty]
        private double _maxProgress;

        public event EventHandler Disposed;
        public TimeStoreItem(
            IServiceProvider serviceProvider,
            WindowsProviderService windowsProviderService
            )
        {
            _serviceProvider = serviceProvider;
            _windowsProviderService = windowsProviderService;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(INTERVAL);
            Timer.Tick += Timer_Tick;
            Timer.Tick += ElapsedEventHandler;
        }

        async partial void OnNameChanged(string? oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                Name = oldValue;
                MessageBox.Show(Properties.MessageResources.InputError,
                    Properties.MessageResources.ErrorTitle,
                    System.Windows.MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            await UpdateItemInDatabase();
        }

        async partial void OnSpanChanged(TimeSpan value)
        {
            await UpdateItemInDatabase();
        }

        async partial void OnIsRunningChanged(bool value)
        {
            await UpdateItemInDatabase();
        }

        async partial void OnIconChanged(SymbolRegular value)
        {
            await UpdateItemInDatabase();
        }

        async partial void OnProgressChanged(double value)
        {
            await UpdateItemInDatabase();
        }

        async partial void OnMaxProgressChanged(double value)
        {
            await UpdateItemInDatabase();
        }

        private async Task UpdateItemInDatabase()
        {
            ITimeStoreItemDataService dataService = new TimeStoreItemDataService(new TimeStoreItemDbContext());
            TimeStoreItemDb itemDb = TimeStoreItemToDbConverter.Convert(this);

            // Databaseにitemが存在するかどうか確認
            if (await dataService.GetTimeStoreItemByIdAsync(itemDb.ID) != null)
            {
                // 存在する場合は更新を行う
                await dataService.UpdateTimeStoreItemAsync(itemDb);
            }
        }

        [LogAspect]
        public void Timer_Tick(object? sender, EventArgs e)
        {
            // プログレスバーの進捗
            Progress += INTERVAL;
            // Spanの表示を1秒減らす
            Span -= TimeSpan.FromSeconds(INTERVAL);
        }

        [LogAspect]
        public async void ElapsedEventHandler(object? sender, EventArgs e)
        {
            if (!IsRunning && Progress >= MaxProgress)
            {
                // Note: DbContextはスコープが違うのでインジェクションができない
                ISelectNotifyDataService dataService = new SelectNotifyDataService(new SelectNotifyDbContext());
                var item = await dataService.GetSelectNotifyItemByIdAsync(SelectNotifyViewModel.NOTIFY_ID);

                if (item.Mode == NotifyMode.Desktop)
                {
                    // デスクトップ通知
                    INotifyEvent notifyEventHandler = new ToastNotifyEvent(ID.ToString(), Name);
                    notifyEventHandler.Notify();
                }
                else if (item.Mode == NotifyMode.FullScreen)
                {
                    // フルスクリーン通知
                    // 1. FullScreenWindowのViewModelを取得してデータを設定
                    var viewModel = _serviceProvider.GetService(typeof(FullScreenWindowViewModel))
                        as FullScreenWindowViewModel;
                    viewModel.ImageSource = item.FilePath;
                    viewModel.CloseRequested += (s, e) =>
                    {
                        // アイテム削除
                        Disposed?.Invoke(this, EventArgs.Empty);
                    };
                    // 2. 1で設定したデータが入力されているのでウィンドウを表示
                    _windowsProviderService.ShowDialog<FullScreenWindow>();
                }

                Dispose();
            }
        }

        [LogAspect]
        internal void StartTimer()
        {
            if (!IsRunning)
                Timer.Start();
        }

        [LogAspect]
        internal void StopTimer()
        {
            if (IsRunning)
                Timer.Stop();
        }

        [LogAspect]
        public void Dispose()
        {
            Timer.Tick -= Timer_Tick;
            Timer.Tick -= ElapsedEventHandler;
        }
    }
}
