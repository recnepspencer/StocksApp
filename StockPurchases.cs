namespace StocksApp
{
    public class StockPurchase
    {
        public string Symbol { get; private set; }
        public int NumberOfShares { get; private set; }
        public double PricePerShare { get; private set; }
        public double TotalCost => NumberOfShares * PricePerShare;

        // Constructor
        public StockPurchase(string symbol, int numberOfShares, double pricePerShare)
        {
            Symbol = symbol;
            NumberOfShares = numberOfShares;
            PricePerShare = pricePerShare;
        }
    }

    public class StockPurchases
    {
        // List to store purchases
        private List<StockPurchase> purchases = new List<StockPurchase>();

        // Method to buy stock
        public void BuyStock(string symbol, int numberOfShares, double pricePerShare)
        {
            var purchase = new StockPurchase(symbol, numberOfShares, pricePerShare);
            purchases.Add(purchase);
            Console.WriteLine($"Purchased {numberOfShares} shares of {symbol} at {pricePerShare:C} per share. Total cost: {purchase.TotalCost:C}");
        }

        // Method to display all purchases
        public void DisplayPurchases()
        {
            foreach (var purchase in purchases)
            {
                Console.WriteLine($"Symbol: {purchase.Symbol}, Shares: {purchase.NumberOfShares}, Price/Share: {purchase.PricePerShare:C}, Total Cost: {purchase.TotalCost:C}");
            }
        }
        public IReadOnlyList<StockPurchase> GetPurchases()
        {
            return purchases.AsReadOnly();
        }
        public void AddPurchase(StockPurchase purchase)
        {
            purchases.Add(purchase);
        }
    }
}