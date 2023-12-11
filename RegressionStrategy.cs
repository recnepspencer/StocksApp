using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StocksApp
{
    public class RegressionStrategy : InvestmentStrategy
    {
        public override IEnumerable<StockPrediction> Evaluate(List<Stock> stocks)
        {
            List<StockPrediction> predictions = new List<StockPrediction>();

            foreach (var stock in stocks)
            {
                var recentHistory = stock.history.Take(10).ToList();

                if (recentHistory.Count < 10)
                {
                    Console.WriteLine($"Insufficient data for stock {stock.Symbol}.");
                    continue; // Not enough data, skip this stock
                }

                double[] prices = recentHistory.Select(h => h.Price).ToArray();
                double[] days = Enumerable.Range(1, recentHistory.Count).Select(x => (double)x).ToArray(); // Start from day 1 to 10

                double rSquared, yIntercept, slope;
                LinearRegression(days, prices, 0, recentHistory.Count, out rSquared, out yIntercept, out slope);

                double predictedPrice = yIntercept + (slope * 15); // Predicting for day 15

                predictions.Add(new StockPrediction
                {
                    Stock = stock,
                    PredictedPrice = predictedPrice
                });
            }

            return predictions;
        }
        // Method to calculate linear regression
        public static void LinearRegression(double[] xVals, double[] yVals,
                                            int inclusiveStart, int exclusiveEnd,
                                            out double rsquared, out double yintercept,
                                            out double slope)
        {
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double ssX = 0;
            double ssY = 0;
            double sumCodeviates = 0;
            double sCo = 0;
            double count = exclusiveEnd - inclusiveStart;

            for (int ctr = inclusiveStart; ctr < exclusiveEnd; ctr++)
            {
                double x = xVals[ctr];
                double y = yVals[ctr];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }
            ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            double RNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            double RDenom = (count * sumOfXSq - (sumOfX * sumOfX))
             * (count * sumOfYSq - (sumOfY * sumOfY));
            sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            double meanX = sumOfX / count;
            double meanY = sumOfY / count;
            double dblR = RNumerator / Math.Sqrt(RDenom);
            rsquared = dblR * dblR;
            yintercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
        }
    }
}