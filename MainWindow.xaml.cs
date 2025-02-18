using BitfinexConnector.Clients;
using BitfinexWPFApp.ViewModels;
using System.Windows;

namespace BitfinexWPFApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var restApiClient = new RestApiClient();
        var portfolioCalculator = new PortfolioCalculator(restApiClient);
        DataContext = new PortfolioViewModel(portfolioCalculator);
    }
}