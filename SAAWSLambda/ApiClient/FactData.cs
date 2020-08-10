using Amazon.Lambda.Core;
using Newtonsoft.Json;
using RestSharp;
using SAAWSLambda.ApiClient.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SAAWSLambda.ApiClient
{
    public class FactData
    {
        private static HttpClient _httpClient;

        public FactData()
        {
            _httpClient = new HttpClient();
        }

        public async Task<Country> GetCountryInfo(string countryName, ILambdaContext context)
        {
            try
            {
                countryName = countryName.ToLowerInvariant();
                var countries = new List<Country>();

                // search by "North Korea" or "Vatican City" gives us poor results
                // instead search by both "North" and "Korea" to get better results
                var countryPartNames = countryName.Split(' ');
                if (countryPartNames.Length > 1)
                {
                    foreach (var searchPart in countryPartNames)
                    {
                        // The United States of America results in too many search requests.
                        if (searchPart != "the" || searchPart != "of")
                        {
                            countries.AddRange(await GetResultsForCountrySearch(searchPart, context));
                        }
                    }
                }
                else
                {
                    countries.AddRange(await GetResultsForCountrySearch(countryName, context));
                }

                // try to find a match on the name "korea" could return both north korea and south korea
                var bestMatch = (from c in countries
                                 where c.name.ToLowerInvariant() == countryName ||
                                 c.demonym.ToLowerInvariant() == $"{countryName}n"   // north korea hack (name is not North Korea, by demonym is North Korean)
                                 orderby c.population descending
                                 select c).FirstOrDefault();

                var match = bestMatch ?? (from c in countries
                                          where c.name.ToLowerInvariant().IndexOf(countryName) > 0
                                          || c.demonym.ToLowerInvariant().IndexOf(countryName) > 0
                                          orderby c.population descending
                                          select c).FirstOrDefault();

                if (match == null && countries.Count > 0)
                {
                    match = countries.FirstOrDefault();
                }

                return match;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<Country>> GetResultsForCountrySearch(string countryName, ILambdaContext context)
        {
            List<Country> countries = new List<Country>();
            var uri = new Uri($"https://restcountries.eu/rest/v2/name/{countryName}");
            context.Logger.LogLine($"Attempting to fetch data from {uri.AbsoluteUri}");
            try
            {
                var response = await _httpClient.GetStringAsync(uri);
                context.Logger.LogLine($"Response from URL:\n{response}");
                // TODO: (PMO) Handle bad requests
                countries = JsonConvert.DeserializeObject<List<Country>>(response);
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"\nException: {ex.Message}");
                context.Logger.LogLine($"\nStack Trace: {ex.StackTrace}");
            }
            return countries;
        }

        public async Task<DeezerSearchResp> SearchDeezer(string query, ILambdaContext context)
        {
            DeezerSearchResp deezer = new DeezerSearchResp();
            
            try
            {   var url = $"https://deezerdevs-deezer.p.rapidapi.com/search?q={query}";
                context.Logger.LogLine($"Attempting to fetch data from {url}");
                var client = new RestClient(url);
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-host", "deezerdevs-deezer.p.rapidapi.com");
                request.AddHeader("x-rapidapi-key", "f250d8c702msh0c771bf84583dc7p122966jsnb8b46d1aa031");
                IRestResponse response = await client.ExecuteAsync(request);
                deezer = JsonConvert.DeserializeObject<DeezerSearchResp>(response.Content);
                return deezer;
            } 
            catch (Exception ex)
            {
                context.Logger.LogLine($"\nException: {ex.Message}");
                context.Logger.LogLine($"\nStack Trace: {ex.StackTrace}");
            }
            return null;
        }

        //private RenderDocumentDirective BuildDirective(string sentences, string bg = "")
        //{
        //    if (string.IsNullOrEmpty(bg))
        //    {
        //        bg = "https://i.ibb.co/5cWztFH/brown-on-seashore-near-mountain-1007657.jpg";

        //    }
        //    var mainLayout = new Layout(
        //        new AlexaTextList
        //        {
        //            BackgroundImageSource = APLValue.To<string>("${textListData.properties.backgroundImageSource}"),
        //            HeaderTitle = APLValue.To<string>("${textListData.properties.headerTitle}"),
        //            HeaderSubtitle = APLValue.To<string>("${textListData.properties.headerSubtitle}"),
        //            HeaderDivider = true,
        //            BackgroundScale = new APLValue<Scale>(Scale.BestFill),
        //            BackgroundAlign = new APLValue<string>("center"),
        //            BackgroundColor = new APLValue<string>("transparent"),
        //            ListItems = APLValue.To<List<AlexaTextListItem>>("${textListData.properties.listItemsToShow}")
        //        }
        //    );
        //    mainLayout.Parameters = new List<Parameter>();
        //    mainLayout.Parameters.Add(new Parameter("textListData"));

        //    var renderDocument = new RenderDocumentDirective
        //    {
        //        Token = "randomToken",
        //        Document = new APLDocument
        //        {
        //            MainTemplate = mainLayout
        //        },

        //        DataSources = new Dictionary<string, APLDataSource>
        //        {
        //            {
        //                "textListData",new ObjectDataSource
        //                {
        //                    Properties = new Dictionary<string, object>
        //                    {
        //                        { "headerTitle", "Alexa text list header title" },
        //                        { "headerSubtitle", "Header subtitle" },
        //                        { "backgroundImageSource", bg },

        //                        {
        //                            "listItemsToShow", new APLValue<List<AlexaTextListItem>>()
        //                            {
        //                                Value = new List<AlexaTextListItem>
        //                                {
        //                                    new AlexaTextListItem
        //                                    {
        //                                        PrimaryText = "Hello world! 1"
        //                                    },
        //                                    new AlexaTextListItem
        //                                    {
        //                                        PrimaryText = "Hello world! 2"
        //                                    },
        //                                    new AlexaTextListItem
        //                                    {
        //                                        PrimaryText = "Hello world! 3"
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //    };
        //    return renderDocument;
        //}

    }
}
