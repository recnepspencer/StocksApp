using System;
using System.Collections.Generic;
using System.Linq;

namespace StocksApp
{
    public class PERatioStrategy : InvestmentStrategy
    {
        public override IEnumerable<StockPrediction> Evaluate(List<Stock> stocks)
        {
            // Calculate the average P/E ratio over the last 10 days for each stock
            var averagePeRatios = stocks.Select(stock => new
            {
                Stock = stock,
                AveragePERatio = stock.history
                                       .OrderByDescending(h => h.Date)
                                       .Take(10)
                                       .Average(h => h.PERatio) // Ensure there are at least 10 entries
            });

            // Order the stocks by their average P/E ratio and take the top 3
            return averagePeRatios.OrderBy(stock => stock.AveragePERatio)
                                  .Take(3)
                                  .Select(stock => new StockPrediction
                                  {
                                      Stock = stock.Stock,
                                      PredictedPrice = stock.AveragePERatio // Use the average P/E ratio for the predicted price
                                  });
        }
    }
}
