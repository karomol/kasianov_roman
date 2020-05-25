
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace telegrambot
{
    public class User
    {
        public long chat_id { get; set; }
        public int keyboard_state { get; set; }
        public int saving_state { get; set; }
        public int last_id { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string saved_country { get; set; }
        public string saved_state { get; set; }
        public string saved_city { get; set; }
        public UserSettings set = new UserSettings();
        public List<string> countries_container = new List<string>();
        public List<string> states_container = new List<string>();
        public List<string> cities_container = new List<string>();
        public int keyboard_number { get; set; }
        public Message user_message { get; set; }
        public string language { get; set; } = "en";
        public bool location_status { get; set; } = false;
    }
}
