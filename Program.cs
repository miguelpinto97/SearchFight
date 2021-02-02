using SearchFight.Models;
using SearchFight.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchFight
{
    class Program
    {
        static void Main(string[] args)
        {
            GoogleSearchService GoogleSearchService = new GoogleSearchService();

            BingSearchService BingSearchService = new BingSearchService();

            List<SearchResult> SearchResultList = new List<SearchResult>();

            Console.Write("What would you like to search for? Write it here: " + Environment.NewLine + Environment.NewLine);

            string SearchQuery = Console.ReadLine();

            var words = Regex.Matches(SearchQuery, @"[\""].+?[\""]|[^ ]+")
                                        .Cast<Match>()
                                        .Select(m => m.Value)
                                        .ToList();

            foreach (string word in words)
            {
                List<SearchResult> TempSearchResultList = new List<SearchResult>();

                TempSearchResultList.Add(GoogleSearchService.GetSearchResult(word));

                TempSearchResultList.Add(BingSearchService.GetSearchResult(word));

                SearchResultList.AddRange(TempSearchResultList);

                PrintResult(TempSearchResultList);
            }

            string GoogleWinner = GetWinner(SearchResultList, Base.Constants.GOOGLE);

            string BingWinner = GetWinner(SearchResultList, Base.Constants.BING);

            long MaxValue = 0;
            string TotalWinner = "";

            foreach (var word in words)
            {
                long TempMaxValue = SearchResultList.Where(x => x.SearchQuery.Equals(word))
                                                    .Select(y => y.TotalResultsCount).Sum();
                
                if (TempMaxValue > MaxValue) 
                {   MaxValue = TempMaxValue;
                    TotalWinner = word;
                }
            }               

            Console.WriteLine($"Google Winner: {GoogleWinner}");

            Console.WriteLine($"Bing Winner: {BingWinner}");

            Console.WriteLine($"Total Winner: {TotalWinner}");

            Console.ReadKey();

        }
   
        static void PrintResult(List<SearchResult> SearchResultList)
        {
            Console.WriteLine(Environment.NewLine);
            Console.Write("Results for: " + SearchResultList.FirstOrDefault().SearchQuery + Environment.NewLine + Environment.NewLine);

            long MaxResult = SearchResultList.Select(x=>x.TotalResultsCount).OrderBy(x=>x).FirstOrDefault();

            foreach (var SearchResult in SearchResultList)
            {
                
                Console.WriteLine($"{SearchResult.SearchEngineName}: " + SearchResult.TotalResultsCount + Environment.NewLine);
            }
        }

        static string GetWinner(List<SearchResult> SearchResultList, string SearchEngine)
        {
            return SearchResultList.Where(x => x.SearchEngineName.Equals(SearchEngine))
                                                  .OrderByDescending(y => y.TotalResultsCount)
                                                  .Select(z => z.SearchQuery).FirstOrDefault();
        }
    
    }
}
