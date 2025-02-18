using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BitfinexWPFApp.ViewModels
{
    public class PortfolioViewModel : INotifyPropertyChanged
    {
        private readonly PortfolioCalculator _portfolioCalculator;

        public ObservableCollection<PortfolioItem> PortfolioItems { get; set; }

        public PortfolioViewModel(PortfolioCalculator portfolioCalculator)
        {
            _portfolioCalculator = portfolioCalculator;
            PortfolioItems = new ObservableCollection<PortfolioItem>();
            LoadPortfolioCommand = new RelayCommand(async (a) => await LoadPortfolioAsync());
        }

        public RelayCommand LoadPortfolioCommand { get; }

        private async Task LoadPortfolioAsync()
        {
            var balances = await _portfolioCalculator.CalculateTotalBalanceAsync();
            PortfolioItems.Clear();
            foreach (var balance in balances)
            {
                PortfolioItems.Add(new PortfolioItem { Currency = balance.Key, TotalBalance = balance.Value });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PortfolioItem
    {
        public string Currency { get; set; }
        public decimal TotalBalance { get; set; }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
