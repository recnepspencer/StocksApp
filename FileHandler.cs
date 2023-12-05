using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
namespace StocksApp
{
    public class FileHandler
    {
        public List<Stock> LoadStockDataFromFolder(string folderPath)
        {
            List<Stock> stocks = new List<Stock>();
            string[] fileEntries = Directory.GetFiles(folderPath, "*.csv");

            foreach (string fileName in fileEntries)
            {
                // Extract stock name and symbol from the file name
                string fileBaseName = Path.GetFileNameWithoutExtension(fileName);
                var match = Regex.Match(fileBaseName, @"(.+)-(.+)");
                if (!match.Success) continue;

                string stockName = match.Groups[1].Value;
                string stockSymbol = match.Groups[2].Value;

                // Read and parse the CSV file
                var stock = ParseCsvFile(fileName, stockName, stockSymbol);
                if (stock != null)
                {
                    stocks.Add(stock);
                }
            }

            return stocks;
        }

        private Stock ParseCsvFile(string filePath, string stockName, string stockSymbol)
        {
            List<Stock.StockHistory> history = new List<Stock.StockHistory>();
            double lastPrice = 0;
            double lastPERatio = 0;
            double week52High = 0;
            double week52Low = 0;

            bool firstLine = true;

            foreach (string line in File.ReadLines(filePath))
            {
                if (firstLine) // Skip the header line
                {
                    firstLine = false;
                    continue;
                }

                string[] parts = line.Split(','); // Adjusted for comma-separated values
                if (parts.Length != 5) continue; // Skip invalid lines

                DateTime date = DateTime.Parse(parts[0]);
                double price = double.Parse(parts[1]);
                double peRatio = double.Parse(parts[2]);
                double lineWeek52Low = double.Parse(parts[3]);
                double lineWeek52High = double.Parse(parts[4]);

                history.Add(new Stock.StockHistory(date, price, peRatio, lineWeek52Low, lineWeek52High));
            }

            if (history.Count > 0)
            {
                // Create a stock with the most recent price and PE ratio, and historical 52-week high/low
                var stock = new Stock(stockName, stockSymbol, lastPrice, lastPERatio, week52High, week52Low);
                stock.history = history; // Directly assign the history list
                return stock;
            }

            return null;
        }
    }
}