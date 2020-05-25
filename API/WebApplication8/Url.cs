using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication8
{
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
