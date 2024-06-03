using BreakTimeApp.Helpers;
using BreakTimeApp.Models;
using BreakTimeApp.Services;

namespace BreakTimeApp.ViewModels.Windows
{
    public partial class NotifyDetailsWindowViewModel : ObservableObject
    {

        /**
         * <summary>
         * 最大時間 (時)
         * </summary>
         */
        private static readonly int MAX_HOUR = 4;

        /**
         * <summary>
         * 最大時間 (分)
         * </summary>
         */
        private static readonly int MAX_MINUTES = 60;

        /**
         * <summary>
         * 最大時間 (秒)
         * </summary>
         */
        private static readonly int MAX_SECONDS = 60;

        public TimeStoreItem Item { get; set; }

        public event EventHandler CloseRequested;

        private readonly IServiceProvider _serviceProvider;

        public int[] Hours { get; set; }

        public int[] Minutes { get; set; }

        public int[] Seconds { get; set; }

        public int SelectedHours { get; set; }

        public int SelectedMinutes { get; set; }

        public int SelectedSeconds { get; set; }

        public NotifyDetailsWindowViewModel()
        {
            Hours = Enumerable.Range(0, MAX_HOUR).ToArray();
            Minutes = Enumerable.Range(0, MAX_MINUTES).ToArray();
            Seconds = Enumerable.Range(0, MAX_SECONDS).ToArray();
        }

        [LogAspect]
        [RelayCommand]
        private void OnChangeItem()
        {
            TimeSpan ts = new TimeSpan(0, SelectedHours, SelectedMinutes, SelectedSeconds);
            Item.Span = ts;
            Item.Progress = 0;
            Item.MaxProgress = ts.TotalSeconds;
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
