using Microsoft.Extensions.Logging;

namespace BreakTimeApp.Helpers
{
    internal static class AppLogEvents
    {
        // アプリケーションの起動と終了
        internal static readonly EventId AppStart = new(1000, "AppStart");
        internal static readonly EventId AppExit = new(1001, "AppExit");

        // ユーザー操作
        internal static readonly EventId UserLogin = new(2000, "UserLogin");
        internal static readonly EventId UserLogout = new(2001, "UserLogout");
        internal static readonly EventId UserAction = new(2002, "UserAction");

        // データの読み書き
        internal static readonly EventId DataRead = new(3000, "DataRead");
        internal static readonly EventId DataWrite = new(3001, "DataWrite");
        internal static readonly EventId DataDelete = new(3002, "DataDelete");

        // エラーおよび例外
        internal static readonly EventId Error = new(4000, "Error");
        internal static readonly EventId Exception = new(4001, "Exception");

        // 設定の変更
        internal static readonly EventId SettingsChange = new(5000, "SettingsChange");

        // ネットワーク操作
        internal static readonly EventId NetworkRequest = new(6000, "NetworkRequest");
        internal static readonly EventId NetworkResponse = new(6001, "NetworkResponse");
        internal static readonly EventId NetworkError = new(6002, "NetworkError");

        // ウィンドウ操作
        internal static readonly EventId WindowOpen = new(7000, "WindowOpen");
        internal static readonly EventId WindowClose = new(7001, "WindowClose");
    }
}
