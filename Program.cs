using System;
using System.Collections.Generic;
namespace StocksApp
{
    public class Program
    {
        private static List<Stock> stocks = new List<Stock>();
        private static StockPurchases stockPurchases = new StockPurchases(); // To track user's purchases
        private static StockPriceFetcher priceFetcher = new StockPriceFetcher(); // To fetch current stock prices

        public static async Task Main()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Add Stock Data");
                Console.WriteLine("2. Show Stock Data");
                Console.WriteLine("3. Load Stock/ Purchase Data from Files");
                Console.WriteLine("4. Select an Investment Strategy");
                Console.WriteLine("5. Purchase Stocks");
                Console.WriteLine("6. Show Returns");
                Console.WriteLine("7. Save Purchases");
                Console.WriteLine("8. Exit");

                string option = Console.ReadLine()!;

                switch (option)
                {
                    case "1":
                        AddStockData();
                        break;
                    case "2":
                        ShowStockData();
                        break;
                    case "3":
                        LoadStocks();
                        LoadPurchases();
                        break;
                    case "4":
                        SelectInvestmentStrategy();
                        break;
                    case "5":
                        Console.WriteLine("Do you want to buy any stocks? (yes/no)");
                        string response = Console.ReadLine()!;
                        if (response.Equals("yes", StringComparison.OrdinalIgnoreCase))
                        {
                            BuyStocksProcess();
                        }
                        break;
                    case "6":
                        await ShowReturns();
                        break;
                    case "7":
                        SaveStockPurchases();
                        break;
                    case "8":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        private static void AddStockData()
        {
            FileHandler fileHandler = new FileHandler();
            Console.WriteLine("Enter Stock Symbol:");
            string symbol = Console.ReadLine()!;

            // Check if the stock already exists
            Stock existingStock = stocks.Find(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase))!;

            if (existingStock != null)
            {
                // If stock exists, update its history
                Console.WriteLine("Enter Current Price:");
                double price = double.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter PE Ratio:");
                double peRatio = double.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter 52-Week Low:");
                double week52Low = double.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter 52-Week High:");
                double week52High = double.Parse(Console.ReadLine()!);

                existingStock.AddToHistory(price, peRatio, week52Low, week52High);
                fileHandler.SaveOrUpdateStockData(existingStock);
                Console.WriteLine("Stock history updated successfully.");
                Console.WriteLine("Stock history updated successfully.");
            }
            else
            {
                // If stock doesn't exist, create a new stock
                Console.WriteLine("Enter Stock Name:");
                string name = Console.ReadLine()!;

                Console.WriteLine("Enter Current Price:");
                double price = double.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter PE Ratio:");
                double peRatio = double.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter 52-Week High:");
                double week52High = double.Parse(Console.ReadLine()!);

                Console.WriteLine("Enter 52-Week Low:");
                double week52Low = double.Parse(Console.ReadLine()!);

                Stock newStock = new Stock(name, symbol, price, peRatio, week52High, week52Low);
                stocks.Add(newStock);

                fileHandler.SaveOrUpdateStockData(newStock);
                Console.WriteLine("New stock data added successfully.");
            }
        }
        private static void LoadPurchases()
        {
            FileHandler fileHandler = new FileHandler();
            List<StockPurchase> loadedPurchases = fileHandler.LoadPurchases();

            foreach (var purchase in loadedPurchases)
            {
                stockPurchases.AddPurchase(purchase);
            }

            Console.WriteLine($"Loaded {loadedPurchases.Count} purchases.");
        }
        private static void SaveStockPurchases()
        {
            FileHandler fileHandler = new FileHandler();
            fileHandler.SaveStockPurchases(stockPurchases.GetPurchases());
            Console.WriteLine("Stock purchases saved successfully.");
        }
        private static void ShowStockData()
        {
            // List all available stock symbols
            Console.WriteLine("Available Stock Symbols:");
            foreach (var stock in stocks)
            {
                Console.WriteLine(stock.Symbol);
            }

            Console.WriteLine("Enter Stock Symbol:");
            string symbol = Console.ReadLine()!;

            // Find the stock
            Stock foundStock = stocks.Find(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase))!;

            if (foundStock != null)
            {
                // Display the stock data
                Console.WriteLine($"Displaying data for {foundStock.Symbol}:");
                foundStock.DisplayStockInfo(); // Assuming this method displays the current stock data
                foundStock.DisplayStockHistory(); // Assuming this method displays the historical stock data
            }
            else
            {
                Console.WriteLine("Stock not found.");
            }
        }

