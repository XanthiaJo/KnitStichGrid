using System.Windows;
using KnitStichGrid.Services;
using KnitStichGrid.ViewModels;

namespace KnitStichGrid.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel(new FinishedSizeCalculator());
    }
}
