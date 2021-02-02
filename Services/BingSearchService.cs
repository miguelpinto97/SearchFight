using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SearchFight.Models;
using System;
using System.Configuration;
using System.Net.Http;

namespace SearchFight.Services
{
    public class BingSearchService : ISearchResult
    {

        public SearchResult GetSearchResult(string SearchQuery)
        {
            long TotalResultsCount = GetTotalResultsCount(SearchQuery);

            SearchResult Result = new SearchResult(Base.Constants.BING, TotalResultsCount, SearchQuery);

            return Result;
        }

        public long GetTotalResultsCount(string SearchQuery)
        {
            JObject BingR = BingResponseResults(SearchQuery);

            long TotalResultsCount = long.Parse(BingR["webPages"]["totalEstimatedMatches"].ToString());

            return TotalResultsCount;
        }

        public JObject BingResponseResults(string SearchQuery)
        {
            HttpClient BingHttpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://api.bing.microsoft.com/v7.0/")
            };

            JObject result = new JObject();

            var subscriptionKey = ConfigurationManager.AppSettings.Get("BingAPIKey");

            BingHttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            var response = BingHttpClient.GetAsync($"search?q={SearchQuery}").Result;


            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;

                result = JsonConvert.DeserializeObject<JObject>(jsonString);
            }

            return result;
        }
    }
}
