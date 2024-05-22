using BreakTimeApp.Models;
using System.Collections.ObjectModel;

namespace BreakTimeApp.ViewModels.Pages
{
    public partial class NotifyViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<TimeStoreItem> _items = new ObservableCollection<TimeStoreItem>();

        [ObservableProperty]
        private TimeStoreItem _selectedItem;

        public NotifyViewModel()
        {
            //「1日と2時間45分15秒」を表すTimeSpanオブジェクトを作成する
            TimeSpan ts1 = new TimeSpan(1, 2, 45, 15);
            // 初期アイテムを追加
            Items.Add(new TimeStoreItem { Start = DateTime.Now, End = DateTime.Now, Span = ts1 });
            Items.Add(new TimeStoreItem { Start = DateTime.Now, End = DateTime.Now + ts1, Span = ts1 });
            Items.Add(new TimeStoreItem { Start = DateTime.Now, End = DateTime.Now + ts1, Span = ts1 });
        }

        [RelayCommand]
        private void Add()
        {
            //「1日と2時間45分15秒」を表すTimeSpanオブジェクトを作成する
            TimeSpan ts1 = new TimeSpan(1, 2, 45, 15);
            // 新しいアイテムを追加
            Items.Add(new TimeStoreItem { Start = DateTime.Now, End = DateTime.Now + ts1, Span = ts1 });
        }

        [RelayCommand]
        private void RemoveItem(TimeStoreItem item)
        {
            if (item != null && Items.Contains(item))
                Items.Remove(item);
        }
    }
}
