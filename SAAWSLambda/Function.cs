using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Alexa.NET.Response;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Newtonsoft.Json;
using Alexa.NET;
using SAAWSLambda.ApiClient;
using Alexa.NET.Response.Directive;
using Alexa.NET.Response.APL;
using Alexa.NET.APL.Components;
using Alexa.NET.APL;
using Alexa.NET.APL.DataSources;
using Alexa.NET.Response.Ssml;
using Alexa.NET.APL.Commands;
using System.IdentityModel.Tokens.Jwt;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SAAWSLambda
{
    public class Function
    {
        FactData _client;

        public Function()
        {
            _client = new FactData();

            
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// 



        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            ILambdaLogger log = context.Logger;
            log.LogLine($"Skill Request Object:" + JsonConvert.SerializeObject(input));

            var accessToken = input.Context.System.User.AccessToken;

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);

            var name = token.Payload["firstname"].ToString();

            //var document = new APLDocument();
            //document.AddResponsiveDesign();

            //document.MainTemplate = new Layout(
            //    new AlexaFooter("try tell me about nigeria")
            //).AsMain();

            string user_name = "";
            Session session = input.Session;

            try
            {
                if (session.Attributes == null)
                    session.Attributes = new Dictionary<string, object>();

                Type requestType = input.GetRequestType();
                if (input.GetRequestType() == typeof(LaunchRequest))
                {
                    session.Attributes["user_name"] = name;

                    string next = $"Welcome {name}, You can ask me about any country, just say tell me about Nigeria or you can say new game to start playing a simple guessing game with me";
                    Reprompt rp = new Reprompt(next);
                    var response = ResponseBuilder.Ask(next, rp, session);
                    var dr = BuildAPLDirective(next);
                    response.Response.Directives.Add(dr);
                    return response;
                }
                else if (input.GetRequestType() == typeof(SessionEndedRequest))
                {
                    user_name = (string)session.Attributes["user_name"];
                    var speech = $"Goodbye {user_name} and thanks for playing!";
                    var response = ResponseBuilder.Tell(speech);
                    var dr = BuildAPLDirective(speech);
                    response.Response.Directives.Add(dr);
                    return response;
                }
                else if (input.GetRequestType() == typeof(IntentRequest))
                {
                    var intentRequest = (IntentRequest)input.Request;
                    switch (intentRequest.Intent.Name)
                    {
                        case "AMAZON.CancelIntent":
                        case "AMAZON.StopIntent":
                            user_name = (string)session.Attributes["user_name"];
                            return ResponseBuilder.Tell($"Goodbye {user_name} and thanks for playing!");
                        case "AMAZON.HelpIntent":
                            {
                                Reprompt rp = new Reprompt("What's next?");
                                return ResponseBuilder.Ask("Here's some help. What's next?", rp, session);
                            }
                        case "AMAZON.YesIntent":
                            {
                                var lastIntent = (string)session.Attributes["last_intent"];
                                switch (lastIntent)
                                {
                                    case "CountryIntent":
                                        {
                                            var country = (string)session.Attributes["country"];
                                            var anthem = await _client.GetCountryAnthemSearch($"{country} national anthem", context);

                                            if (anthem == null)
                                            {
                                                string msg = $"Something went wrong. Please ask again.";
                                                Reprompt er = new Reprompt(msg);
                                                return ResponseBuilder.Ask(msg, er, session);
                                            }

                                            //var progressiveResponse = new ProgressiveResponse(input);
                                            //progressiveResponse.SendSpeech("With Pleasure! just hold on a second");

                                            
                                            string audioUrl = anthem.data[0].preview;
                                            string audioToken = anthem.data[0].title_short;

                                            //var response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, audioUrl, audioToken);
                                            //var dr = BuildAudioAPLDirective(anthem.data[0].album.cover_xl);
                                            var response = ResponseBuilder.Empty();
                                            response.Response.Directives.Add(AddAudioDirective(audioUrl, audioToken, anthem.data[0].album.cover_xl));
                                            return response;

                                        }
                                    default:
                                        {
                                            string msg = $"You are trying to trick me. I know i didn't ask you anything";
                                            Reprompt er = new Reprompt(msg);
                                            return ResponseBuilder.Ask(msg, er, session);
                                        }
                                }
                            }
                        case "AMAZON.NoIntent":
                            {
                                var rnd = new Random();
                                var i = rnd.Next(0, 2);
                                string msg = "";
                                if (i == 0)
                                    msg = "Remember you can still play a guessing game with me by saying New Game";
                                else
                                    msg = "You can ask me about any country by saying Tell me about the contry. example Tell me about Nigeria";

                                Reprompt er = new Reprompt(msg);
                                var response = ResponseBuilder.Ask(msg, er, session);
                                var dr = BuildAPLDirective(msg);
                                response.Response.Directives.Add(dr);
                                return response;
                            }
                        case "NewGameIntent":
                            {
                                session.Attributes["num_guesses"] = 0;
                                session.Attributes["last_intent"] = "NewGameIntent";
                                Random rnd = new Random();
                                Int32 magicNumber = rnd.Next(1, 10);
                                session.Attributes["magic_number"] = magicNumber;

                                string next = "Guess a number betwen 1 and 10";
                                Reprompt rp = new Reprompt(next);
                                var response = ResponseBuilder.Ask(next, rp, session);
                                var dr = BuildAPLDirective(next);
                                response.Response.Directives.Add(dr);
                                return response;
                            }
                        case "NewRespIntent":
                            {
                                // check answer
                                session.Attributes["last_intent"] = "NewRespIntent";
                                string userString = intentRequest.Intent.Slots["Value"].Value;
                                Int32 userInt = 0;
                                Int32.TryParse(userString, out userInt);
                                bool correct = (userInt == (Int32)(long)session.Attributes["magic_number"]);
                                Int32 numTries = (Int32)(long)session.Attributes["num_guesses"] + 1;
                                string speech = "";
                                if (correct)
                                {
                                    speech = "Correct! You guessed it in " + numTries.ToString() + " tries. Say new game to play again, or stop to exit. ";
                                    session.Attributes["num_guesses"] = 0;
                                }
                                else
                                {
                                    speech = "Nope, guess again.";
                                    session.Attributes["num_guesses"] = numTries;
                                }
                                Reprompt rp = new Reprompt("speech");
                                var response = ResponseBuilder.Ask(speech, rp, session);
                                var dr = BuildAPLDirective(speech);
                                response.Response.Directives.Add(dr);
                                return response;
                            }
                        case "CountryIntent":
                            {
                                session.Attributes["last_intent"] = "CountryIntent";
                                string countryString = intentRequest.Intent.Slots["Country"].Value;

                                if (string.IsNullOrEmpty(countryString))
                                {
                                    var msg = "I'm sorry, but I didn't understand the country you were asking for. Please ask again.";
                                    Reprompt er = new Reprompt(msg);
                                    var response = ResponseBuilder.Ask(msg, er, session);
                                    var dr = BuildAPLDirective(msg);
                                    response.Response.Directives.Add(dr);
                                    return response;
                                }

                                var countryInfo = await _client.GetCountryInfo(countryString, context);

                                if (countryInfo == null)
                                {
                                    string msg = $"I cannot recorganize {countryString} as a country. Please ask again.";
                                    Reprompt er = new Reprompt(msg);
                                    var response = ResponseBuilder.Ask(msg, er, session);
                                    var dr = BuildAPLDirective(msg);
                                    response.Response.Directives.Add(dr);
                                    return response;
                                }
                                session.Attributes["country"] = countryInfo.demonym;
                                string next = $"{countryInfo.name} is in {countryInfo.subregion}. The capital is {countryInfo.capital} and the population is {countryInfo.population}. {countryInfo.demonym} native language is {countryInfo.languages[0].name} and their currency is called {countryInfo.currencies[0].name}. Would you like me to play {countryInfo.demonym} national anthem?";
                                Reprompt rp = new Reprompt(next);
                                var resp = ResponseBuilder.Ask(next, rp, session);
                                var dre = BuildAPLDirective(next);
                                resp.Response.Directives.Add(dre);
                                return resp;
                            }
                        case "AMAZON.FallbackIntent":
                            {
                                string msg = "Sylvester Assistant does not support that. You can ask me about any country. example, Tell me about Nigeria";
                                Reprompt er = new Reprompt(msg);
                                var response = ResponseBuilder.Ask(msg, er, session);
                                var dr = BuildAPLDirective(msg);
                                response.Response.Directives.Add(dr);
                                return response;
                            }
                        default:
                            {

                                log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                                string speech = "I didn't understand - try again?";
                                Reprompt rp = new Reprompt(speech);
                                var response = ResponseBuilder.Ask(speech, rp, session);
                                var dr = BuildAPLDirective(speech);
                                response.Response.Directives.Add(dr);
                                return response;
                            }
                    }
                }
                user_name = (string)session.Attributes["user_name"];
                return ResponseBuilder.Tell($"Goodbye {user_name} and thanks for playing!");
            }
            catch (Exception ex)
            {
                string msg = "Something went wrong. Please ask again";
                Reprompt er = new Reprompt(msg);
                var response = ResponseBuilder.Ask(msg, er, session);
                var dr = BuildAPLDirective(msg);
                response.Response.Directives.Add(dr);
                return response;
            }

            
        }


        private RenderDocumentDirective BuildAPLDirective(string sentences, string bg = "")
        {
            //var sentences = "Smooth as an android's bottom, eh, Data? About four years." +
            //    "I got tired of hearing how young I looked. " +
            //    "Captain, why are we out here chasing comets? " +
            //    "Fate protects fools, little children and ships named Enterprise.";
            if (string.IsNullOrEmpty(bg))
            {
                //bg = "https://kids-advice.s3.amazonaws.com/apl-wallpaper.jpg";
                bg = "https://i.ibb.co/5cWztFH/brown-on-seashore-near-mountain-1007657.jpg";

            }
            var mainLayout = new Layout(
                new Container(
                    //new ScrollView(
                    //    //new Text(sentences)
                    //    //{
                    //    //    FontSize = "60dp",
                    //    //    TextAlign = "Center",
                    //    //    Id = "talker"
                    //    //}


                    //)
                    //{ Width = "100vw", Height = "100vh" }
                    
                    new Image(bg) { Width = "100vw", Height = "100vh",
                        Position = new APLValue<string>("absolute"),
                        Scale = new APLValue<Scale?>(Scale.BestFill) 
                    },
                    new Text(sentences)
                    {
                        FontSize = "40dp",
                        Style = new APLValue<string>("textStyleBody"),
                        TextAlign = new APLValue<string>("center"),
                        Id = "talker",
                        Content = APLValue.To<string>("${payload.script.properties.text}"),
                        Speech = APLValue.To<string>("${payload.script.properties.speech}")
                    }
                )
                { Width = "100vw", Height = "100vh", JustifyContent = new APLValue<string>("center"), Grow = new APLValue<int?>(1) }
            );
            mainLayout.Parameters = new List<Parameter>();
            mainLayout.Parameters.Add(new Parameter("payload"));

            var speech = new Speech(new PlainText(sentences));
            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = mainLayout
                },
                DataSources = new Dictionary<string, APLDataSource>
                {
                    {
                        "script",new ObjectDataSource
                        {
                            Properties = new Dictionary<string, object>
                            {
                                { "ssml", speech.ToXml()}
                            },
                            Transformers = new List<APLTransformer>{
                                APLTransformer.SsmlToText(
                                    "ssml",
                                    "text"),
                                APLTransformer.SsmlToSpeech(
                                    "ssml",
                                    "speech")
                            }
                        }

                    }
                }

            };
            return renderDocument;
        }

        private RenderDocumentDirective BuildAudioAPLDirective(string bg = "")
        {
            if (string.IsNullOrEmpty(bg))
            {
                //bg = "https://kids-advice.s3.amazonaws.com/apl-wallpaper.jpg";
                bg = "https://i.ibb.co/5cWztFH/brown-on-seashore-near-mountain-1007657.jpg";
            }
            var mainLayout = new Layout(
                new Container(

                    new Image(bg) { Width = "100vw", Height = "100vh", Scale = new APLValue<Scale?>(Scale.BestFill) }
                )
                { Width = "100vw", Height = "100vh" }
            );
            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = mainLayout
                }
            };
            return renderDocument;
        }

        private AudioPlayerPlayDirective AddAudioDirective(string url, string token, string bg)
        {
            var c = new AudioPlayerPlayDirective()
            {
                
                //PlayBehavior = PlayBehavior.ReplaceAll,
                AudioItem = new Alexa.NET.Response.Directive.AudioItem()
                {
                    Stream = new AudioItemStream()
                    {
                        Url = url,
                        Token = token
                    },
                    Metadata = new AudioItemMetadata
                    {
                        Title = "Stream One",
                        Subtitle = "A subtitle for stream one",
                        Art = new AudioItemSources
                        {
                            Sources = new List<AudioItemSource>
                            {
                                new AudioItemSource
                                {
                                    Url = bg
                                }
                            }
                        },
                        BackgroundImage = new AudioItemSources
                        {
                            Sources = new List<AudioItemSource>
                            {
                                new AudioItemSource
                                {
                                    Url = bg
                                }
                            }
                        },
                        
                    },
                    
                }

            };
            return c;
        }
    }
}