        private static void LoadStocks()
        {
            try
            {
                Console.WriteLine("Enter the path to the folder containing the stock data files:");
                string folderPath = Console.ReadLine()!;

                FileHandler fileHandler = new FileHandler();
                var loadedStocks = fileHandler.LoadStockDataFromFolder(folderPath);

                if (loadedStocks.Count == 0)
                {
                    Console.WriteLine("No stock data found in the files.");
                    return;
                }

                foreach (var loadedStock in loadedStocks)
                {
                    var existingStock = stocks.Find(s => s.Symbol.Equals(loadedStock.Symbol, StringComparison.OrdinalIgnoreCase));
                    if (existingStock != null)
                    {
                        Console.WriteLine($"Updating existing stock: {loadedStock.Symbol}");
                        foreach (var historyItem in loadedStock.history)
                        {
                            existingStock.AddToHistory(historyItem.Price, historyItem.PERatio, historyItem.Week52Low, historyItem.Week52High);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Adding new stock: {loadedStock.Symbol}");
                        stocks.Add(loadedStock);
                    }
                }

                Console.WriteLine("Stocks loaded successfully from files.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The specified directory was not found. Please check the path and try again.");
            }
            catch (IOException e)
            {
                Console.WriteLine($"An I/O error occurred: {e.Message}");
            }
        }
        private static void SelectInvestmentStrategy()
        {
            Console.WriteLine("Select an Investment Strategy:");
            Console.WriteLine("1. Regression Strategy");
            Console.WriteLine("2. PE Ratio Strategy");
            Console.WriteLine("3. Bottoming Out Strategy");

            string strategyOption = Console.ReadLine()!;

            switch (strategyOption)
            {
                case "1":
                    ApplyRegressionStrategy();
                    break;
                case "2":
                    ApplyPERatioStrategy();
                    break;
                case "3":
                    ApplyBottomingOutStrategy();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
        private static void ApplyRegressionStrategy()
        {
            var regressionStrategy = new RegressionStrategy();
            var stockPredictions = regressionStrategy.Evaluate(stocks);

            foreach (var prediction in stockPredictions)
            {
                Console.WriteLine($"Stock: {prediction.Stock.Symbol}, Predicted Price in 5 days: {prediction.PredictedPrice}");
            }
        }

        private static void ApplyPERatioStrategy()
        {
            var peRatioStrategy = new PERatioStrategy();
            var pePredictions = peRatioStrategy.Evaluate(stocks);

            foreach (var prediction in pePredictions)
            {
                Console.WriteLine($"Stock: {prediction.Stock.Symbol}, Average P/E Ratio (last 10 days): {prediction.PredictedPrice}");
            }
        }

        private static void ApplyBottomingOutStrategy()
        {
            var bottomingOutStrategy = new BottomingOutStrategy(0.25);
            var bottomingOutPredictions = bottomingOutStrategy.Evaluate(stocks).ToList();

            if (bottomingOutPredictions.Any())
            {
                foreach (var prediction in bottomingOutPredictions)
                {
                    Console.WriteLine($"Recommended to Buy: {prediction.Stock.Symbol}, " +
                                      $"Average Price (last 10 days): {prediction.PredictedPrice.ToString("F2")}");
                }
            }
            else
            {
                Console.WriteLine("No stocks meet the criteria for the Bottoming Out Strategy.");
            }
        }
        private static void BuyStocksProcess()
        {
            Console.WriteLine("What symbol?");
            string symbol = Console.ReadLine()!;

            Console.WriteLine("How many shares?");
            if (!int.TryParse(Console.ReadLine(), out int numberOfShares))
            {
                Console.WriteLine("Invalid number of shares.");
                return;
            }

            // Retrieve the most recent price for the specified stock
            var stock = stocks.FirstOrDefault(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
            if (stock == null || !stock.history.Any())
            {
                Console.WriteLine("Stock not found or no price data available.");
                return;
            }

            double pricePerShare = stock.history.OrderByDescending(h => h.Date).First().Price;

            // Creating an instance of StockPurchases to handle the purchase
            stockPurchases.BuyStock(symbol!, numberOfShares, pricePerShare);

            // Optionally display all purchases
            stockPurchases.DisplayPurchases();
        }

        private static async Task ShowReturns()
        {
            foreach (var purchase in stockPurchases.GetPurchases())
            {
                Console.WriteLine(purchase);
                try
                {
                    decimal currentPrice = await priceFetcher.GetPrice(purchase.Symbol);
                    decimal purchasePrice = (decimal)purchase.PricePerShare;
                    int numberOfShares = purchase.NumberOfShares;
                    decimal totalReturn = (currentPrice - purchasePrice) * numberOfShares;

                    Console.WriteLine($"Current price for {purchase.Symbol} is {currentPrice:C}");
                    Console.WriteLine($"You bought {numberOfShares} shares at {purchasePrice:C} per share.");
                    Console.WriteLine($"Your current return for {purchase.Symbol} is {totalReturn:C}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not fetch current price for {purchase.Symbol}. Error: {ex.Message}");
                }
            }
        }
    }
}