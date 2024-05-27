using Wpf.Ui.Controls;

namespace BreakTimeApp.Models
{
    public partial class TimeStoreItem : ObservableObject
    {
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

        public Guid Guid { get; set; }

        public DateTime Start { get; set; }

        [ObservableProperty]
        private DateTime _end;
        public TimeSpan Span { get; set; }

        [ObservableProperty]
        private bool _active;

        [ObservableProperty]
        private SymbolRegular _icon;

    }
}
