using BreakTimeApp.Helpers;
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

        /**
         * <summary>
         * 最大時間 (時)
         * </summary>
         */
        public static readonly int MAX_HOUR = 4;

        /**
         * <summary>
         * 最大時間 (分)
         * </summary>
         */
        public static readonly int MAX_MINUTES = 60;

        /**
         * <summary>
         * 最大時間 (秒)
         * </summary>
         */
        public static readonly int MAX_SECONDS = 60;

        /**
         * <summary>
         * プログレスバーの最大値
         * </summary>
         */
        private static readonly int MAX_PROGRESS = 100;

        private readonly DispatcherTimer _timer;

        public Guid ID { get; set; }

        public DateTime Start { get; set; }

        public TimeSpan Span { get; set; }

        [ObservableProperty]
        private DateTime _end;

        [ObservableProperty]
        private bool _isRunning;

        [ObservableProperty]
        private SymbolRegular _icon;

        [ObservableProperty]
        private double _progress;

        public TimeStoreItem()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(INTERVAL);
            _timer.Tick += timer_Tick;
            _timer.Tick += toast_ElapsedEventHandler;
        }

        [LogAspect]
        private void timer_Tick(object? sender, EventArgs e)
        {
            double progressUnit = MAX_PROGRESS / Span.TotalSeconds;
            Progress += progressUnit;
        }

        [LogAspect]
        private void toast_ElapsedEventHandler(object? sender, EventArgs e)
        {
            if (!IsRunning && Progress > MAX_PROGRESS)
            {
#if WINDOWS10_0_17763_0_OR_GREATER
                // Toastを組み立てる
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string acceptImagePath = Path.Combine(baseDirectory, "Assets/Images/ic_fluent_checkmark_24_filled.png");
                string snoozeImagePath = Path.Combine(baseDirectory, "Assets/Images/ic_fluent_snooze_24_filled.png");
                string dismissImagePath = Path.Combine(baseDirectory, "Assets/Images/ic_fluent_dismiss_24_filled.png");
                var toastContent = new ToastContentBuilder()
                    .AddText("New product in stock!")
                    .AddButton(new ToastButton()
                        .SetContent("Accept")
                        .AddArgument("action", "accept")
                        // TODO: AddArgumentにGuidを渡す
                        .SetImageUri(new Uri(acceptImagePath)))
                    .AddButton(new ToastButton()
                        .SetContent("Snooze")
                        .AddArgument("action", "snooze")
                        // TODO: AddArgumentにGuidを渡す
                        .SetImageUri(new Uri(snoozeImagePath)))
                    .AddButton(new ToastButton()
                        .SetContent("Dismiss")
                        .AddArgument("action", "dismiss")
                        // TODO: AddArgumentにGuidを渡す
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

        public void Dispose()
        {
            _timer.Tick -= timer_Tick;
            _timer.Tick -= toast_ElapsedEventHandler;
        }
    }
}
