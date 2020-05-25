
using System.Collections.Generic;

namespace telegrambot
{
    //classes for deserialization url data
    public class Url
    {
        public string status { get; set; }
        public List<Url_Items> news = new List<Url_Items>();
    }
    public class Url_Items
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string author { get; set; }
        public string image { get; set; }
        public string language { get; set; }
        public string[] category { get; set; }
        public string published { get; set; }
    }
}
