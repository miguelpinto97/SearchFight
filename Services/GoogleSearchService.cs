using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SearchFight.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SearchFight.Services
{
    public class GoogleSearchService : ISearchResult
    {
        public SearchResult GetSearchResult(string SearchQuery)
        {
            long TotalResultsCount = GetTotalResultsCount(SearchQuery);

            SearchResult Result = new SearchResult(Base.Constants.GOOGLE, TotalResultsCount, SearchQuery);

            return Result;           
        }

        public long GetTotalResultsCount(string SearchQuery)
        {
            JObject GoogleR = GoogleResponseResults(SearchQuery);

            long TotalResultsCount = long.Parse(GoogleR["queries"]["request"][0]["totalResults"].ToString());

            return TotalResultsCount;
        }

        public JObject GoogleResponseResults(string SearchQuery)
        {
            HttpClient GoogleHttpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://www.googleapis.com/")
            };

            JObject result = new JObject();

            var key = ConfigurationManager.AppSettings.Get("GoogleAPIKey");

            var response = GoogleHttpClient.GetAsync($"customsearch/v1?key={key}&cx=015438794092490447996:qwpzakkkkpw&q={SearchQuery}").Result;

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
