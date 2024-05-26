using BreakTimeApp.Models;
using BreakTimeApp.Views.Windows;

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

}
