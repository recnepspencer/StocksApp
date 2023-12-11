namespace StocksApp
{
    public abstract class InvestmentStrategy
    {
        public abstract IEnumerable<StockPrediction> Evaluate(List<Stock> stocks);
    }
    public class StockPrediction
    {
        public Stock Stock { get; set; }
        public double PredictedPrice { get; set; }

        public override string ToString()
        {
            return $"Stock: {Stock.Symbol}, Predicted Price: {PredictedPrice}";
        }
    }
}