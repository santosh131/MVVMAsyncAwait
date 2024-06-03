using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MVVMAsyncAwait.Concrete;
using MVVMAsyncAwait.Interfaces;
using MVVMAsyncAwait.ViewModels;
using System.Windows;

namespace MVVMAsyncAwait
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ServiceCollection sc = new ServiceCollection();
            ServiceProvider serviceProvider = sc
                .AddTransient<IEmployeeService, EmployeeService>()
                .AddTransient<EmployeeViewModel>()
                .BuildServiceProvider();
            Ioc.Default.ConfigureServices(serviceProvider);
        }
    }
}
