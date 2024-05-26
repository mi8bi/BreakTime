using Microsoft.Extensions.DependencyInjection;

namespace BreakTimeApp.Services;

public class WindowsProviderService
{
    private readonly IServiceProvider _serviceProvider;

    public WindowsProviderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T GetWindow<T>()
        where T : class
    {
        if (!typeof(T).IsSubclassOf(typeof(Window)))
        {
            throw new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");
        }

        T windowInstance =
            _serviceProvider.GetService(typeof(T)) as T
            ?? throw new InvalidOperationException("Window is not registered as service.");

        return windowInstance;
    }

    public void ShowDialog<T>()
        where T : class
    {
        if (!typeof(Window).IsAssignableFrom(typeof(T)))
        {
            throw new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");
        }

        Window windowInstance =
            _serviceProvider.GetService<T>() as Window
            ?? throw new InvalidOperationException("Window is not registered as service.");
        windowInstance.Owner = Application.Current.MainWindow;
        windowInstance.ShowDialog();
    }

    public void Show<T>()
        where T : class
    {
        if (!typeof(Window).IsAssignableFrom(typeof(T)))
        {
            throw new InvalidOperationException($"The window class should be derived from {typeof(Window)}.");
        }

        Window windowInstance =
            _serviceProvider.GetService<T>() as Window
            ?? throw new InvalidOperationException("Window is not registered as service.");
        windowInstance.Owner = Application.Current.MainWindow;
        windowInstance.Show();
    }

}
