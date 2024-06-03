using CommunityToolkit.Mvvm.DependencyInjection;
using MVVMAsyncAwait.ViewModels;
using System.Windows;

namespace MVVMAsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<EmployeeViewModel>();
        }
    }
}
