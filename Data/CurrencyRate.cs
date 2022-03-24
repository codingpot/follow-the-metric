namespace follow_the_metric.Data
{
    public class CurrencyRate
    {
        string? apiKey = null;
        Dictionary<string, double> currencyRates = new Dictionary<string, double>();
        DateTime? date = null;
        Dictionary<string, string> descToCurrencyCode = new Dictionary<string, string>();

        public CurrencyRate()
        {
            if (apiKey == null)
            {
                apiKey = Environment.GetEnvironmentVariable("CURRENCY_API_KEY");
                if (apiKey == null)
                {
                    return;
                }
                Update();
            }
        }

        private void Update() 
        {
            var client = new HttpClient();
            var getRateTask = client.GetFromJsonAsync<CurrencyRateResponse>($"https://api.currencyfreaks.com/latest?apikey={apiKey}");
            getRateTask.Wait();
            if (getRateTask.Result != null)
            {
                currencyRates = getRateTask.Result.rates!;
                date = DateTime.Parse(getRateTask.Result.date!);
            }

            var getListTask = client.GetFromJsonAsync<CurrencyListResponse[]>($"https://api.currencyfreaks.com/supported-currencies");
            getListTask.Wait();
            if (getListTask.Result != null)
            {
                descToCurrencyCode = new Dictionary<string, string>();
                foreach (var country in getListTask.Result)
                {
                    if (country != null) {
                        descToCurrencyCode.Add($"{country.countryName} - {country.currencyName} ({country.currencyCode})", country.currencyCode!);
                    }
                }
            }
        }

        public double GetPair(string first, string second)
        {
            if (date != null)
            {
                var now = DateTime.Now;
                if (now - date >= TimeSpan.FromHours(12.0))
                {
                    Update();
                }
            }
            if (currencyRates == null)
            {
                return 1.0;
            }
            try {
                return currencyRates[descToCurrencyCode[second]] / currencyRates[descToCurrencyCode[first]];
            } catch (KeyNotFoundException) {
                return 1.0;
            }
        }

        public string[] GetCurrencys()
        {
            var keys = descToCurrencyCode.Keys.ToArray();
            Array.Sort(keys);
            return keys;
        }
    }

    public class CurrencyRateResponse
    {
        public string? date { get; set; }
        public string? @base { get; set; }
        public Dictionary<string, double>? rates { get; set; }
    }

    public class CurrencyListResponse
    {
        public string? currencyCode { get; set; }
        public string? currencyName { get; set; }
        public string? icon { get; set; }
        public string? status { get; set; }
        public string? available_in_historical_data_from { get; set; }
        public string? available_in_historical_data_till { get; set; }
        public string? countryCode { get; set; }
        public string? countryName { get; set; }
    }
}