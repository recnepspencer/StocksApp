using System;
using System.Collections.Generic;
namespace StocksApp{
public class Program
{
    private static List<Stock> stocks = new List<Stock>();

    public static void Main()
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add Stock Data");
            Console.WriteLine("2. Show Stock Data");
            Console.WriteLine("3. Load Stock Data from Files");
            Console.WriteLine("4. Exit");

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
                    break;
                case "4":
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

            Console.WriteLine("New stock data added successfully.");
        }
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
        Stock foundStock = stocks.Find(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));

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



}

}