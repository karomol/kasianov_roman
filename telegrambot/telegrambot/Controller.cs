using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using System.Collections.Generic;
using File = System.IO.File;
using System.Net.Http;
using System.Threading;
using Telegram.Bot.Types.InlineQueryResults;

namespace telegrambot
{
    public class Controller
    {        
        static public Dictionary<long, User> users = JsonConvert.DeserializeObject<Dictionary<long, User>>(File.ReadAllText("json1.json"));
        
        private static TBot Bot = new TBot();

        static int last_message;

        private static async void MessageDistributor(Message message, long id)
        {
            User u = GetUser(id);
            u.chat_id = message.Chat.Id;
            u.user_message = message;
            if (u.user_message.Type == MessageType.Location)
            {
                try { Searcher.Location(message.Location.Latitude, message.Location.Longitude, message.Chat.Id); }
                catch { }
            }
            if (u.user_message == null || u.user_message.Type != MessageType.Text)
                return;
            if (u.saving_state != 0)
            {
                u.saving_state = 0;
                Nulling(u.user_message.Chat.Id);
            }
            switch (u.user_message.Text.Split(' ').First())
            {
                case "/airquality":
                    try
                    {
                        await Bot.Client.DeleteMessageAsync(u.user_message.Chat.Id, u.last_id);
                    }
                    catch { }
                    try
                    {
                        Nulling(u.user_message.Chat.Id);
                        Console.WriteLine($"chat:{u.chat_id} - last_message = { last_message}");
                        await Bot.Client.SendTextMessageAsync(u.user_message.Chat.Id, text: "...");
                        u.last_id = last_message + 1;
                        u.keyboard_state = 1;
                        DataHandler(u.user_message.Chat.Id);
                    }
                    catch
                    {
                        await Bot.Client.SendTextMessageAsync(u.user_message.Chat.Id, "Wait a little and try again");
                    }
                    break;

                case "/settings":
                    await Keyboard.SendSettingsKeyboard(u.user_message.Chat.Id);
                    break;
                case "/location":
                    await RequestLocation(u.user_message.Chat.Id);
                    break;
                case "/inlinelang":
                    await Keyboard.SendLanguageKeyboard(u.user_message.Chat.Id);
                    break;                
                case "/savedcity":
                    string text;
                    try
                    {
                        if (u.saved_city != null)
                        {
                            text = GetText(await Searcher.FindAirQualityAsync(u.user_message.Chat.Id, 1), u.user_message.Chat.Id);
                        }
                        else
                            text = "you haven't saved any city yet, in order to do it click here /airquality";
                        await Bot.Client.SendTextMessageAsync(u.user_message.Chat.Id, text: text);
                    }
                    catch { }
                    break;

                default:
                    if (u.user_message.EntityValues == null)                                          
                        await Bot.Client.SendTextMessageAsync(u.user_message.Chat.Id, text: help, replyMarkup: new ReplyKeyboardRemove());
                    break;
            }            
            u.user_message = null;
        }
        //message checker
        private static void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;
            DictionaryRecorder(message.Chat.Id);
            MessageDistributor(message, message.From.Id);
            last_message = message.MessageId;
        }
        // Process Inline Keyboard callback data        
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            string error_text = "An error has occured, try again!";
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            Console.WriteLine(callbackQuery.Data);
            User u = GetUser(callbackQuery.Message.Chat.Id);
            if ((callbackQuery.Data[0] == '1') && (u.keyboard_state != 0))
            {
                string edited_calldata = callbackQuery.Data.Remove(0, 2).Trim();
                if ((VariableChecker(callbackQuery.Message.Chat.Id) == 0) && (u.keyboard_state == 1) && (callbackQuery.Data[0] == '1') && (callbackQuery.Data[1] == '1'))
                {
                    if (edited_calldata == "1")
                        Keyboard.Shift(callbackQuery.Message.Chat.Id, 1, 1);
                    if (edited_calldata == "2")
                        Keyboard.Shift(callbackQuery.Message.Chat.Id, 2, 1);
                    if ((edited_calldata != "1") && (edited_calldata != "2") && (edited_calldata != null) && (edited_calldata != ""))
                    {
                        try
                        {
                            u.keyboard_state = 2;
                            await Bot.Client.AnswerCallbackQueryAsync(callbackQuery.Id, $"You choosed {edited_calldata}");
                            u.country = edited_calldata;
                            u.keyboard_number = 0;
                            DataHandler(callbackQuery.Message.Chat.Id);
                            DataDeleter(callbackQuery.Message.Chat.Id, 1);
                        }
                        catch
                        {
                            await Bot.Client.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id, text: error_text);
                            Nulling(callbackQuery.Message.Chat.Id);
                        }
                    }
                }
                if ((VariableChecker(callbackQuery.Message.Chat.Id) == 1) && (u.keyboard_state == 2) && (callbackQuery.Data[0] == '1') && (callbackQuery.Data[1] == '2'))
                {
                    if (edited_calldata == "1")
                        Keyboard.Shift(callbackQuery.Message.Chat.Id, 1, 2);
                    if (edited_calldata == "2")
                        Keyboard.Shift(callbackQuery.Message.Chat.Id, 2, 2);
                    if ((edited_calldata != "1") && (edited_calldata != "2") && (edited_calldata != null) && (edited_calldata != ""))
                    {
                        try
                        {
                            u.keyboard_state = 3;
                            await Bot.Client.AnswerCallbackQueryAsync(callbackQuery.Id, $"You choosed {edited_calldata}");
                            u.state = edited_calldata;
                            u.keyboard_number = 0;
                            DataHandler(callbackQuery.Message.Chat.Id);
                            DataDeleter(callbackQuery.Message.Chat.Id, 2);
                        }
                        catch
                        {
                            await Bot.Client.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id, text: error_text);
                            Nulling(callbackQuery.Message.Chat.Id);
                        }
                    }
                }
                if ((VariableChecker(callbackQuery.Message.Chat.Id) == 2) && (u.keyboard_state == 3) && (callbackQuery.Data[0] == '1') && (callbackQuery.Data[1] == '3'))
                {
                    if (edited_calldata == "1")
                        Keyboard.Shift(callbackQuery.Message.Chat.Id, 1, 3);
                    if (edited_calldata == "2")
                        Keyboard.Shift(callbackQuery.Message.Chat.Id, 2, 3);
                    if ((edited_calldata != "1") && (edited_calldata != "2") && (edited_calldata != null) && (edited_calldata != ""))
                    {
                        try
                        {
                            await Bot.Client.DeleteMessageAsync(callbackQuery.Message.Chat.Id, u.last_id);
                            u.keyboard_state = 0;
                            await Bot.Client.AnswerCallbackQueryAsync(callbackQuery.Id, $"You choosed {edited_calldata}");
                            u.city = edited_calldata;
                            DataDeleter(callbackQuery.Message.Chat.Id, 3);
                            string text = GetText(await Searcher.FindAirQualityAsync(callbackQuery.Message.Chat.Id, 0), callbackQuery.Message.Chat.Id);
                            await Bot.Client.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id, text: text);
                            u.saving_state = 1;

                            await Keyboard.SendSaveButton(callbackQuery.Message.Chat.Id);
                        }
                        catch
                        {
                            await Bot.Client.SendTextMessageAsync(chatId: callbackQuery.Message.Chat.Id, text: "The information about this city is absent, try again later or choose another city");
                            Nulling(callbackQuery.Message.Chat.Id);
                        }
                    }
                }
            }
            if ((callbackQuery.Data[0] == '2'))
            {
                string callback = callbackQuery.Data.Replace('2', ' ');
                UserSettings.SettingsChanger(callback, callbackQuery.Id, callbackQuery.Message.Chat.Id);
            }
            if ((callbackQuery.Data[0] == '3'))
            {
                if (u.city != null)
                {
                    u.last_id = 0;
                    u.saved_country = u.country;
                    u.saved_state = u.state;
                    u.saved_city = u.city;
                    Nulling(callbackQuery.Message.Chat.Id);
                    await Bot.Client.AnswerCallbackQueryAsync(callbackQuery.Id, $"City was saved!");
                }
            }
            if ((callbackQuery.Data[0] == '4'))
            {
                string callback = callbackQuery.Data.Replace('4', ' ');
                UserSettings.LanguageChanger(callback, callbackQuery.Id, callbackQuery.Message.Chat.Id);
            }
        }
        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            if ((inlineQueryEventArgs.InlineQuery.Query != null) && (inlineQueryEventArgs.InlineQuery.Query != ""))
            {
                Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");
                string language;
                if (users.ContainsKey(inlineQueryEventArgs.InlineQuery.From.Id))
                {
                    User u = GetUser(inlineQueryEventArgs.InlineQuery.From.Id);
                    language = u.language;
                }
                else
                    language = "en";
                string[] inlineQuery = inlineQueryEventArgs.InlineQuery.Query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    string url;
                    if (inlineQuery.Length == 1)
                        url = $"/{inlineQuery[0]}&{language}";
                    else
                        url = $"/{inlineQuery[0]}&{language}/{inlineQuery[1]}";
                    Console.WriteLine(url);
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync("http://localhost:59592/api/news" + url);
                    response.EnsureSuccessStatusCode();
                    var resp = await response.Content.ReadAsStringAsync();
                    Url obj = JsonConvert.DeserializeObject<Url>(resp);
                    Console.WriteLine(obj.status);

                    if (obj.status != "failure")
                    {
                        InlineQueryResultBase[] results = new InlineQueryResultBase[obj.news.Count];
                        for (int i = 0; i < results.Length; i++)
                        {
                            if (i < 50)
                            {
                                string cat = null;
                                if (obj.news[i].category.Length != 0)
                                {
                                    cat = $"{obj.news[i].category[obj.news[i].category.Length - 1]}: ";
                                }
                                results[i] = new InlineQueryResultArticle(
                                    id: $"{i}",
                                    title: $"{cat}{obj.news[i].title}",
                                    inputMessageContent: new InputTextMessageContent(
                                        $"{obj.news[i].url}")
                                    );
                            }
                            else
                                break;
                        }
                        await Bot.Client.AnswerInlineQueryAsync(
                                inlineQueryId: inlineQueryEventArgs.InlineQuery.Id,
                                results: results,
                                isPersonal: true,
                                cacheTime: 0
                            );
                    }
                }
                catch { }
            }
        }
        //Checks the value of differnt fields
        static public int VariableChecker(long id)
        {
            User u = GetUser(id);
            if (u.country == null)
                return 0;
            if ((u.country != null) && (u.state == null))
                return 1;
            if ((u.country != null) && (u.state != null) && (u.city == null))
                return 2;
            if ((u.country != null) && (u.state != null) && (u.city != null))
                return 3;
            else
                return -1;
        }
        //Data handler
        private static async void DataHandler(long chat_id)
        {
            Forecast_Item[] newone = await Searcher.FindLocalityAsync(chat_id);
            User u = GetUser(chat_id);
            if (newone[0] != null)
            {
                if (VariableChecker(chat_id) == 0)
                {
                    foreach (Forecast_Item item in newone)
                        u.countries_container.Add(item.country);
                    Thread.Sleep(500);
                    await Keyboard.SendLocalityKeyboard(chat_id);
                }
                if (VariableChecker(chat_id) == 1)
                {
                    foreach (Forecast_Item item in newone)
                        u.states_container.Add(item.state);
                    Thread.Sleep(500);
                    await Keyboard.SendLocalityKeyboard(chat_id);
                }

                if (VariableChecker(chat_id) == 2)
                {
                    foreach (Forecast_Item item in newone)
                        u.cities_container.Add(item.city);
                    Thread.Sleep(500);
                    await Keyboard.SendLocalityKeyboard(chat_id);
                }
            }
            else
            {
                await Bot.Client.DeleteMessageAsync(u.chat_id, u.last_id);
                Nulling(chat_id);
                await Bot.Client.SendTextMessageAsync(chat_id, "content not found");
            }
        }
        //Data deleter
        private static void DataDeleter(long chat_id, int i)
        {
            User u = GetUser(chat_id);
            if ((i == 3) && (u.cities_container.Count != 0))
            {
                for (int j = u.cities_container.Count - 1; j >= 0; j--)
                {
                    u.cities_container.RemoveAt(j);
                }
            }
            if ((i == 2) && (u.states_container.Count != 0))
            {
                for (int j = u.states_container.Count - 1; j >= 0; j--)
                {
                    u.states_container.RemoveAt(j);
                }
            }
            if ((i == 1) && (u.countries_container.Count != 0))
            {
                for (int j = u.countries_container.Count - 1; j >= 0; j--)
                {
                    u.countries_container.RemoveAt(j);
                }
            }

            if (i == 0)
            {
                if (u.cities_container.Count != 0)
                {
                    for (int j = u.cities_container.Count - 1; j >= 0; j--)
                    {
                        u.cities_container.RemoveAt(j);
                    }
                }
                if (u.states_container.Count != 0)
                {
                    for (int j = u.states_container.Count - 1; j >= 0; j--)
                    {
                        u.states_container.RemoveAt(j);
                    }
                }
                if (u.countries_container.Count != 0)
                {
                    for (int j = u.countries_container.Count - 1; j >= 0; j--)
                    {
                        u.countries_container.RemoveAt(j);
                    }
                }
            }
        }                
        public static async Task Main()
        {            
            var me = await Bot.Client.GetMeAsync();
            Console.Title = me.Username;

            Bot.Client.OnMessage += BotOnMessageReceived;
            Bot.Client.OnMessageEdited += BotOnMessageReceived;
            Bot.Client.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.Client.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.Client.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            File.WriteAllText("json1.json", JsonConvert.SerializeObject(users));
            Bot.Client.StopReceiving();
        }
        //Record a new user to a dictionary
        static void DictionaryRecorder(long id)
        {
            if (users.ContainsKey(id) != true)
                users.Add(id, new User());
        }
        //Returns User object
        public static User GetUser(long id)
        {
            try
            {
                return users[id];
            }
            catch
            {
                DictionaryRecorder(id);
                return users[id];
            }
        }
        //            
        static async Task RequestLocation(long id)
        {
            var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
            {
                    KeyboardButton.WithRequestLocation("Share your location with bot"),
                });
            RequestReplyKeyboard.OneTimeKeyboard = true;
            await Bot.Client.SendTextMessageAsync(
                chatId: id,
                text: "Where are you?",
                replyMarkup: RequestReplyKeyboard
            ); ;
        }                
        //Generates the airquality result
        public static string GetText(Forecast_Menu m, long id)
        {
            string text;
            Console.WriteLine($"1{ m.status}1");
            if (m.status == "success")
            {
                text = "Here are the air quality information:\n";
                User u = GetUser(id);
                string country = $"country: {m.data.country}\n";
                string region = $"region: {m.data.state}\n";
                string city = $"city: {m.data.city}\n";
                string temperature = $"temperature: {m.data.current.weather.tp}°C\n";
                string humidity = $"humidity: {m.data.current.weather.hu}%\n";
                string preassure = $"pressure: {m.data.current.weather.pr}mb\n";
                string WD = $"WD: {m.data.current.weather.wd}\n";
                string WS = $"WS: {m.data.current.weather.ws}\n";
                string aqicn = $"aqicn: {m.data.current.pollution.aqicn}\n";
                string aqius = $"aqius: {m.data.current.pollution.aqius}\n";
                string maincn = $"maincn: {m.data.current.pollution.maincn}\n";
                string mainus = $"mainus: {m.data.current.pollution.mainus}\n";
                List<string> temp = new List<string>(12);
                List<bool> tempuser = new List<bool>(12);
                temp.Add(country);
                temp.Add(region);
                temp.Add(city);
                temp.Add(temperature);
                temp.Add(humidity);
                temp.Add(preassure);
                temp.Add(WD);
                temp.Add(WS);
                temp.Add(aqicn);
                temp.Add(aqius);
                temp.Add(maincn);
                temp.Add(mainus);

                tempuser.Add(u.set.country);
                tempuser.Add(u.set.state);
                tempuser.Add(u.set.city);
                tempuser.Add(u.set.tp);
                tempuser.Add(u.set.hu);
                tempuser.Add(u.set.pr);
                tempuser.Add(u.set.wd);
                tempuser.Add(u.set.ws);
                tempuser.Add(u.set.aqicn);
                tempuser.Add(u.set.aqius);
                tempuser.Add(u.set.maincn);
                tempuser.Add(u.set.mainus);
                int i = 0;
                foreach (bool b in tempuser)
                {
                    if (b == true)
                    {
                        text += temp[i];
                    }
                    i++;
                }
            }
            else
                text = "Content is absent, try again later or choose another city";
            return text;
        }                        
        //Nulling method
        public static void Nulling(long id)
        {
            User u = GetUser(id);
            DataDeleter(id, 0);
            u.keyboard_state = 0;
            u.country = null;
            u.state = null;
            u.city = null;
            u.keyboard_number = 0;
        }
        static string help = "Use /airquality to get airquality of the appropriate city (select country, then state and then city)"
                      + "\nuse /settings to tune the display of AQ data"
                      + "\nuse /savedcity to get air quality of a saved city"
                      + "\nuse /location to get air quality of a nearby city,"
                      + "\nyou can also just send location to the bot and get the result"
                      + "\nIn order to use inline query type @aqandnewsbot, then type with space the desired keyword,"
                      + "\nthen type with space category if you want the results to be filtered"
                      + "\nuse /inlinelang to select language for inline queries(default English)";
    }
}