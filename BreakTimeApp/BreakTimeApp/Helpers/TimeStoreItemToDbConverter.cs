using BreakTimeApp.Models;
using Wpf.Ui.Controls;

namespace BreakTimeApp.Helpers
{
    internal class TimeStoreItemToDbConverter
    {
        internal static TimeStoreItemDb Convert(TimeStoreItem item)
        {
            TimeStoreItemDb itemDb = App.GetService<TimeStoreItemDb>();
            itemDb.ID = item.ID.ToString();
            itemDb.Name = item.Name;
            itemDb.Span = item.Span.ToString();
            itemDb.IsRunning = item.IsRunning;
            itemDb.Progress = item.Progress;
            itemDb.MaxProgress = item.MaxProgress;
            itemDb.Icon = (int)item.Icon;
            return itemDb;
        }

        internal static TimeStoreItem ConvertBack(TimeStoreItemDb itemDb)
        {
            TimeStoreItem item = App.GetService<TimeStoreItem>();
            item.ID = Guid.Parse(itemDb.ID);
            item.Name = itemDb.Name;
            item.Span = TimeSpan.Parse(itemDb.Span);
            item.IsRunning = itemDb.IsRunning;
            item.Progress = itemDb.Progress;
            item.MaxProgress = itemDb.MaxProgress;
            item.Icon = (SymbolRegular)itemDb.Icon;
            return item;
        }
    }
}
