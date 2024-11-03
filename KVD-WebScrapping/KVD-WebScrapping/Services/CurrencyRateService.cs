using HtmlAgilityPack;

public class CurrencyRateService
{
    private readonly HttpClient _httpClient;

    public CurrencyRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<CurrencyRate>> GetCurrencyRates()
    {
        var url = "https://www.hsbc.com.my/investments/products/foreign-exchange/currency-rate/";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var htmlContent = await response.Content.ReadAsStringAsync();
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            var table = htmlDocument.DocumentNode.SelectSingleNode("//table[@class='desktop']");

            if (table != null)
            {
                var rows = table.SelectNodes(".//tr");

                if (rows != null)
                {
                    return rows.Skip(1)
                        .Select(row =>
                        {
                            var cells = row.SelectNodes(".//td");

                            if (cells != null && cells.Count == 2)
                            {
                                return new CurrencyRate
                                {
                                    CurrencyCode = cells[0].InnerText.Trim(),
                                    CurrencyName = cells[1].InnerText.Trim(),
                                };
                            }

                            return null;
                        })
                        .Where(rate => rate != null);
                }
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("Error fetching currency rates: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An unexpected error occurred: " + ex.Message);
        }

        return Enumerable.Empty<CurrencyRate>();
    }
}