using BreakTimeApp.Helpers;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace BreakTimeApp.Models
{
    public partial class TimeStoreItem : ObservableObject
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

        private readonly DispatcherTimer _timer;

        public Guid Guid { get; set; }

        public DateTime Start { get; set; }

        [ObservableProperty]
        private DateTime _end;
        public TimeSpan Span { get; set; }

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
            double progressUnit = 100 / Span.TotalSeconds;
            Progress += progressUnit;
        }

        [LogAspect]
        private void toast_ElapsedEventHandler(object? sender, EventArgs e)
        {
            if (!IsRunning)
            {
                // TODO toast message
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

    }
}
