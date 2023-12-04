using System;
using System.Collections.Generic;

public class Stock
{
    // Properties
    public string Name { get; private set; }
    public string Symbol { get; private set; }
    public double Price { get; private set; }
    public double PERatio { get; private set; }
    public double Week52High { get; private set; }
    public double Week52Low { get; private set; }

    // Nested class for Stock History
    private class StockHistory
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public double PERatio { get; set; }
        public double Week52Low { get; set; }
        public double Week52High { get; set; }

        public StockHistory(DateTime date, double price, double peRatio, double week52Low, double week52High)
        {
            Date = date;
            Price = price;
            PERatio = peRatio;
            Week52Low = week52Low;
            Week52High = week52High;
        }
    }

    // Collection to hold StockHistory
    private List<StockHistory> history;

    // Constructor
    public Stock(string name, string symbol, double price, double peRatio, double week52High, double week52Low)
    {
        Name = name;
        Symbol = symbol;
        Price = price;
        PERatio = peRatio;
        Week52Low = week52Low;
        Week52High = week52High;
        history = new List<StockHistory>();
    }

    // Method to add stock history
    public void AddToHistory(double price, double peRatio, double week52Low, double week52High)
    {
        var stockHistory = new StockHistory(DateTime.Now, price, peRatio, week52Low, week52High);
        history.Add(stockHistory);
    }

    // Method to display stock information
    public void DisplayStockInfo()
    {
        Console.WriteLine($"Stock Name: {Name}");
        Console.WriteLine($"Symbol: {Symbol}");
        Console.WriteLine($"Price: {Price}");
        Console.WriteLine($"PE Ratio: {PERatio}");
        Console.WriteLine($"52-Week High: {Week52High}");
        Console.WriteLine($"52-Week Low: {Week52Low}");
    }
        public void DisplayStockHistory()
    {
        Console.WriteLine($"Historical Data for {Symbol}:");
        foreach (var historyItem in history)
        {
            Console.WriteLine($"Date: {historyItem.Date}, Price: {historyItem.Price}, PE Ratio: {historyItem.PERatio}, 52-Week Low: {historyItem.Week52Low}, 52-Week High: {historyItem.Week52High}");
        }
    }
}
