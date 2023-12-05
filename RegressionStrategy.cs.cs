using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StocksApp
{
    public class RegressionStrategy : InvestmentStrategy
    {
        public override double PredictFuturePrice(Stock stock)
        {
            var recentHistory = stock.history
                                    .OrderByDescending(h => h.Date)
                                    .Take(10)
                                    .ToList();

            if (recentHistory.Count < 10)
            {
                throw new InvalidOperationException("Not enough data to perform regression.");
            }

            double[] prices = recentHistory.Select(h => h.Price).ToArray();
            double[] days = recentHistory.Select((h, index) => (double)index).ToArray();

            // Perform linear regression
            var (slope, intercept) = Fit.Line(days, prices);

            // Predict price for 5 days into the future
            double predictionDay = days[0] + 5;
            double predictedPrice = slope * predictionDay + intercept;

            return predictedPrice;
        }
    }
}
