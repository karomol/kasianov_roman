

namespace telegrambot
{
    //data for display
    public class UserSettings
    {
        private static TBot Bot = new TBot();
        public bool country { get; set; } = true;
        public bool state { get; set; } = true;
        public bool city { get; set; } = true;
        public bool tp { get; set; } = true;
        public bool pr { get; set; } = true;
        public bool hu { get; set; } = true;
        public bool ws { get; set; } = true;
        public bool wd { get; set; } = true;
        public bool ic { get; set; } = true;
        public bool aqius { get; set; } = true;
        public bool mainus { get; set; } = true;
        public bool aqicn { get; set; } = true;
        public bool maincn { get; set; } = true;

        //Method thst changes language for inline query
        public async static void LanguageChanger(string callback, string callback_id, long chat_id)
        {
            User u = Controller.GetUser(chat_id);
            switch (callback.Trim())
            {
                case "en":
                    u.language = "en";
                    await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"English selected");
                    break;
                case "us":
                    u.language = "us";
                    await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"US selected");
                    break;
                case "es":
                    u.language = "es";
                    await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"Spanish selected");
                    break;
                case "de":
                    u.language = "de";
                    await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"German selected");
                    break;
                case "it":
                    u.language = "it";
                    await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"Italian selected");
                    break;
                case "fr":
                    u.language = "fr";
                    await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"French selected");
                    break;
                case "ja":
                    u.language = "ja";
                    await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"Japanese selected");
                    break;
                case "zh":
                    u.language = "zh";
                    await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"Chinese selected");
                    break;
            }
        }
        //Method that changes datadisplay settings
        public async static void SettingsChanger(string callback, string callback_id, long chat_id)
        {
            User u = Controller.GetUser(chat_id);
            switch (callback.Trim())
            {
                case "country":
                    if (u.set.country == true)
                    {
                        u.set.country = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"country turned off");
                    }
                    else
                    {
                        u.set.country = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"country turned on");
                    }
                    break;

                case "region":
                    if (u.set.state == true)
                    {
                        u.set.state = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"region turned off");
                    }
                    else
                    {
                        u.set.state = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"region turned on");
                    }
                    break;

                case "city":
                    if (u.set.city == true)
                    {
                        u.set.city = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"city turned off");
                    }
                    else
                    {
                        u.set.city = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"city turned on");
                    }
                    break;

                case "temperature":
                    if (u.set.tp == true)
                    {
                        u.set.tp = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"temperature turned off");
                    }
                    else
                    {
                        u.set.tp = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"temperature turned on");
                    }
                    break;

                case "pressure":
                    if (u.set.pr == true)
                    {
                        u.set.pr = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"pressure turned off");
                    }
                    else
                    {
                        u.set.pr = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"pressure turned on");
                    }
                    break;

                case "humidity":
                    if (u.set.hu == true)
                    {
                        u.set.hu = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"humidity turned off");
                    }
                    else
                    {
                        u.set.hu = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"humidity turned on");
                    }
                    break;

                case "WD":
                    if (u.set.wd == true)
                    {
                        u.set.wd = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"WD turned off");
                    }
                    else
                    {
                        u.set.wd = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"WD turned on");
                    }
                    break;

                case "WS":
                    if (u.set.ws == true)
                    {
                        u.set.ws = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"WS turned off");
                    }
                    else
                    {
                        u.set.ws = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"WS turned on");
                    }
                    break;

                case "aqicn":
                    if (u.set.aqicn == true)
                    {
                        u.set.aqicn = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"aqicn turned off");
                    }
                    else
                    {
                        u.set.aqicn = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"aqicn turned on");
                    }
                    break;

                case "aqius":
                    if (u.set.aqius == true)
                    {
                        u.set.aqius = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"aqius turned off");
                    }
                    else
                    {
                        u.set.aqius = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"aqius turned on");
                    }
                    break;

                case "maincn":
                    if (u.set.maincn == true)
                    {
                        u.set.maincn = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"maincn turned off");
                    }
                    else
                    {
                        u.set.maincn = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"maincn turned on");
                    }
                    break;

                case "mainus":
                    if (u.set.mainus == true)
                    {
                        u.set.mainus = false;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"mainus turned off");
                    }
                    else
                    {
                        u.set.mainus = true;
                        await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"mainus turned on");
                    }
                    break;
                default:
                    u.set.country = true;
                    u.set.state = true;
                    u.set.city = true;
                    u.set.tp = true;
                    u.set.pr = true;
                    u.set.hu = true;
                    u.set.maincn = true;
                    u.set.mainus = true;
                    u.set.aqicn = true;
                    u.set.aqius = true;
                    u.set.wd = true;
                    u.set.ws = true;
                    await Bot.Client.AnswerCallbackQueryAsync(callback_id, $"default settings applied");
                    break;
            }
        }
    }
}
