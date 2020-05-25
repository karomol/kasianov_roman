
namespace telegrambot
{
    //Classes for deserialization airquality data
    public class Forecast_Menu
    {
        public string status { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public Location location { get; set; }
        public Current current { get; set; }

    }
    public class Location
    {
        public string type { get; set; }
        public double[] coordinates { get; set; }
    }
    public class Current
    {
        public Weather weather { get; set; }
        public Pollution pollution { get; set; }
    }
    public class Weather
    {
        public string ts { get; set; }
        public double tp { get; set; }
        public double pr { get; set; }
        public double hu { get; set; }
        public double ws { get; set; }
        public double wd { get; set; }
        public string ic { get; set; }
    }
    public class Pollution
    {
        public string ts { get; set; }
        public double aqius { get; set; }
        public string mainus { get; set; }
        public double aqicn { get; set; }
        public string maincn { get; set; }
    }
}
