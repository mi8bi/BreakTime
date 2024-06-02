﻿using BreakTimeApp.Helpers;
using BreakTimeApp.Models;

namespace BreakTimeApp.ViewModels.Windows
{
    public partial class NotifyDetailsWindowViewModel : ObservableObject
    {

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
            Hours = Enumerable.Range(0, TimeStoreItem.MAX_HOUR).ToArray();
            Minutes = Enumerable.Range(0, TimeStoreItem.MAX_MINUTES).ToArray();
            Seconds = Enumerable.Range(0, TimeStoreItem.MAX_SECONDS).ToArray();
        }

        [LogAspect]
        [RelayCommand]
        private void OnChangeItem()
        {
            TimeSpan ts = new TimeSpan(0, SelectedHours, SelectedMinutes, SelectedSeconds);
            Item.Span = ts;
            Item.End = Item.Start + ts;

            if (Item.End < DateTime.Now)
            {
                Item.Start = DateTime.Now;
                Item.End = DateTime.Now + ts;
            }

            CloseRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
