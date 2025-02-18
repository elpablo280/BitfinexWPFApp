using BitfinexConnector.Interface;
using System.Linq;

namespace BitfinexWPFApp
{
    public class PortfolioCalculator
    {
        private readonly IRestApi _restApiClient;

        public PortfolioCalculator(IRestApi restApiClient)
        {
            _restApiClient = restApiClient;
        }

        public async Task<Dictionary<string, decimal>> CalculateTotalBalanceAsync()
        {
            var balances = new Dictionary<string, decimal>
            {
                { "USD", 0m },  // на балансе 0 долларов
                { "BTC", 1m },
                { "XRP", 15000m },
                { "XMR", 50m },
                { "DSH", 30m }
            };
            var USDrates = new Dictionary<string, decimal>
            {
                { "USD", 1m }   // курс доллара к доллару 1:1
            };
            var totalBalances = new Dictionary<string, decimal>();

            foreach (var currency in balances.Keys)
            {
                var pairCurrencyToUSD = $"t{currency}USD";
                var candles = await _restApiClient.GetCandleSeriesAsync(pairCurrencyToUSD, "1m", DateTimeOffset.UtcNow.AddDays(-1));
                var lastCandle = candles.OrderByDescending(c => c.OpenTime).FirstOrDefault();
                if (lastCandle != null)
                {
                    var rate = lastCandle.ClosePrice;
                    USDrates.Add(currency, rate);   // заполняем словарь курсов к доллару
                }
            }

            foreach (var USDrate in USDrates)
            {
                decimal totalBalance = 0m;
                foreach (var balance in balances)
                {
                    totalBalance += balance.Value * USDrates[balance.Key] / USDrates[USDrate.Key];
                }
                totalBalances.Add(USDrate.Key, totalBalance);
            }

            return totalBalances;
        }
    }
}