using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFight.Models
{
    public class SearchResult
    {
        public string SearchEngineName { get; set; }
        public long TotalResultsCount { get; set; }
        public string SearchQuery { get; set; }

        public SearchResult(string SearchEngineName, long TotalResultsCount, string SearchQuery)
        {
            this.SearchEngineName = SearchEngineName;
            this.TotalResultsCount = TotalResultsCount;
            this.SearchQuery = SearchQuery;
        }
        public SearchResult()
        {

        }
    }
}
