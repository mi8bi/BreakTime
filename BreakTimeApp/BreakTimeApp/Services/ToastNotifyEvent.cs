
using Microsoft.Toolkit.Uwp.Notifications;
using System.IO;
using Windows.UI.Notifications;

namespace BreakTimeApp.Services
{
    public class ToastNotifyEvent : INotifyEvent
    {
        private readonly string _id;
        private readonly string _text;

        /**
         * <summary>
         * <paramref name="id"/>通知アイテムのID(uuid)
         * <paramref name="text"/>通知内容のテキスト
         * </summary>
         **/
        public ToastNotifyEvent(string id, string text)
        {
            _id = id;
            _text = text;
        }

        public void Notify()
        {
#if WINDOWS10_0_17763_0_OR_GREATER
                // Toastを組み立てる
                string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Images");
                string acceptImagePath = Path.Combine(baseDirectory, "ic_fluent_checkmark_24_filled.png");
                string snoozeImagePath = Path.Combine(baseDirectory, "ic_fluent_snooze_24_filled.png");
                string dismissImagePath = Path.Combine(baseDirectory, "ic_fluent_dismiss_24_filled.png");
                var toastContent = new ToastContentBuilder()
                    .AddText(_text)
                    .AddArgument(Properties.ToastResources.guid, _id)
                    .AddButton(new ToastButton()
                        .SetContent(Properties.ToastResources.acceptContent)
                        .AddArgument(Properties.ToastResources.action, Properties.ToastResources.accept)
                        .SetImageUri(new Uri(acceptImagePath)))
                    .AddButton(new ToastButton()
                        .SetContent(Properties.ToastResources.snoozeContent)
                        .AddArgument(Properties.ToastResources.action, Properties.ToastResources.snooze)
                        .SetImageUri(new Uri(snoozeImagePath)))
                    .AddButton(new ToastButton()
                        .SetContent(Properties.ToastResources.dismissContent)
                        .AddArgument(Properties.ToastResources.action, Properties.ToastResources.dismiss)
                        .SetImageUri(new Uri(dismissImagePath)))
                    .GetToastContent();
                // Toastのレイアウトを作成
                var toast = new ToastNotification(toastContent.GetXml());
                // Toast表示
                ToastNotificationManagerCompat.CreateToastNotifier().Show(toast);
#endif
        }
    }
}
