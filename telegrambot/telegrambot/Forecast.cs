

namespace telegrambot
{
    //Classes for deserialization locality data
    public class Forecast
    {
        public string status { get; set; }

        public Forecast_Item[] data { get; set; }
    }
    public class Forecast_Item
    {
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }
}
