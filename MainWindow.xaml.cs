using BitfinexConnector.Clients;
using BitfinexWPFApp.ViewModels;
using System.Windows;

namespace BitfinexWPFApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var restApiClient = new RestApiClient();
        var webSocketClient = new WebSocketClient();
        var portfolioCalculator = new PortfolioCalculator(restApiClient);
        DataContext = new PortfolioViewModel(portfolioCalculator);
    }
}