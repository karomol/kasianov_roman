using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace telegrambot
{
    public class Searcher
    {
        private static TBot Bot = new TBot();
        //Finds selected locality
        public static async Task<Forecast_Item[]> FindLocalityAsync(long id)
        {
            string url = null;
            User u = Controller.GetUser(id);
            if (Controller.VariableChecker(id) == 0)
            {
                url = $@"/forecast";
            }
            if (Controller.VariableChecker(id) == 1)
            {
                url = $@"/forecast/{u.country}";
            }

            if (Controller.VariableChecker(id) == 2)
            {
                url = $@"/forecast/{u.country}/{u.state}";
            }
            try
            {
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://localhost:59592/api" + url);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                Forecast obj = JsonConvert.DeserializeObject<Forecast>(resp);
                if (obj.status == "success")
                    return obj.data;
                else
                    return new Forecast_Item[1];
            }
            catch
            {
                return new Forecast_Item[1];
            }
        }
        //Finds airquality of the selected locality
        public static async Task<Forecast_Menu> FindAirQualityAsync(long id, int i)
        {
            string url = null;
            User u = Controller.GetUser(id);
            if ((Controller.VariableChecker(id) == 3) && (i == 0))
            {
                url = $@"/forecast/{u.country}/{u.state}/{u.city}";
            }
            if (i == 1)
            {
                url = $@"/forecast/{u.saved_country}/{u.saved_state}/{u.saved_city}";
            }
            try
            {
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://localhost:59592/api" + url);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                Forecast_Menu obj = JsonConvert.DeserializeObject<Forecast_Menu>(resp);
                if (obj.status == "success")
                    return obj;
                else
                    return new Forecast_Menu();
            }
            catch
            {
                return new Forecast_Menu();
            }
        }
        //Method which finds AQ of the city nearby your location
        public static async void Location(float langtitude, float londtitude, long id)
        {
            string url = $@"/forecast/location={langtitude}&{londtitude}";
            User u = Controller.GetUser(id);
            try
            {
                Console.WriteLine(langtitude);
                Console.WriteLine(londtitude);
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("http://localhost:59592/api" + url);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                Forecast_Menu obj = JsonConvert.DeserializeObject<Forecast_Menu>(resp);
                Console.WriteLine(obj.status);
                if (obj.status == "success")
                    await Bot.Client.SendTextMessageAsync(id, Controller.GetText(obj, id), replyMarkup: new ReplyKeyboardRemove());
                else
                    await Bot.Client.SendTextMessageAsync(id, "Content not found", replyMarkup: new ReplyKeyboardRemove());
            }
            catch
            {
                await Bot.Client.SendTextMessageAsync(id, "Content not found", replyMarkup: new ReplyKeyboardRemove());
            }
        }

    }
}
