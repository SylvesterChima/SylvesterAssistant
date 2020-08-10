using Alexa.NET.APL;
using Alexa.NET.APL.Commands;
using Alexa.NET.APL.Components;
using Alexa.NET.APL.DataSources;
using Alexa.NET.Response;
using Alexa.NET.Response.APL;
using Alexa.NET.Response.Ssml;
using System;
using System.Collections.Generic;
using System.Text;

namespace SAAWSLambda
{
    public class Template
    {
        string bgUrl = "https://bestilldemo.azurewebsites.net/images/bestillbg.jpg";
        string logoUrl = "https://bestilldemo.azurewebsites.net/images/bestilllogo.png";

        public RenderDocumentDirective BuildAPLDirective(string sentences, string bg = "",string title = "", string subtitle = "" )
        {
            if (!string.IsNullOrEmpty(bg))
            {
               bgUrl = bg;

            }
            var layout = new Layout
            {
                Description = "Main template",
                Parameters = new List<Parameter> { new Parameter("payload") },
                Items = new Container[]
                {
                    new Container
                    {
                        Height = "100vh",
                         Items=new APLValue<List<APLComponent>>
                         {
                             Value = new List<APLComponent>
                             {
                                 new Image
                                 {
                                    Width = "100vw",
                                    Height = "100vh",
                                    Position = new APLValue<string>("absolute"),
                                    Scale = new APLValue<Scale?>(Scale.BestFill),
                                    Source = new APLValue<string>(bgUrl)
                                 },

                                 new AlexaHeader
                                {
                                    HeaderTitle = new APLValue<string>(title),
                                    HeaderSubtitle = new APLValue<string>(subtitle),
                                    HeaderAttributionImage = new APLValue<string>(logoUrl)

                                },

                                 new Container
                                 {
                                     Grow = new APLValue<int?>(1),
                                     PaddingLeft = 60,
                                     PaddingRight = 60,
                                     PaddingBottom = 40,
                                     Items = new APLValue<List<APLComponent>>
                                     {
                                         Value = new List<APLComponent>
                                         {
                                             
                                             new Text(sentences)
                                             {
                                                FontSize = "30dp",
                                                Style = new APLValue<string>("textStyleBody"),
                                                Spacing = 12
                                             }
                                         }
                                     }
                                 }
                             }
                         }
                    }
                }
            };

            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = layout,
                    Version = APLDocumentVersion.V1_3,
                    Imports= new List<Import>
                    {
                        new Import
                        {
                            Name="alexa-layouts",
                            Version = "1.1.0"
                        }
                    }
                }

            };
            return renderDocument;
        }
        public RenderDocumentDirective BuildItemListDirective(string sentences, string bg = "", string title = "", string subtitle = "")
        {
            if (!string.IsNullOrEmpty(bg))
            {
                bgUrl = bg;

            }
            var items = new List<AlexaTextListItem>();
            foreach (var item in sentences.Split(","))
            {
                items.Add(new AlexaTextListItem { PrimaryText = item });
            }
            var layout = new Layout
            {
                Description = "Main template",
                Parameters = new List<Parameter> { new Parameter("payload") },
                Items = new Container[]
                {
                    new Container
                    {
                        Height = "100vh",
                        Items=new APLValue<List<APLComponent>>
                        {
                            Value = new List<APLComponent>
                            {

                                new AlexaTextList
                                {
                                    BackgroundImageSource = bgUrl,
                                    HeaderTitle = title,
                                    HeaderSubtitle = subtitle,
                                    HeaderAttributionImage = new APLValue<string>(logoUrl),
                                    HeaderDivider = true,
                                    ColorOverlay = new APLValue<bool?>(true),
                                    BackgroundScale = new APLValue<Scale>(Scale.BestFill),
                                    BackgroundAlign = new APLValue<string>("center"),
                                    BackgroundColor = new APLValue<string>("transparent"),
                                    ListItems = new APLValue<List<AlexaTextListItem>>()
                                    {
                                        Value = items
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = layout,
                    Version = APLDocumentVersion.V1_3,
                    Imports = new List<Import>
                    {
                        new Import
                        {
                            Name="alexa-layouts",
                            Version = "1.1.0"
                        }
                    }
                }

            };
            return renderDocument;
        }
        public RenderDocumentDirective BuildDataListDirective(string sentences, string bg = "", string title = "", string subtitle = "")
        {
            if (!string.IsNullOrEmpty(bg))
            {
                bgUrl = bg;

            }
            var items = new List<APLComponent>();
            var prayers = sentences.Split(",");
            for (int i = 0; i < prayers.Length; i++)
            {
                items.Add(new Text($"{i + 1}. {prayers[i]}") {
                    FontSize = "30dp",
                    Spacing = 12,
                    PaddingLeft = 60,
                    PaddingRight = 60
                });
            }
            var layout = new Layout
            {
                Description = "Main template",
                Parameters = new List<Parameter> { new Parameter("payload") },
                Items = new Container[]
                {
                    new Container
                    {
                        Height = "100vh",
                         Items=new APLValue<List<APLComponent>>
                         {
                             Value = new List<APLComponent>
                             {
                                 new Image
                                 {
                                    Width = "100vw",
                                    Height = "100vh",
                                    Position = new APLValue<string>("absolute"),
                                    Scale = new APLValue<Scale?>(Scale.BestFill),
                                    Source = new APLValue<string>(bgUrl)
                                 },

                                 new AlexaHeader
                                {
                                    HeaderTitle = new APLValue<string>(title),
                                    HeaderSubtitle = new APLValue<string>(subtitle),
                                    HeaderAttributionImage = new APLValue<string>(logoUrl)

                                },

                                 new Container
                                 {
                                     Grow = new APLValue<int?>(1),
                                     PaddingLeft = 60,
                                     PaddingRight = 60,
                                     PaddingBottom = 40,
                                     Items = new APLValue<List<APLComponent>>
                                     {
                                         Value = items
                                     }
                                 }
                             }
                         }
                    }
                }
            };

            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = layout,
                    Version = APLDocumentVersion.V1_3,
                    Imports = new List<Import>
                    {
                        new Import
                        {
                            Name="alexa-layouts",
                            Version = "1.1.0"
                        }
                    }
                }

            };
            return renderDocument;
        }
        public RenderDocumentDirective BuildScrollingTextDirective(string sentences, string bg = "", string title = "", string subtitle = "")
        {
            if (!string.IsNullOrEmpty(bg))
            {
                bgUrl = bg;

            }
            var speech = new Speech(new PlainText(sentences));

            var layout = new Layout
            {
                Parameters = new List<Parameter> { new Parameter("payload") },

                Items = new Container[]
                {
                    new Container
                    {
                        Height = "100vh",
                         Items=new APLValue<List<APLComponent>>
                         {
                             Value = new List<APLComponent>
                             {
                                 new Image
                                 {
                                    Width = "100vw",
                                    Height = "100vh",
                                    Position = new APLValue<string>("absolute"),
                                    Scale = new APLValue<Scale?>(Scale.BestFill),
                                    Source = new APLValue<string>(bgUrl)
                                 },

                                 new AlexaHeader
                                {
                                    HeaderTitle = new APLValue<string>(title),
                                    HeaderSubtitle = new APLValue<string>(subtitle),
                                    HeaderAttributionImage = new APLValue<string>(logoUrl)

                                },

                                 new ScrollView(
                                    new Text(sentences)
                                    {
                                        FontSize = "40dp",
                                        TextAlign = "Center",
                                        Id = "talker",
                                        Content = APLValue.To<string>("${payload.script.properties.text}"),
                                        Speech = APLValue.To<string>("${payload.script.properties.speech}"),
                                        OnMount = new APLValue<IList<APLCommand>>
                                        {
                                            Value = new List<APLCommand>
                                            {
                                                new SpeakItem
                                                {
                                                    ComponentId = "talker",
                                                    HighlightMode = new APLValue<HighlightMode?>(HighlightMode.Line)
                                                }
                                            }
                                        }
                                    }
                                )
                                { Width = "100vw", Height = "100vh",PaddingLeft = 60, PaddingRight = 60, PaddingBottom = 140 }
                             }
                         }
                    }
                }

            };

            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = layout,
                    Version = APLDocumentVersion.V1_3,
                    Imports = new List<Import>
                    {
                        new Import
                        {
                            Name="alexa-layouts",
                            Version = "1.1.0"
                        }
                    }

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
        public RenderDocumentDirective BuildScrollingSequenceDirective(string sentences, string bg = "", string title = "", string subtitle = "")
        {
            if (!string.IsNullOrEmpty(bg))
            {
                bgUrl = bg;

            }
            var items = new List<APLComponent>();
            var prayers = sentences.Replace("\n","").Split(",");
            for (int i = 0; i < prayers.Length; i++)
            {
                items.Add(new Text()
                {
                    Id = $"itm{i.ToString()}",
                    Content = APLValue.To<string>("${payload.script.properties.text" + i.ToString() + "}"),
                    //Speech = APLValue.To<string>("${payload.script.properties.speech" + i.ToString() + "}")
                });
            }

            var ppt = new Dictionary<string, object>();
            Speech speech = null;
            for (int i = 0; i < prayers.Length; i++)
            {
                speech = new Speech(new PlainText($"{i + 1}. {prayers[i]}"));
                ppt.Add($"ssml{i}", speech.ToXml());
            }

            var ff = new List<APLTransformer>();
            for (int i = 0; i < prayers.Length; i++)
            {
                ff.Add(APLTransformer.SsmlToText($"ssml{i}", $"text{i}"));
                ff.Add(APLTransformer.SsmlToSpeech($"ssml{i}", $"speech{i}"));
            }

            var layout = new Layout
            {
                Parameters = new List<Parameter> { new Parameter("payload") },

                Items = new Container[]
                {
                    new Container
                    {
                        Height = "100vh",
                         Items=new APLValue<List<APLComponent>>
                         {
                             Value = new List<APLComponent>
                             {
                                 new AlexaBackground
                                 {
                                    Width = "100vw",
                                    Height = "100vh",
                                    Position = new APLValue<string>("absolute"),
                                    BackgroundVideoSource = new APLValue<VideoSource>
                                    {
                                        Value = new VideoSource
                                        {
                                            Uri = new APLValue<Uri>(new Uri("https://bestilldemo.azurewebsites.net/images/gifbg.gif")),
                                            RepeatCount = new APLValue<int?>(10)
                                        }
                                    },
                                    VideoAutoPlay = new APLValue<bool?>(true)
                                 },

                                 new AlexaHeader
                                {
                                    HeaderTitle = new APLValue<string>(title),
                                    HeaderSubtitle = new APLValue<string>(subtitle),
                                    HeaderAttributionImage = new APLValue<string>(logoUrl)

                                },

                                 new Sequence
                                 {
                                    Id = "mySequence",
                                    Width = "100vw",
                                    Height = "100vh",
                                     PaddingLeft = 60,
                                     PaddingRight = 60,
                                     PaddingBottom = 140,
                                    Items = new APLValue<List<APLComponent>>
                                    {
                                        Value = items
                                    },
                                    OnMount = new APLValue<IList<APLCommand>>
                                    {
                                        Value = new List<APLCommand>
                                        {
                                            new SpeakList
                                            {
                                                ComponentId = "mySequence",
                                                Start = new APLValue<int>(0),
                                                Count = new APLValue<int>(prayers.Length),
                                                MinimumDwellTime = new APLValue<int?>(30000),
                                                ScreenLock = new APLValue<bool?>(true),
                                                Align = new APLValue<ItemAlignment?>(ItemAlignment.Center),
                                            }
                                        }
                                    }
                                 }

                                 
                             }
                         }
                    }
                }

            };

            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = layout,
                    Version = APLDocumentVersion.V1_3,
                    Imports = new List<Import>
                    {
                        new Import
                        {
                            Name="alexa-layouts",
                            Version = "1.1.0"
                        }
                    }

                },
                DataSources = new Dictionary<string, APLDataSource>
                {
                    {
                        "script",new ObjectDataSource
                        {
                            Properties = ppt,
                            Transformers = ff
                        }
                    }
                }
            };
            return renderDocument;
        }
        public RenderDocumentDirective BuildAutoPagerDirective(string sentences, string bg = "", string title = "", string subtitle = "")
        {
            if (!string.IsNullOrEmpty(bg))
            {
                bgUrl = bg;

            }
            var items = new List<APLComponent>();
            var prayers = sentences.Replace("\n", "").Split(",");
            for (int i = 0; i < prayers.Length; i++)
            {
                items.Add(new Text($"{prayers[i]} \n                                                ")
                {
                    //Id = $"itm{i.ToString()}",
                    TextAlign = new APLValue<string>("center"),
                    TextAlignVertical  = new APLValue<string>("center"),
                    TextOverflow = new APLValue<TextOverflow?>(TextOverflow.Wrap),
                    Width = "100vw"
                    //Content = APLValue.To<string>("${payload.script.properties.text" + i.ToString() + "}"),
                    //Speech = APLValue.To<string>("${payload.script.properties.speech" + i.ToString() + "}")
                });
            }

            //var ppt = new Dictionary<string, object>();
            //Speech speech = null;
            //for (int i = 0; i < prayers.Length; i++)
            //{
            //    speech = new Speech(new PlainText($"{i + 1}. {prayers[i]}"));
            //    ppt.Add($"ssml{i}", speech.ToXml());
            //}

            //var ff = new List<APLTransformer>();
            //for (int i = 0; i < prayers.Length; i++)
            //{
            //    ff.Add(APLTransformer.SsmlToText($"ssml{i}", $"text{i}"));
            //    ff.Add(APLTransformer.SsmlToSpeech($"ssml{i}", $"speech{i}"));
            //}

            var layout = new Layout
            {
                Parameters = new List<Parameter> { new Parameter("payload") },

                Items = new Container[]
                {
                    new Container
                    {
                        Height = "100vh",
                         Items=new APLValue<List<APLComponent>>
                         {
                             Value = new List<APLComponent>
                             {
                                 new AlexaBackground
                                 {
                                    Width = "100vw",
                                    Height = "100vh",
                                    Position = new APLValue<string>("absolute"),
                                    BackgroundVideoSource = new APLValue<VideoSource>
                                    {
                                        Value = new VideoSource
                                        {
                                            Uri = new APLValue<Uri>(new Uri("https://bestilldemo.azurewebsites.net/images/soundwave.mp4")),
                                            RepeatCount = new APLValue<int?>(10)
                                        }
                                    },
                                    VideoAutoPlay = new APLValue<bool?>(true)
                                 },

                                 new AlexaHeader
                                {
                                    HeaderTitle = new APLValue<string>(title),
                                    HeaderSubtitle = new APLValue<string>(subtitle),
                                    HeaderAttributionImage = new APLValue<string>(logoUrl)

                                },

                                 new Pager
                                 {
                                    Id = "myPager",
                                    Width = "100vw",
                                    Height = "100vh",
                                     PaddingLeft = 60,
                                     PaddingRight = 60,
                                     PaddingBottom = 140,
                                    Items = new APLValue<List<APLComponent>>
                                    {
                                        Value = items
                                    },
                                    OnMount = new APLValue<IList<APLCommand>>
                                    {
                                        Value = new List<APLCommand>
                                        {
                                            new AutoPage
                                            {
                                                ComponentId = "myPager",
                                                ScreenLock = new APLValue<bool?>(true),
                                                DelayMilliseconds = new APLValue<int?>(40000),
                                                Count = new APLValue<int?>(items.Count),
                                                Duration = new APLValue<int?>(40000)
                                            }
                                        }
                                    }
                                 }


                             }
                         }
                    }
                }

            };

            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = layout,
                    Version = APLDocumentVersion.V1_3,
                    Imports = new List<Import>
                    {
                        new Import
                        {
                            Name="alexa-layouts",
                            Version = "1.1.0"
                        }
                    }

                },
                //DataSources = new Dictionary<string, APLDataSource>
                //{
                //    {
                //        "script",new ObjectDataSource
                //        {
                //            Properties = ppt,
                //            Transformers = ff
                //        }
                //    }
                //}
            };
            return renderDocument;
        }

        public RenderDocumentDirective BuildScrollingSequenceWithVoiceDirective(string sentences, string bg = "", string title = "", string subtitle = "")
        {

            if (!string.IsNullOrEmpty(bg))
            {
                bgUrl = bg;

            }
            var items = new List<APLComponent>();
            var prayers = sentences.Replace("\n", "").Split(",");
            for (int i = 0; i < prayers.Length; i++)
            {
                items.Add(new Text()
                {
                    Id = $"itm{i.ToString()}",
                    Width = "100vw",
                    Height = "100vh",
                    PaddingLeft = 40,
                    PaddingRight = 40,
                    TextAlign = new APLValue<string>("center"),
                    AlignSelf = new APLValue<string>("center"),
                    TextAlignVertical =new APLValue<string>("center"),
                    Content = APLValue.To<string>("${payload.script.properties.text" + i.ToString() + "}"),
                    //Speech = APLValue.To<string>("${payload.script.properties.speech" + i.ToString() + "}")
                });
            }

            var ppt = new Dictionary<string, object>();
            Speech speech = null;
            for (int i = 0; i < prayers.Length; i++)
            {
                speech = new Speech(new PlainText($"{i + 1}. {prayers[i]}"));
                ppt.Add($"ssml{i}", speech.ToXml());
            }

            var ff = new List<APLTransformer>();
            for (int i = 0; i < prayers.Length; i++)
            {
                ff.Add(APLTransformer.SsmlToText($"ssml{i}", $"text{i}"));
                ff.Add(APLTransformer.SsmlToSpeech($"ssml{i}", $"speech{i}"));
            }

            var layout = new Layout
            {
                Parameters = new List<Parameter> { new Parameter("payload") },

                Items = new Container[]
                {
                    new Container
                    {
                        Height = "100vh",
                         Items=new APLValue<List<APLComponent>>
                         {
                             Value = new List<APLComponent>
                             {

                                 //new Video
                                 //{
                                 //   Width = "100vw",
                                 //   Height = "100vh",
                                 //   Position = new APLValue<string>("absolute"),
                                 //   Scale = new APLValue<Scale>(Scale.BestFill),
                                 //   Autoplay = new APLValue<bool?>(true),
                                 //   AudioTrack = new APLValue<string>("none"),
                                 //   //Source = new APLValue<string>("https://bestilldemo.azurewebsites.net/images/gifbg.gif")
                                 //   Source = new APLValue<IList<VideoSource>>
                                 //   {
                                 //       Value = new List<VideoSource>
                                 //       {
                                 //           new VideoSource
                                 //           {
                                 //               RepeatCount = new APLValue<int?>(10),
                                 //               Uri = new APLValue<Uri>(new Uri("https://bestilldemo.azurewebsites.net/images/soundwave.mp4"))
                                 //           }
                                 //       }
                                 //   }
                                 //},
                                 new AlexaBackground
                                 {
                                    Width = "100vw",
                                    Height = "100vh",
                                    Position = new APLValue<string>("absolute"),
                                    VideoAudioTrack = new APLValue<string>("none"),
                                    BackgroundVideoSource = new APLValue<VideoSource>
                                    {
                                        Value = new VideoSource
                                        {
                                            Uri = new APLValue<Uri>(new Uri("https://bestilldemo.azurewebsites.net/images/soundwave.mp4")),
                                            RepeatCount = new APLValue<int?>(10)
                                        }
                                    },
                                    VideoAutoPlay = new APLValue<bool?>(true)
                                 },

                                 new AlexaHeader
                                {
                                    HeaderTitle = new APLValue<string>(title),
                                    HeaderSubtitle = new APLValue<string>(subtitle),
                                    HeaderAttributionImage = new APLValue<string>(logoUrl)

                                 },

                                 new Pager
                                 {
                                    Id = "myPager",
                                    Width = "100vw",
                                    Height = "100vh",
                                    PaddingBottom = 140,
                                    AlignSelf = new APLValue<string>("center"),
                                    Items = new APLValue<List<APLComponent>>
                                    {
                                        Value = items
                                    },
                                    OnMount = new APLValue<IList<APLCommand>>
                                    {
                                        Value = new List<APLCommand>
                                        {

                                            new AutoPage
                                            {
                                                ComponentId = "myPager",
                                                ScreenLock = new APLValue<bool?>(true),
                                                DelayMilliseconds = new APLValue<int?>(40000),
                                                Count = new APLValue<int?>(items.Count),
                                                Duration = new APLValue<int?>(40000),
                                            }

                                            //new Parallel
                                            //{
                                            //    Commands= new APLValue<IList<APLCommand>>
                                            //    {
                                            //        Value = new List<APLCommand>
                                            //        {
                                            //            new SpeakList
                                            //            {
                                            //                ComponentId = "myPager",
                                            //                Start = new APLValue<int>(0),
                                            //                Count = new APLValue<int>(items.Count),
                                            //                MinimumDwellTime = new APLValue<int?>(40000),
                                            //                ScreenLock = new APLValue<bool?>(true),
                                            //                Align = new APLValue<ItemAlignment?>(ItemAlignment.Center),
                                            //            },

                                            //            new AutoPage
                                            //            {
                                            //                ComponentId = "myPager",
                                            //                ScreenLock = new APLValue<bool?>(true),
                                            //                DelayMilliseconds = new APLValue<int?>(40000),
                                            //                Count = new APLValue<int?>(items.Count),
                                            //                Duration = new APLValue<int?>(40000),
                                            //            }
                                            //        }
                                            //    }
                                            //}

                                        }
                                    }
                                 }

                                 

                             }
                         }
                    }
                }

            };

            var renderDocument = new RenderDocumentDirective
            {
                Token = "randomToken",
                Document = new APLDocument
                {
                    MainTemplate = layout,
                    Version = APLDocumentVersion.V1_3,
                    Imports = new List<Import>
                    {
                        new Import
                        {
                            Name="alexa-layouts",
                            Version = "1.1.0"
                        }
                    }

                },
                DataSources = new Dictionary<string, APLDataSource>
                {
                    {
                        "script",new ObjectDataSource
                        {
                            Properties = ppt,
                            Transformers = ff
                        }
                    }
                }
            };
            return renderDocument;















        }

    }
}