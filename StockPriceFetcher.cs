using System;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace StocksApp
{
    public class StockPriceFetcher
    {
        public async Task<decimal> GetPrice(string symbol)
        {
            try
            {
                var url = $"https://finance.yahoo.com/quote/{symbol}";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                // XPath to target the fin-streamer element with the specific data attributes
                var xpath = "//fin-streamer[@data-symbol='" + symbol + "' and @data-test='qsp-price']";

                var priceNode = htmlDocument.DocumentNode.SelectSingleNode(xpath);

                if (priceNode != null && decimal.TryParse(priceNode.GetAttributeValue("value", ""), out decimal currentPrice))
                {
                    return currentPrice;
                }
                else if (priceNode != null && decimal.TryParse(priceNode.InnerText, out currentPrice))
                {
                    // Fallback to innerText if the value attribute is not present or not a valid decimal
                    return currentPrice;
                }
                else
                {
                    throw new Exception("Price could not be found or parsed.");
                }
            }
            catch (Exception ex)
            {
                // Handle different exceptions accordingly
                throw new Exception($"An error occurred while fetching the price: {ex.Message}");
            }
        }
    }
}