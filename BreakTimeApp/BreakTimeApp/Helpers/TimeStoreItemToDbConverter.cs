using BreakTimeApp.Models;
using Wpf.Ui.Controls;

namespace BreakTimeApp.Helpers
{
    internal class TimeStoreItemToDbConverter
    {
        internal static TimeStoreItemDb Convert(TimeStoreItem item)
        {
            TimeStoreItemDb itemDb = new TimeStoreItemDb();
            itemDb.ID = item.ID.ToString();
            itemDb.Span = item.Span.ToString();
            itemDb.IsRunning = item.IsRunning;
            itemDb.Progress = item.Progress;
            itemDb.MaxProgress = item.MaxProgress;
            itemDb.Icon = (int) item.Icon;

            return itemDb;
        }
        internal static TimeStoreItem ConvertBack(TimeStoreItemDb itemDb)
        {
            return new TimeStoreItem()
            {
                ID = Guid.Parse(itemDb.ID),
                Span = TimeSpan.Parse(itemDb.Span),
                IsRunning = itemDb.IsRunning,
                Progress = itemDb.Progress,
                MaxProgress = itemDb.MaxProgress,
                Icon = (SymbolRegular) itemDb.Icon,
            };
        }
    }
}
