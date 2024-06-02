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
            itemDb.Start = item.Start.ToString();
            itemDb.Span = item.Span.ToString();
            itemDb.End = item.End.ToString();
            itemDb.IsRunning = item.IsRunning;
            itemDb.Progress = item.Progress;
            itemDb.Icon = (int) item.Icon;

            return itemDb;
        }
        internal static TimeStoreItem ConvertBack(TimeStoreItemDb itemDb)
        {
            return new TimeStoreItem()
            {
                ID = Guid.Parse(itemDb.ID),
                Start = DateTime.Parse(itemDb.Start),
                End = DateTime.Parse(itemDb.End),
                Span = TimeSpan.Parse(itemDb.Span),
                IsRunning = itemDb.IsRunning,
                Icon = (SymbolRegular) itemDb.Icon,
            };
        }
    }
}
