namespace StocksApp
{
    public class BottomingOutStrategy : InvestmentStrategy
    {
        private readonly double thresholdPercentage;

        public BottomingOutStrategy(double thresholdPercentage = 0.05)
        {
            this.thresholdPercentage = thresholdPercentage;
        }

        public override IEnumerable<StockPrediction> Evaluate(List<Stock> stocks)
        {
            List<StockPrediction> predictions = new List<StockPrediction>();

            foreach (var stock in stocks)
            {
                var recentHistory = stock.history.OrderByDescending(h => h.Date).Take(10).ToList();

                if (recentHistory.Count < 10)
                {
                    continue; // Skip if there's not enough data
                }

                double averagePriceLast10Days = recentHistory.Average(h => h.Price);

                // Use the most recent 52-week high and low values
                double recent52WeekHigh = recentHistory[0].Week52High;
                double recent52WeekLow = recentHistory[0].Week52Low;
                double range = recent52WeekHigh - recent52WeekLow;
                double threshold = recent52WeekLow + (range * thresholdPercentage);

                if (averagePriceLast10Days <= threshold)
                {
                    predictions.Add(new StockPrediction
                    {
                        Stock = stock,
                        PredictedPrice = averagePriceLast10Days
                    });
                }
            }

            return predictions;
        }
    }

}
