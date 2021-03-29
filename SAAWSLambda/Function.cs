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
using SAAWSLambda.ApiClient.Response;
using Kevsoft.Ssml;

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

        string bgUrl = "https://bestilldemo.azurewebsites.net/images/bestillbg.jpg";
        string logoUrl = "https://bestilldemo.azurewebsites.net/images/bestilllogo.png";


        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            ILambdaLogger log = context.Logger;
            log.LogLine($"Skill Request Object:" + JsonConvert.SerializeObject(input));
            //APLSupport.Add();

            var accessToken = input.Context.System.User.AccessToken;

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);

            var name = token.Payload["firstname"].ToString();

            string user_name = "";
            Session session = input.Session;

            try
            {
                if (session.Attributes == null)
                    session.Attributes = new Dictionary<string, object>();

                Type requestType = input.GetRequestType();
                if (input.GetRequestType() == typeof(LaunchRequest))
                {



                //    var sentences = "Smooth as an android's bottom, eh, Data? About four years." +
                //"I got tired of hearing how young I looked. " +
                //"Captain, why are we out here chasing comets? " +
                //"Fate protects fools, little children and ships named Enterprise."+
                //"I got tired of hearing how young I looked. " +
                //"Captain, why are we out here chasing comets? " +
                //"Fate protects fools, little children and ships named Enterprise."+
                //"I got tired of hearing how young I looked. " +
                //"Captain, why are we out here chasing comets? " +
                //"Fate protects fools, little children and ships named Enterprise."+
                //"I got tired of hearing how young I looked. " +
                //"Captain, why are we out here chasing comets? " +
                //"Fate protects fools, little children and ships named Enterprise.";
                    //var speech = new Speech(new PlainText(sentences));
                    //var mainLayout = new Layout(
                    //    new Container(
                    //        new Image
                    //        {
                    //            Width = "100vw",
                    //            Height = "100vh",
                    //            Position = new APLValue<string>("absolute"),
                    //            Scale = new APLValue<Scale?>(Scale.BestFill),
                    //            Source = new APLValue<string>(bgUrl)
                    //        },


                    //        new ScrollView(
                    //            new Text(sentences)
                    //            {
                    //                PaddingLeft = 40,
                    //                PaddingRight = 40,
                    //                PaddingBottom = 40,
                    //                FontSize = "40dp",
                    //                TextAlign = "Center",
                    //                Id = "talker",
                    //                Content = APLValue.To<string>("${payload.script.properties.text}"),
                    //                Speech = APLValue.To<string>("${payload.script.properties.speech}")
                    //            }
                    //        )
                    //        { Width = "100vw", Height = "100vh" }
                    //    )
                    //);
                    //mainLayout.Parameters = new List<Parameter>();
                    //mainLayout.Parameters.Add(new Parameter("payload"));

                    //var response = ResponseBuilder.Empty();
                    //response.Response.ShouldEndSession = false;

                    //var directive = new RenderDocumentDirective
                    //{
                    //    Token = "randomToken",
                    //    Document = new APLDocument
                    //    {
                    //        MainTemplate = mainLayout
                    //    },
                    //    DataSources = new Dictionary<string, APLDataSource>
                    //    {
                    //        {
                    //            "script",new ObjectDataSource
                    //            {
                    //                Properties = new Dictionary<string, object>
                    //                {
                    //                    { "ssml", speech.ToXml()}
                    //                },
                    //                Transformers = new List<APLTransformer>{
                    //                    APLTransformer.SsmlToText(
                    //                        "ssml",
                    //                        "text"),
                    //                    APLTransformer.SsmlToSpeech(
                    //                        "ssml",
                    //                        "speech")
                    //                }
                    //            }
                    //        }
                    //    }
                    //};


                    //response.Response.Directives.Add(directive);
                    //var speakItem = new SpeakItem
                    //{
                    //    ComponentId = "talker",
                    //    HighlightMode = new APLValue<HighlightMode?>(HighlightMode.Line)
                    //};
                    //response.Response.Directives.Add(new ExecuteCommandsDirective("randomToken", speakItem));
                    //return response;




                    session.Attributes["user_name"] = name;
                    var xml = await new Ssml().Say("Take a \n deep breath")
                        .Say("<audio src =\"soundbank://soundlibrary/transportation/amzn_sfx_car_accelerate_01\" />")
                        .Say("then continue.").ForAlexa()
                        .ToStringAsync();
                    var d = new SsmlOutputSpeech
                    {
                        Ssml = $"<speak>Welcome to Ride Hailer<audio src =\"https://bestilldemo.azurewebsites.net/images/audio10.mp3\" />You can order a ride,or request a fare estimate.Which will it be ?</speak>"
                    };
                    string next = $"Welcome {name}, \n You can ask me about any country";
                    Reprompt rp = new Reprompt(next);



                    var rt = input.Context.System.Device.IsInterfaceSupported("Display");
                    if (rt)
                    {
                        //var shape = input.Context.Viewport?.Shape;
                        //var response = ResponseBuilder.AskWithCard(next, "Welcome", next, rp);


                        //session.Attributes.Add("LastIntent", input.Request.Intent.Name);
                        //PlainTextOutputSpeech speech = new PlainTextOutputSpeech(msg);
                        //var response = ResponseBuilder.Ask(next, new Reprompt(""), session);

                        //var response = ResponseBuilder.Empty();
                        //response.Response.ShouldEndSession = false;

                        var response = ResponseBuilder.Ask(next, rp, session);
                        //var dr = new Template().BuildAPLDirective(next, null,"Sylvester Assistant", "Welcome");
                        var dr = new Template().BuildScrollingSequenceWithVoiceDirective(next, null, "Sylvester Assistant", "Welcome");

                        //var dr = new Template().BuildItemListDirective(next, null, "Sylvester Assistant", "Welcome");
                        //var dr = new Template().BuildDataListDirective(next, null,"Sylvester Assistant", "Welcome");
                        //var dr = BuildAPLDirective(next);
                        response.Response.Directives.Add(dr);
                        //var speakItem = new SpeakList
                        //{
                        //    ComponentId = "mySequence",
                        //    Start = new APLValue<int>(0),
                        //    Count = new APLValue<int>(next.Split(",").Length),
                        //    MinimumDwellTime = new APLValue<int?>(1000),
                        //    Align = new APLValue<ItemAlignment?>(ItemAlignment.Center)
                        //};
                        //response.Response.Directives.Add(new ExecuteCommandsDirective("randomToken", speakItem));
                        return response;
                    }
                    else
                    {
                        return ResponseBuilder.Ask(next, rp, session);

                    }


                }
                else if (input.GetRequestType() == typeof(SessionEndedRequest))
                {
                    user_name = (string)session.Attributes["user_name"];
                    var speech = $"Goodbye {user_name} and thanks for playing!";
                    var response = ResponseBuilder.Tell(speech);
                    var dr = new Template().BuildAPLDirective(speech, null, "Sylvester Assistant", "Welcome");
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
                        case "AudioPlayer.PlaybackNearlyFinished":
                            {
                                Reprompt rp = new Reprompt("What's next?");
                                return ResponseBuilder.Ask("Here's some help. What's next?", rp, session);
                            }
                        case "MusicIntent":
                            {
                                var query = intentRequest.Intent.Slots["searchQuery"].Value;
                                var result = await _client.SearchDeezer(query, context);
                                if (result == null)
                                {
                                    string msg = $"Could not get the music you are looking for. Please ask again.";
                                    Reprompt er = new Reprompt(msg);
                                    return ResponseBuilder.Ask(msg, er, session);
                                }
                                var response = ResponseBuilder.Empty();

                                var playlist = AddPlayListDirective(result);
                                session.Attributes["playlist"] = JsonConvert.SerializeObject(playlist);
                                session.Attributes["playlist_current_index"] = 0;
                                response.Response.Directives.Add(playlist[0]);
                                return response;
                            }
                        case "AMAZON.YesIntent":
                            {
                                var lastIntent = (string)session.Attributes["last_intent"];
                                switch (lastIntent)
                                {
                                    case "CountryIntent":
                                        {
                                            var country = (string)session.Attributes["country"];
                                            var anthem = await _client.SearchDeezer($"{country} national anthem", context);

                                            if (anthem == null)
                                            {
                                                string msg = $"Something went wrong. Please ask again.";
                                                Reprompt er = new Reprompt(msg);
                                                return ResponseBuilder.Ask(msg, er, session);
                                            }

                                            //var progressiveResponse = new ProgressiveResponse(input);
                                            //progressiveResponse.SendSpeech("With Pleasure! just hold on a second");

                                            
                                            //string audioUrl = anthem.data[0].preview;
                                            //string audioToken = anthem.data[0].title_short;

                                            //var response = ResponseBuilder.AudioPlayerPlay(PlayBehavior.ReplaceAll, audioUrl, audioToken);
                                            //var dr = BuildAudioAPLDirective(anthem.data[0].album.cover_xl);
                                            var response = ResponseBuilder.Empty();
                                            response.Response.Directives.Add(AddAudioDirective(anthem));
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
                                var dr = new Template().BuildAPLDirective(msg, null, "Sylvester Assistant", "Welcome");
                                response.Response.Directives.Add(dr);
                                return response;
                            }
                        case "NewGameIntent":
                            {
                                //session.Attributes["num_guesses"] = 0;
                                //session.Attributes["last_intent"] = "NewGameIntent";
                                //Random rnd = new Random();
                                //Int32 magicNumber = rnd.Next(1, 10);
                                //session.Attributes["magic_number"] = magicNumber;

                                //string next = "Guess a number betwen 1 and 10";
                                //Reprompt rp = new Reprompt(next);
                                //var response = ResponseBuilder.Ask(next, rp, session);
                                //var dr = BuildAPLDirective(next);
                                //response.Response.Directives.Add(dr);
                                //return response;

                                var resp = ResponseBuilder.Empty();
                                var audioUrl = "https://djejflde2khrv.cloudfront.net/broadcasts/desktop/win20200706.mp3";

                                var audioPlayer = new AudioPlayerPlayDirective()
                                {
                                    PlayBehavior = PlayBehavior.Enqueue,
                                    AudioItem = new AudioItem()
                                    {
                                        Stream = new AudioItemStream()
                                        {
                                            Url = audioUrl,
                                            Token = "Today's Devotional"
                                        },
                                        Metadata = new AudioItemMetadata
                                        {
                                            Title = "Today's Devotional",
                                            Subtitle = "hello",
                                            Art = new AudioItemSources
                                            {
                                                Sources = new List<AudioItemSource>
                                                {
                                                    new AudioItemSource
                                                    {
                                                        Url = "https://i.ibb.co/5cWztFH/brown-on-seashore-near-mountain-1007657.jpg"
                                                    }
                                                }
                                                },
                                                BackgroundImage = new AudioItemSources
                                                {
                                                    Sources = new List<AudioItemSource>
                                                {
                                                    new AudioItemSource
                                                    {
                                                        Url = "https://i.ibb.co/5cWztFH/brown-on-seashore-near-mountain-1007657.jpg"
                                                    }
                                                }
                                            },

                                        },

                                    }

                                };

                                
                                resp.Response.Directives.Add(audioPlayer);
                                return resp;
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
                                var dr = new Template().BuildAPLDirective(speech, null, "Sylvester Assistant", "Welcome");
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
                                    var dr = new Template().BuildAPLDirective(msg, null, "Sylvester Assistant", "Welcome");
                                    response.Response.Directives.Add(dr);
                                    return response;
                                }

                                var countryInfo = await _client.GetCountryInfo(countryString, context);

                                if (countryInfo == null)
                                {
                                    string msg = $"I cannot recorganize {countryString} as a country. Please ask again.";
                                    Reprompt er = new Reprompt(msg);
                                    var response = ResponseBuilder.Ask(msg, er, session);
                                    var dr = new Template().BuildAPLDirective(msg, null, "Sylvester Assistant", "Welcome");
                                    response.Response.Directives.Add(dr);
                                    return response;
                                }
                                session.Attributes["country"] = countryInfo.demonym;
                                string next = $"{countryInfo.name} is in {countryInfo.subregion}. The capital is {countryInfo.capital} and the population is {countryInfo.population}. {countryInfo.demonym} native language is {countryInfo.languages[0].name} and their currency is called {countryInfo.currencies[0].name}. Would you like me to play {countryInfo.demonym} national anthem?";
                                Reprompt rp = new Reprompt(next);
                                var resp = ResponseBuilder.Ask(next, rp, session);
                                var dre = new Template().BuildAPLDirective(next, null, "Sylvester Assistant", "Welcome");
                                resp.Response.Directives.Add(dre);
                                return resp;
                            }
                        case "QuietTimeIntent":
                            {
                                string msg = "Okay thanks";
                                Reprompt er = new Reprompt(msg);
                                var response = ResponseBuilder.Ask(msg, er, session);
                                var dr = new Template().BuildAPLDirective(msg, null, "Sylvester Assistant", "Welcome");
                                response.Response.Directives.Add(dr);
                                return response;
                            }
                        case "AMAZON.FallbackIntent":
                            {
                                string msg = "Sylvester Assistant does not support that. You can ask me about any country. example, Tell me about Nigeria";
                                Reprompt er = new Reprompt(msg);
                                var response = ResponseBuilder.Ask(msg, er, session);
                                var dr = new Template().BuildAPLDirective(msg, null, "Sylvester Assistant", "Welcome");
                                response.Response.Directives.Add(dr);
                                return response;
                            }
                        default:
                            {

                                log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                                string speech = "I didn't understand - try again?";
                                Reprompt rp = new Reprompt(speech);
                                var response = ResponseBuilder.Ask(speech, rp, session);
                                var dr = new Template().BuildAPLDirective(speech, null, "Sylvester Assistant", "Welcome");
                                response.Response.Directives.Add(dr);
                                return response;
                            }
                    }
                }
                else if(input.GetRequestType() == typeof(AudioPlayerRequest))
                {
                    var audioRequest = (AudioPlayerRequest)input.Request;
                    switch (audioRequest.AudioRequestType)
                    {
                        case AudioRequestType.PlaybackNearlyFinished:
                            {
                                var response = ResponseBuilder.Empty();

                                var jsonStr = (string)session.Attributes["playlist"];
                                var playlist = JsonConvert.DeserializeObject<List<AudioPlayerPlayDirective>>(jsonStr);
                                var index = (int)session.Attributes["playlist_current_index"];

                                if(playlist.Count == index)
                                {
                                    return response;
                                }
                                response.Response.Directives.Add(playlist[index + 1]);
                                session.Attributes["playlist_current_index"] = index + 1;
                                return response;
                            }
                        default:
                            {
                                string speech = "Something went wrong! Please try again!";
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
                var dr = new Template().BuildAPLDirective(msg, null, "Sylvester Assistant", "Welcome");
                response.Response.Directives.Add(dr);
                return response;
            }

            
        }

        private RenderDocumentDirective BuildAPL()
        {
            
            var directive = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = new Layout(new[]
                    {
                        new Container(new APLComponent[]{
                            new Text("APL in C#"){FontSize = "24dp",TextAlign= "Center"},
                            new Image("https://images.example.com/photos/2143/lights-party-dancing-music.jpg?cs=srgb&dl=cheerful-club-concert-2143.jpg&fm=jpg"){Width = 400,Height=400}
                        }){Direction = "row"}
                    })
                }
            };
            return directive;
        }


        private RenderDocumentDirective BuildAPLDirective(string sentences, string bg = "")
        {
            if (string.IsNullOrEmpty(bg))
            {
                bg = "https://i.ibb.co/5cWztFH/brown-on-seashore-near-mountain-1007657.jpg";

            }
            var mainLayout = new Layout(
                new Container(
                    
                    new Image("${payload.bodyTemplate1Data.Properties.backgroundImage.sources[0].url}") { Width = "100vw", Height = "100vh",
                        Position = new APLValue<string>("absolute"),
                        Scale = new APLValue<Scale?>(Scale.BestFill) 
                    },
                    new Text(sentences)
                    {
                        FontSize = "40dp",
                        Style = new APLValue<string>("textStyleBody"),
                        TextAlign = new APLValue<string>("center"),
                        Id = "talker",
                        Content = APLValue.To<string>("${payload.script.properties.textContent.primaryText.text}"),
                        Speech = APLValue.To<string>("${payload.script.properties.textContent.primaryText.text}")
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
                        "script",new Alexa.NET.APL.DataSources.ObjectDataSource
                        {
                            ObjectId = "bt1Sample",
                            Title = "Sylvester Assistant",
                            Properties = new Dictionary<string, object>
                            {
                                { "backgroundImage", new Dictionary<string, object>
                                    {
                                        { "contentDescription", null },
                                        { "smallSourceUrl", null },
                                        { "largeSourceUrl", null },
                                        { "sources", new List<object>
                                            {
                                                new Dictionary<string, object>{
                                                    {"url", "https://d2o906d8ln7ui1.cloudfront.net/images/BT1_Background.png" },
                                                    { "size", "small" },
                                                    { "widthPixels", 0 },
                                                    { "heightPixels", 0 }
                                                },
                                                new Dictionary<string, object>{
                                                    {"url", "https://d2o906d8ln7ui1.cloudfront.net/images/BT1_Background.png" },
                                                    { "size", "small" },
                                                    { "widthPixels", 0 },
                                                    { "heightPixels", 0 }
                                                }
                                            }
                                        }
                                    }
                                },
                                {
                                    "textContent", new Dictionary<string, object>
                                    {
                                        {
                                            "primaryText", new Dictionary<string, object>
                                            {
                                                { "type", "PlainText" },
                                                {"text", sentences }
                                            }
                                        }
                                    }
                                },
                                {
                                    "logoUrl","https://d2o906d8ln7ui1.cloudfront.net/images/cheeseskillicon.png"
                                }
                            }

                        }

                    }
                }

            };
            return renderDocument;
        }

        private RenderDocumentDirective BuildDirective(string sentences, string bg = "")
        {
            if (string.IsNullOrEmpty(bg))
            {
                bg = "https://i.ibb.co/5cWztFH/brown-on-seashore-near-mountain-1007657.jpg";

            }
            var mainLayout = new Layout(
                new Container(
                    new AlexaTextList
                    {
                        BackgroundImageSource = APLValue.To<string>("${textListData.backgroundImageSource}"),
                        HeaderTitle = APLValue.To<string>("${textListData.headerTitle}"),
                        HeaderSubtitle = APLValue.To<string>("${textListData.headerSubtitle}"),
                        HeaderDivider = true,
                        BackgroundScale = new APLValue<Scale>(Scale.BestFill),
                        BackgroundAlign = new APLValue<string>("center"),
                        BackgroundColor = new APLValue<string>("transparent"),
                        ListItems = APLValue.To<APLValue<List<AlexaTextListItem>>>("${textListData.listItemsToShow}")
                    }

                )
                { Width = "100vw", Height = "100vh" }
            );
            mainLayout.Parameters = new List<Parameter>();
            mainLayout.Parameters.Add(new Parameter("textListData"));
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
                        "textListData",new ObjectDataSource
                        {
                            Properties = new Dictionary<string, object>
                            {
                                { "headerTitle", "Alexa text list header title" },
                                { "headerSubtitle", "Header subtitle" },
                                { "backgroundImageSource", bg },

                                {
                                    "listItemsToShow", new APLValue<List<AlexaTextListItem>>()
                                    {
                                        Value = new List<AlexaTextListItem>
                                        {
                                            new AlexaTextListItem
                                            {
                                                PrimaryText = "Hello world! 1"
                                            },
                                            new AlexaTextListItem
                                            {
                                                PrimaryText = "Hello world! 2"
                                            },
                                            new AlexaTextListItem
                                            {
                                                PrimaryText = "Hello world! 3"
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            };
            return renderDocument;
        }

        private RenderDocumentDirective BuildListTextDirective(string sentences, string bg = "")
        {
            if (string.IsNullOrEmpty(bg))
            {
                //bg = "https://kids-advice.s3.amazonaws.com/apl-wallpaper.jpg";
                bg = "https://i.ibb.co/5cWztFH/brown-on-seashore-near-mountain-1007657.jpg";

            }
            var document = new APLDocument
            {
                MainTemplate = new Layout
                {
                    Parameters = new List<Parameter>
                    {
                        new Parameter("textListData")
                    },
                    Description = "List View",
                    Items = new APLComponent[]
                    {
                        new AlexaTextList
                        {
                            BackgroundImageSource=bg,
                            HeaderTitle = "Alexa text list header title",
                            HeaderSubtitle = "sub",
                            HeaderDivider = true,
                            BackgroundScale = new APLValue<Scale>(Scale.BestFill),
                            BackgroundAlign = new APLValue<string>("center"),
                            BackgroundColor = new APLValue<string>("transparent"),
                            ListItems = new APLValue<List<AlexaTextListItem>>()
                            {
                                Value = new List<AlexaTextListItem>
                                {
                                    new AlexaTextListItem
                                    {
                                        PrimaryText = "Hello world! 1"
                                    },
                                    new AlexaTextListItem
                                    {
                                        PrimaryText = "Hello world! 2"
                                    },
                                    new AlexaTextListItem
                                    {
                                        PrimaryText = "Hello world! 3"
                                    }
                                }
                            }
                        }
                    }
                }
            };
;
            var renderDocument = new RenderDocumentDirective
            {
                Token = "randToken",
                Document = document

            };
            return renderDocument;
        }


        private AudioPlayerPlayDirective AddAudioDirective( DeezerSearchResp deezer)
        {
            var c = new AudioPlayerPlayDirective()
            {
                
                //PlayBehavior = PlayBehavior.ReplaceAll,
                AudioItem = new Alexa.NET.Response.Directive.AudioItem()
                {
                    Stream = new AudioItemStream()
                    {
                        Url = deezer.data[0].preview,
                        Token = deezer.data[0].title_short
                    },
                    Metadata = new AudioItemMetadata
                    {
                        Title = deezer.data[0].album.title,
                        Subtitle = deezer.data[0].title_short,
                        Art = new AudioItemSources
                        {
                            Sources = new List<AudioItemSource>
                            {
                                new AudioItemSource
                                {
                                    Url = deezer.data[0].album.cover_xl
                                }
                            }
                        },
                        BackgroundImage = new AudioItemSources
                        {
                            Sources = new List<AudioItemSource>
                            {
                                new AudioItemSource
                                {
                                    Url = deezer.data[0].album.cover_xl
                                }
                            }
                        },
                        
                    },
                    
                }

            };
            return c;
        }

        private List<AudioPlayerPlayDirective> AddPlayListDirective(DeezerSearchResp deezer)
        {
            List<AudioPlayerPlayDirective> playlist = new List<AudioPlayerPlayDirective>();
            foreach (var item in deezer.data)
            {
                var c = new AudioPlayerPlayDirective()
                {
                    //PlayBehavior = PlayBehavior.Enqueue,
                    AudioItem = new Alexa.NET.Response.Directive.AudioItem()
                    {
                        Stream = new AudioItemStream()
                        {
                            Url = item.preview,
                            Token = item.title_short
                        },
                        Metadata = new AudioItemMetadata
                        {
                            Title = item.album.title,
                            Subtitle = item.title_short,
                            Art = new AudioItemSources
                            {
                                Sources = new List<AudioItemSource>
                                {
                                    new AudioItemSource
                                    {
                                        Url = item.album.cover_xl
                                    }
                                }
                            },
                            BackgroundImage = new AudioItemSources
                            {
                                Sources = new List<AudioItemSource>
                                {
                                    new AudioItemSource
                                    {
                                        Url = item.album.cover_xl
                                    }
                                }
                            },

                        },

                    }

                };
                playlist.Add(c);
            }
            return playlist;
        }
    }
}
