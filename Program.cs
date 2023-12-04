using System;
using System.Collections.Generic;

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
            Console.WriteLine("3. Exit");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    AddStockData();
                    break;
                case "2":
                    ShowStockData();
                    break;
                case "3":
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
        string symbol = Console.ReadLine();

        // Check if the stock already exists
        Stock existingStock = stocks.Find(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));

        if (existingStock != null)
        {
            // If stock exists, update its history
            Console.WriteLine("Enter Current Price:");
            double price = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter PE Ratio:");
            double peRatio = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter 52-Week Low:");
            double week52Low = double.Parse(Console.ReadLine());
            
            Console.WriteLine("Enter 52-Week High:");
            double week52High = double.Parse(Console.ReadLine());

            existingStock.AddToHistory(price, peRatio, week52Low, week52High);

            Console.WriteLine("Stock history updated successfully.");
        }
        else
        {
            // If stock doesn't exist, create a new stock
            Console.WriteLine("Enter Stock Name:");
            string name = Console.ReadLine();

            Console.WriteLine("Enter Current Price:");
            double price = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter PE Ratio:");
            double peRatio = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter 52-Week High:");
            double week52High = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter 52-Week Low:");
            double week52Low = double.Parse(Console.ReadLine());

            Stock newStock = new Stock(name, symbol, price, peRatio, week52High, week52Low);
            stocks.Add(newStock);

            Console.WriteLine("New stock data added successfully.");
        }
    }

    private static void ShowStockData()
    {
        Console.WriteLine("Enter Stock Symbol:");
        string symbol = Console.ReadLine();

        Stock foundStock = stocks.Find(s => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));

        if (foundStock != null)
        {
            Console.WriteLine("Choose the data to display:");
            Console.WriteLine("1. Today's Stock Data");
            Console.WriteLine("2. Historical Stock Data");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Display today's stock data
                    foundStock.DisplayStockInfo();
                    break;
                case "2":
                    // Display historical stock data
                    foundStock.DisplayStockHistory();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Stock not found.");
        }
    }

}
