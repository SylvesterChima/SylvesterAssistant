using System;
using System.Collections.Generic;
using System.Text;

namespace SAAWSLambda.ApiClient.Response
{
    public class DeezerSearchResp
    {
        public List<Data> data { get; set; }
    }

    public class Data
    {
        public string title { get; set; }
        public string title_short { get; set; }
        public string preview { get; set; }
        public Album album { get; set; }

    }

    public class Album
    {
        public int id { get; set; }
        public string title { get; set; }
        public string cover_xl { get; set; }
    }
}
