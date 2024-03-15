using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using PreciosoApp.ViewModels;
using PreciosoApp.Views;

namespace PreciosoApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                /*
                desktop.MainWindow = new LoginWindowView
                {
                    DataContext = new LoginWindowViewModel()
                };
                */

                var loginWindowView = new LoginWindowView();
                var loginWindowViewModel = new LoginWindowViewModel(loginWindowView); // Pass the instance
                loginWindowView.DataContext = loginWindowViewModel;
                desktop.MainWindow = loginWindowView;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}