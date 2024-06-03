using BreakTimeApp.Helpers;
using BreakTimeApp.Models;
using BreakTimeApp.Services;
using BreakTimeApp.ViewModels.Pages;
using BreakTimeApp.ViewModels.Windows;
using BreakTimeApp.Views.Pages;
using BreakTimeApp.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Wpf.Ui;

namespace BreakTimeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration((hostingContext, config) => {
                var environment = hostingContext.HostingEnvironment.EnvironmentName;
                config.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location));
                config
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<WindowsProviderService>();
                // 表示するウィンドウの登録
                services.AddTransient<NotifyDetailsWindow>();
                services.AddSingleton<NotifyDetailsWindowViewModel>();

                // Pages
                services.AddSingleton<NotifyPage>();
                services.AddSingleton<NotifyViewModel>();
                services.AddSingleton<DataPage>();
                services.AddSingleton<DataViewModel>();
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();

                // DB
                services.AddTransient<TimeStoreItem>();
                services.AddTransient<TimeStoreItemDb>();
            })
            .ConfigureLogging(logging =>
            {
                // ロギングの設定
                // 既定のロガーを削除
                logging.ClearProviders();
                // 「デバッグ」にログ出力されるように設定
                logging.AddDebug();

                // NLogの追加
                logging.SetMinimumLevel(LogLevel.Trace);
                logging.AddNLog();
            })
            .Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        [LogAspect]
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        [LogAspect]
        private void OnStartup(object sender, StartupEventArgs e)
        {
            _host.Start();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        [LogAspect]
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        [LogAspect]
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
