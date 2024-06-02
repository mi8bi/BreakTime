using BreakTimeApp.Helpers;
using BreakTimeApp.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using System.Windows.Threading;
using Windows.UI.Notifications;
using Wpf.Ui.Controls;

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

        private readonly DispatcherTimer _timer;

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

        /**
         * <summary>
         * 破棄されたことをイベント通知
         * </summary>
         */
        public event EventHandler Disposed;

        public TimeStoreItem()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(INTERVAL);
            _timer.Tick += timer_Tick;
            _timer.Tick += toast_ElapsedEventHandler;
        }

        partial void OnNameChanged(string? oldValue, string newValue)
        {
            _ = UpdateItemInDatabase();
        }

        partial void OnSpanChanged(TimeSpan value)
        {
            _ = UpdateItemInDatabase();
        }

        partial void OnIsRunningChanged(bool value)
        {
            _ = UpdateItemInDatabase();
        }

        partial void OnIconChanged(SymbolRegular value)
        {
            _ = UpdateItemInDatabase();
        }

        partial void OnProgressChanged(double value)
        {
            _ = UpdateItemInDatabase();
        }

        private async Task UpdateItemInDatabase()
        {
            ITimeStoreItemDataService dataService = new TimeStoreItemDataService(new TimeStoreItemDbContext());
            await dataService.UpdateTimeStoreItemAsync(TimeStoreItemToDbConverter.Convert(this));
        }

        [LogAspect]
        private void timer_Tick(object? sender, EventArgs e)
        {
            // プログレスバーの進捗
            Progress += INTERVAL;
            // Spanの表示を1秒減らす
            Span -= TimeSpan.FromSeconds(INTERVAL);
        }

        [LogAspect]
        private void toast_ElapsedEventHandler(object? sender, EventArgs e)
        {
            if (!IsRunning && Progress >= MaxProgress)
            {
#if WINDOWS10_0_17763_0_OR_GREATER
                // Toastを組み立てる
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string acceptImagePath = Path.Combine(baseDirectory, "Assets/Images/ic_fluent_checkmark_24_filled.png");
                string snoozeImagePath = Path.Combine(baseDirectory, "Assets/Images/ic_fluent_snooze_24_filled.png");
                string dismissImagePath = Path.Combine(baseDirectory, "Assets/Images/ic_fluent_dismiss_24_filled.png");
                var toastContent = new ToastContentBuilder()
                    .AddText("New product in stock!")
                    // TODO: AddArgumentにGuidを渡す
                    .AddArgument("guid", ID.ToString())
                    .AddButton(new ToastButton()
                        .SetContent("Accept")
                        .AddArgument("action", "accept")
                        .SetImageUri(new Uri(acceptImagePath)))
                    .AddButton(new ToastButton()
                        .SetContent("Snooze")
                        .AddArgument("action", "snooze")
                        .SetImageUri(new Uri(snoozeImagePath)))
                    .AddButton(new ToastButton()
                        .SetContent("Dismiss")
                        .AddArgument("action", "dismiss")
                        .SetImageUri(new Uri(dismissImagePath)))
                    .GetToastContent();
                // Toastのレイアウトを作成
                var toast = new ToastNotification(toastContent.GetXml());
                // Toast表示
                ToastNotificationManagerCompat.CreateToastNotifier().Show(toast);
#endif

                Dispose();
            }
        }

        [LogAspect]
        internal void StartTimer()
        {
            if (!IsRunning)
                _timer.Start();
        }

        [LogAspect]
        internal void StopTimer()
        {
            if (IsRunning)
                _timer.Stop();
        }

        [LogAspect]
        public void Dispose()
        {
            _timer.Tick -= timer_Tick;
            _timer.Tick -= toast_ElapsedEventHandler;
            // 破棄されたことをイベント通知
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
