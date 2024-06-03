using BreakTimeApp.Helpers;
using BreakTimeApp.Services;
using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using System.Windows.Threading;
using Windows.UI.Notifications;
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

        /**
         * <summary>
         * 破棄されたことをイベント通知
         * </summary>
         */
        public event EventHandler Disposed;

        public TimeStoreItem()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(INTERVAL);
            Timer.Tick += Timer_Tick;
            Timer.Tick += Toast_ElapsedEventHandler;
        }

        partial void OnNameChanged(string? oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                Name = oldValue;
                MessageBox.Show(Properties.MessageResources.InputError,
                    Properties.MessageResources.ErrorTitle,
                    System.Windows.MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

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

        partial void OnMaxProgressChanged(double value)
        {
            _ = UpdateItemInDatabase();
        }

        private async Task UpdateItemInDatabase()
        {
            ITimeStoreItemDataService dataService = new TimeStoreItemDataService(new TimeStoreItemDbContext());
            await dataService.UpdateTimeStoreItemAsync(TimeStoreItemToDbConverter.Convert(this));
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
        public void Toast_ElapsedEventHandler(object? sender, EventArgs e)
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
                    .AddText(Name)
                    .AddArgument(Properties.ToastResources.guid, ID.ToString())
                    .AddButton(new ToastButton()
                        .SetContent(Properties.ToastResources.acceptContent)
                        .AddArgument(Properties.ToastResources.action, Properties.ToastResources.accept)
                        .SetImageUri(new Uri(acceptImagePath)))
                    .AddButton(new ToastButton()
                        .SetContent(Properties.ToastResources.snoozeContent)
                        .AddArgument(Properties.ToastResources.action, Properties.ToastResources.snooze)
                        .SetImageUri(new Uri(snoozeImagePath)))
                    .AddButton(new ToastButton()
                        .SetContent(Properties.ToastResources.dismissContent)
                        .AddArgument(Properties.ToastResources.action, Properties.ToastResources.dismiss)
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
            Timer.Tick -= Toast_ElapsedEventHandler;
            // 破棄されたことをイベント通知
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
