using BreakTimeApp.Helpers;
using BreakTimeApp.ViewModels.Windows;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace BreakTimeApp.Views.Windows
{
    public partial class MainWindow : INavigationWindow
    {
        public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel,
            IPageService pageService,
            INavigationService navigationService
        )
        {
            ViewModel = viewModel;
            DataContext = this;

            SystemThemeWatcher.Watch(this);

            InitializeComponent();
            SetPageService(pageService);

            navigationService.SetNavigationControl(RootNavigation);
            // ウィンドウを閉じる処理
            Closing += MainWindow_Closing;
            initializeTheme();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // アプリケーションの終了をキャンセル
            e.Cancel = true;

            // ウィンドウを非表示にする
            Hide();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }

            base.OnStateChanged(e);
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        [LogAspect]
        protected override void OnClosed(EventArgs e)
        {
            // タスクトレイでアプリを終了するため、ここでは何もしない
        }

        [LogAspect]
        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        [LogAspect]
        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // アプリケーション終了
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void initializeTheme()
        {
            switch (Properties.Settings.Default.Theme)
            {
                case "theme_light":
                    ApplicationThemeManager.Apply(ApplicationTheme.Light);
                    break;
                default:
                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    break;
            }
        }

    }
}
