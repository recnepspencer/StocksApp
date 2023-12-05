namespace StocksApp
{
    public abstract class InvestmentStrategy
    {
        public abstract double PredictFuturePrice(Stock stock);
    }
}