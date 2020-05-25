
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace telegrambot
{
    public class Keyboard
    {
        private static TBot Bot = new TBot();

        //Method that sends an inline  weather keyboard 
        public static async Task SendLocalityKeyboard(long chat_id)
        {
            try
            {
                User u = Controller.GetUser(chat_id);
                int Length = 0;
                if (Controller.VariableChecker(chat_id) == 0)
                {
                    Length = u.countries_container.Count;
                }
                if (Controller.VariableChecker(chat_id) == 1)
                {
                    Length = u.states_container.Count;
                }
                if (Controller.VariableChecker(chat_id) == 2)
                {
                    Length = u.cities_container.Count;
                }
                string text = null;
                for (int i = u.keyboard_number * 9; i < u.keyboard_number * 9 + 9; i += 9)
                {
                    string n = null;
                    string[] temp = new string[9];
                    string next = "==>";
                    string previous = "<==";
                    if (u.keyboard_number == 0)
                        previous = null;
                    if (u.keyboard_number * 9 + 9 >= Length)
                        next = null;
                    for (int j = 0; j < 9; j++)
                    {
                        try
                        {
                            if (Controller.VariableChecker(chat_id) == 0)
                            {
                                temp[j] = u.countries_container[i + j];
                                n = "11";
                                text = "Choose one country↓";
                            }
                            if (Controller.VariableChecker(chat_id) == 1)
                            {
                                temp[j] = u.states_container[i + j];
                                n = "12";
                                text = $"{u.country} states↓";
                            }
                            if (Controller.VariableChecker(chat_id) == 2)
                            {
                                temp[j] = u.cities_container[i + j];
                                n = "13";
                                text = $"country: {u.country}, state: {u.state}, cities↓";
                            }
                        }
                        catch
                        {
                            temp[j] = null;
                        }
                    }
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                {                        
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"{temp[0]}", $"{n}{temp[0]}"),
                        InlineKeyboardButton.WithCallbackData($"{temp[1]}", $"{n}{temp[1]}"),
                        InlineKeyboardButton.WithCallbackData($"{temp[2]}", $"{n}{temp[2]}"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"{temp[3]}", $"{n}{temp[3]}"),
                        InlineKeyboardButton.WithCallbackData($"{temp[4]}", $"{n}{temp[4]}"),
                        InlineKeyboardButton.WithCallbackData($"{temp[5]}", $"{n}{temp[5]}"),
                    },
                    // third row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"{temp[6]}", $"{n}{temp[6]}"),
                        InlineKeyboardButton.WithCallbackData($"{temp[7]}", $"{n}{temp[7]}"),
                        InlineKeyboardButton.WithCallbackData($"{temp[8]}", $"{n}{temp[8]}"),
                } ,
                // fourth row
                new []
                    {
                        InlineKeyboardButton.WithCallbackData($"{previous}", $"{n}1"),
                        InlineKeyboardButton.WithCallbackData($"{next}", $"{n}2"),
                    },});
                    await Bot.Client.EditMessageTextAsync(chatId: chat_id, u.last_id, text, replyMarkup: inlineKeyboard);
                }
            }
            catch { await Bot.Client.SendTextMessageAsync(chatId: chat_id, text: "wait a little and try again"); }
        }
        //sends save button
        public static async Task SendSaveButton(long chat_id)
        {
            var savebutton = new InlineKeyboardMarkup(new[]
            {
                new []
                        {
                            InlineKeyboardButton.WithCallbackData($"save data", $"3save"),
                    },});
            await Bot.Client.SendTextMessageAsync(chatId: chat_id, text: "Press here↓", replyMarkup: savebutton);
            await Bot.Client.SendTextMessageAsync(chatId: chat_id, text: "And you will be able to get AQ information about this city by using /savedcity ?");
        }
        public static async void Shift(long chat_id, int i, int j)
        {
            User u = Controller.GetUser(chat_id);
            if (i == 1)
            {
                u.keyboard_state = 0;
                u.keyboard_number--;
                try
                {
                    await Keyboard.SendLocalityKeyboard(chat_id);
                }
                catch { }
                u.keyboard_state = j;
            }
            if (i == 2)
            {
                u.keyboard_state = 0;
                u.keyboard_number++;
                try
                {
                    await Keyboard.SendLocalityKeyboard(chat_id);
                }
                catch { }
                u.keyboard_state = j;
            }
        }
        //Method that sends an inline  language keyboard
        public static async Task SendLanguageKeyboard(long chat_id)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {                        
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"English", $"4en"),
                        InlineKeyboardButton.WithCallbackData($"Italian", $"4it"),
                        InlineKeyboardButton.WithCallbackData($"French", $"4fr"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"Japanese", $"4ja"),
                        InlineKeyboardButton.WithCallbackData($"German", $"4de"),
                        InlineKeyboardButton.WithCallbackData($"Spanish", $"4es"),
                    }
                    ,});
            await Bot.Client.SendTextMessageAsync(chatId: chat_id, text: "LanguageSettings", replyMarkup: inlineKeyboard);
        }
        //Method that sends an inline  settings keyboard
        public static async Task SendSettingsKeyboard(long chat_id)
        {
            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {                                   
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"country", $"2country"),
                        InlineKeyboardButton.WithCallbackData($"region", $"2region"),
                        InlineKeyboardButton.WithCallbackData($"city", $"2city"),
                        InlineKeyboardButton.WithCallbackData($"temperature", $"2temperature"),
                    },
                    // second row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"humidity", $"2humidity"),
                        InlineKeyboardButton.WithCallbackData($"pressure", $"2pressure"),
                        InlineKeyboardButton.WithCallbackData($"WD", $"2WD"),
                        InlineKeyboardButton.WithCallbackData($"WS", $"2WS"),
                    },
                    // third row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData($"aqicn", $"2aqicn"),
                        InlineKeyboardButton.WithCallbackData($"aqius", $"2aqius"),
                        InlineKeyboardButton.WithCallbackData($"maincn", $"2maincn"),
                        InlineKeyboardButton.WithCallbackData($"mainus", $"2mainus"),
                },
                    // fourth row
            new []
                    {
                        InlineKeyboardButton.WithCallbackData($"default settings", $"2default"),
                },});
            await Bot.Client.SendTextMessageAsync(chatId: chat_id, text: "DataDisplaySettings", replyMarkup: inlineKeyboard);
        }
    }
}
