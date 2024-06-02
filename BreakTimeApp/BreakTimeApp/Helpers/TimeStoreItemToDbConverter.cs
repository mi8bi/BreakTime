using BreakTimeApp.Models;
using Wpf.Ui.Controls;

namespace BreakTimeApp.Helpers
{
    internal class TimeStoreItemToDbConverter
    {
        internal static TimeStoreItemDb Convert(TimeStoreItem item)
        {
            return new TimeStoreItemDb()
            {
                ID = item.ID.ToString(),
                Name = item.Name,
                Span = item.Span.ToString(),
                IsRunning = item.IsRunning,
                Progress = item.Progress,
                MaxProgress = item.MaxProgress,
                Icon = (int) item.Icon
            };
        }

        internal static TimeStoreItem ConvertBack(TimeStoreItemDb itemDb)
        {
            return new TimeStoreItem()
            {
                ID = Guid.Parse(itemDb.ID),
                Name = itemDb.Name,
                Span = TimeSpan.Parse(itemDb.Span),
                IsRunning = itemDb.IsRunning,
                Progress = itemDb.Progress,
                MaxProgress = itemDb.MaxProgress,
                Icon = (SymbolRegular) itemDb.Icon,
            };
        }
    }
}
