using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Wedding
{
   internal class Program
   {
      //private static string token { get; set; } = "6123367281:AAFrPTXtsRggDpUdP4j-7rzR40YL0FfUGEU";
      private static string token { get; set; } = "6114792400:AAEwH1vpZsH4uHHJFTQ_hPElpjxXQ-UAvFU";
      private static TelegramBotClient client;
      static void Main()
      {
         client = new TelegramBotClient(token);
         client.StartReceiving();
         //SendNotify();
         client.OnMessage += ClientMessage;
         client.OnUpdate += UpdateData;
         client.OnCallbackQuery += (object sc, CallbackQueryEventArgs ev) => {
            InlineButtonOperation(sc, ev);
         };
         Console.ReadLine();
      }

      private static async void SendNotify()
      {
         try {
            Connect.LoadUser(users);
            string request = string.Empty;
            for (int i = 0; i < users.Count; i++) {
               await client.SendTextMessageAsync(users[i].id, "Введите своё имя следующим сообщением для фиксирования ваших ответов");
               request += "update `User` set message = 'waitname' where id = '" + users[i].id + "'; ";
            }
            Connect.Query(request);
         } catch { }
      }

      //static long channel = -1001753358180;
      static long channel = -1001796822827;
      static List<User> users = new List<User>();

      private static async void ClientMessage(object sender, MessageEventArgs e)
      {
         try {
            var message = e.Message;
            try {
               await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId - 1);
            } catch { }
            if (message.Text == "/start") {
               Connect.LoadUser(users);
               var user = users.Find(x => x.id == message.Chat.Id.ToString());
               if (user == null)
                  Connect.Query("insert into `User` (id, message, last, username) values ('" + message.Chat.Id + "', 'waitname', 'no', '" + message.From.Username + "');");
               else
                  Connect.Query("update `User` set message = 'waitname' where id = '" + message.Chat.Id + "';");
               await client.SendTextMessageAsync(message.Chat.Id, "Чтобы мы могли идентифицировать твои ответы, напиши, пожалуйста, свое имя следующим сообщением");
            }
            else if (message.Text == "/menu") {
               Connect.LoadUser(users);
               var user = users.Find(x => x.id == message.Chat.Id.ToString());
               if (user != null && user.last == "yes") {
                  InlineKeyboardMarkup last = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Вишлист", url: "https://mywishboard.com/collection/wedding-wishlist-1144138") }, new[] { InlineKeyboardButton.WithCallbackData("Хочу сюрприз и не боюсь позора", "MakeSurprise") }, new[] { InlineKeyboardButton.WithCallbackData("Заказать свою песню", "MusicGov") }, new[] { InlineKeyboardButton.WithCallbackData("Оставить предпочтения по меню", "FavFood") }, new[] { InlineKeyboardButton.WithCallbackData("Саммери", "Sammeri") }, new[] { InlineKeyboardButton.WithCallbackData("Техника безопасности", "Safety") }, new[] { InlineKeyboardButton.WithCallbackData("FAQ", "FAQ") }, new[] { InlineKeyboardButton.WithCallbackData("Кружочек молодоженам на будущее", "VideoNote") } });
                  await client.SendTextMessageAsync(message.Chat.Id, "📜 Меню", replyMarkup: last);
               }
               else
                  await client.SendTextMessageAsync(message.Chat.Id, "Для доступа к меню бота пройдите опрос");
            }
            else {
               Connect.LoadUser(users);
               var user = users.Find(x => x.id == message.Chat.Id.ToString());
               if (user != null) {
                  if (user.message == "whatloss" && message.Type != Telegram.Bot.Types.Enums.MessageType.VideoNote)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitmusic" && message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitmusic" && message.Type == Telegram.Bot.Types.Enums.MessageType.Text) {
                     Connect.Query("update `User` set message = 'waitwish', music = '" + message.Text + "' where id = '" + message.Chat.Id + "';");
                     await client.SendTextMessageAsync(message.Chat.Id, "И какое послание ты хочешь, чтобы диджей передал");
                  }
                  else if (user.message == "waitwish" && message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitwish" && message.Type == Telegram.Bot.Types.Enums.MessageType.Text) {
                     Connect.LoadUser(users);
                     var userWish = users.Find(x => x.id == message.Chat.Id.ToString());
                     Connect.Query("update `User` set music = '" + userWish.music + " (Пожелание: " + message.Text + ")', message = 'none' where id = '" + message.Chat.Id + "';");
                     await client.SendTextMessageAsync(message.Chat.Id, "Твоё предложение учтено 😉");
                  }
                  else if (user.message == "waitnote" && message.Type != Telegram.Bot.Types.Enums.MessageType.VideoNote)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitnote_1" && message.Type != Telegram.Bot.Types.Enums.MessageType.VideoNote)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitnote_2" && message.Type != Telegram.Bot.Types.Enums.MessageType.VideoNote)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitnote_3" && message.Type != Telegram.Bot.Types.Enums.MessageType.VideoNote)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitnote_4" && message.Type != Telegram.Bot.Types.Enums.MessageType.VideoNote)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitnote_5" && message.Type != Telegram.Bot.Types.Enums.MessageType.VideoNote)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitnote_6" && message.Type != Telegram.Bot.Types.Enums.MessageType.VideoNote)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitexeption" && message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitexeption" && message.Type == Telegram.Bot.Types.Enums.MessageType.Text) {
                     try {
                        await client.DeleteMessageAsync(message.Chat.Id, message.MessageId - 1);
                     } catch { }
                     Connect.Query("update `User` set exeption = '" + message.Text + "' where id = '" + message.Chat.Id + "';");
                     AllFood(message);
                  }
                  else if (user.message == "waitname" && message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  else if (user.message == "waitname" && message.Type == Telegram.Bot.Types.Enums.MessageType.Text) {
                     Connect.Query("update `User` set username = '" + message.Text + "', message = 'none' where id = '" + message.Chat.Id + "';");
                     InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("✅ Да", "YesVisit") }, new[] { InlineKeyboardButton.WithCallbackData("⛔️ Нет", "NoVisit") } });
                     await client.SendTextMessageAsync(message.Chat.Id, "Привет " + message.Text + "!\n\n1 апреля традиционно считается Днём смеха. Но несмотря на это, новость о нашей свадьбе не является шуткой 😁 Поэтому мы с удовольствием приглашаем тебя на наш праздник любви, смеха и безумия.\n\nПридешь ли ты к нам на свадьбу 1 апреля?", replyMarkup: keyboard);
                  }
               }
            }
         } catch { }
      }

      static InlineKeyboardMarkup cancel = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("⛔️ Отмена", "Cancel") } });
      private static async void InlineButtonOperation(object sc, CallbackQueryEventArgs ev)
      {
         try {
            var message = ev.CallbackQuery.Message;
            var data = ev.CallbackQuery.Data;
            if (data != "1Text" && data != "Minion" && data != "Duck" && data != "Tunec" && data != "Losos" && data != "Pasta" && data != "Prosecco" && data != "Aperol" && data != "Coctails" && data != "PlayDrink" && data != "WhiteVine" && data != "RedVine" && data != "NoAlko" && data != "Strong") {
               try {
                  await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId, replyMarkup: null);
               } catch { }
            }
            if (data == "NoVisit") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               await client.SendTextMessageAsync(message.Chat.Id, "Очень жаль, что у тебя не получится прийти, но ты можешь записать круглешок молодоженам и гостям 😉");
               await Task.Delay(200);
               await client.SendTextMessageAsync(message.Chat.Id, "Что само обидное в том, что ты не сможешь быть на свадьбе?\n\n*Отправь следующее сообщение кружочком*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
               Connect.Query("update `User` set message = 'whatloss' where id = '" + message.Chat.Id + "';");
            }
            else if (data == "YesVisit") {
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("All inclusive 💍🥂", "One|Time") }, new[] { InlineKeyboardButton.WithCallbackData("Только бар 💃🏽", "Two|Time") }, new[] { InlineKeyboardButton.WithCallbackData("Только ЗАГС 🥲", "Three|Time") }, new[] { InlineKeyboardButton.WithCallbackData("Назад", "BackToStart") } });
               await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "Отлично, на какую часть именно?\n\n1. И на роспись в ЗАГС, и на вечеринку в бар - я, знаете ли, беру от жизни все!\n\n2. Только на вечеринку в бар - умею правильно расставлять приоритеты.\n\n3. Только на роспись в ЗАГС, у меня очень серьезное оправдание😁", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: keyboard);
            }
            else if (data == "BackToStart") {
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("✅ Да", "YesVisit") }, new[] { InlineKeyboardButton.WithCallbackData("⛔️ Нет", "NoVisit") } });
               await client.EditMessageTextAsync(message.Chat.Id, message.MessageId, "1 апреля традиционно считается Днём смеха. Но несмотря на это, новость о нашей свадьбе не является шуткой 😁 Поэтому мы с удовольствием приглашаем тебя на наш праздник любви, смеха и безумия.\n\nПридешь ли ты к нам на свадьбу 1 апреля?", replyMarkup: keyboard);
            }
            else if (data.Contains("|Time")) {
               if (data.Contains("One") || data.Contains("Two") || data.Contains("Three")) {
                  try {
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  } catch { }
                  if (data.Contains("One")) {
                     Connect.Query("update `User` set visit = 'ЗАГС + Бар' where id = '" + message.Chat.Id + "';");
                     InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Открыть в картах", "https://yandex.ru/maps/org/175522552121") }, new[] { InlineKeyboardButton.WithCallbackData("Далее", "1Text") } });
                     await client.SendTextMessageAsync(message.Chat.Id, "Отлично, ждем тебя в ЗАГСе Birds на 84 этаже к 15:30, после росписи поедм Prosecco Bar на Сретенке, 7", replyMarkup: keyboard);
                     await Task.Delay(200);
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  }
                  else if (data.Contains("Two")) {
                     Connect.Query("update `User` set visit = 'Бар' where id = '" + message.Chat.Id + "';");
                     InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Открыть в картах", "https://yandex.ru/maps/org/36342458151") }, new[] { InlineKeyboardButton.WithCallbackData("Далее", "11Text") } });
                     await client.SendTextMessageAsync(message.Chat.Id, "Супер, тогда приезжай сразу в Prosecco Bar на Сретенке, 7 к 16:30", replyMarkup: keyboard);
                     await Task.Delay(200);
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  }
                  else if (data.Contains("Three")) {
                     Connect.Query("update `User` set visit = 'ЗАГС' where id = '" + message.Chat.Id + "';");
                     InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Открыть в картах", "https://yandex.ru/maps/org/175522552121") }, new[] { InlineKeyboardButton.WithCallbackData("Далее", "1Text") } });
                     await client.SendTextMessageAsync(message.Chat.Id, "Очень жаль, что ты не сможешь быть с нами весь день, но мы будем рады видеть тебя в ЗАГСе Birds к 15:30", replyMarkup: keyboard);
                     await Task.Delay(200);
                     await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
                  }
               }
            }
            else if (data == "Sammeri") {
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("ЗАГС в Birds", "https://yandex.ru/maps/org/175522552121") }, new[] { InlineKeyboardButton.WithUrl("Prosecco Bar PR11", "https://yandex.ru/maps/org/36342458151") }, new[] { InlineKeyboardButton.WithUrl("Вишлист 💝", url: "https://mywishboard.com/collection/wedding-wishlist-1144138") }, new[] { InlineKeyboardButton.WithUrl("Дресс-код для девочек 🐆", url: "https://ru.pinterest.com/chizhuliya/girls-dress-code-moodboard/?invite_code=ec74c929c3a24755bba643f72eacc6e7&sender=375558193834645871") }, new[] { InlineKeyboardButton.WithUrl("Дресс-код для мальчиков 🦣", url: "https://ru.pinterest.com/chizhuliya/boys-dress-code-moodboard/?invite_code=2e3ccf3a119648fdbe6efa8b76352032&sender=375558193834645871") }, new[] { InlineKeyboardButton.WithUrl("Написать Владу", url: "https://t.me/vladislav_sapunov") }, new[] { InlineKeyboardButton.WithUrl("Написать Даше", url: "https://t.me/dadaborik") }, new[] { InlineKeyboardButton.WithUrl("Написать Артуру", url: "https://t.me/ArthurCarter") } });
               await client.SendTextMessageAsync(message.Chat.Id, "Свадьба проходит в Москве 1 апреля🥂\n\nСбор в ЗАГСе в Birds на 84 этаже в 15:30 💍\n\nЧеловек, который встретит и поможет в случае чего - Яна +7 (977) 681-17-41\n\nДля тех, кто сразу едет в Prosecco Bar PR11 время сбора 16:30 💃🏽\n\nЧеловек, который встретит и решит любой вопрос - Ася +7 (977) 681-17-41\n\nЕсли есть сюрпризы, творческие заготовки итд пиши скорее Владу 🐒\n\nПо всем другим вопросам пиши нашим организаторам Даше и Артуру 🐳", replyMarkup: keyboard);
            }
            else if (data == "1Text") {
               try {
                  InlineKeyboardMarkup map = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Открыть в картах", "https://yandex.ru/maps/org/175522552121") } });
                  await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId, replyMarkup: map);
               } catch { }
               await client.SendTextMessageAsync(message.Chat.Id, "С таймингом опредлились, давай разберемся с концепцией мероприятия\n\nЧто тебе нужно знать о нем, чтобы подготовиться и взять от этого дня максимум эмоций и впечатлений - три простых вещи:");
               await Task.Delay(500);
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Далее", "2Text") } });
               await client.SendTextMessageAsync(message.Chat.Id, "1. Мы хотим разделить радость этого дня в компании самых близких людей, поэтому будем рады со-творчеству с твоей стороны. Ведь еще в школьные времена повелось, что если не сказал никому, что у него белая спина, то считай день прошел зря. Наша свадьба - не исключение. У тебя будет возможность попробовать себя в роли властной (или не очень?) тамады. Для этого выбери желаемую активность, которую ты хочешь провести на нашем празднике. Бот в телеграмме тебе в этом поможет чуть позже. Количество мест ограничено 🌝", replyMarkup: keyboard);
            }
            else if (data == "11Text") {
               try {
                  InlineKeyboardMarkup map = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Открыть в картах", "https://yandex.ru/maps/org/36342458151") } });
                  await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId, replyMarkup: map);
               } catch { }
               await client.SendTextMessageAsync(message.Chat.Id, "С таймингом опредлились, давай разберемся с концепцией мероприятия\n\nЧто тебе нужно знать о нем, чтобы подготовиться и взять от этого дня максимум эмоций и впечатлений - три простых вещи:");
               await Task.Delay(500);
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Далее", "2Text") } });
               await client.SendTextMessageAsync(message.Chat.Id, "1. Мы хотим разделить радость этого дня в компании самых близких людей, поэтому будем рады со-творчеству с твоей стороны. Ведь еще в школьные времена повелось, что если не сказал никому, что у него белая спина, то считай день прошел зря. Наша свадьба - не исключение. У тебя будет возможность попробовать себя в роли властной (или не очень?) тамады. Для этого выбери желаемую активность, которую ты хочешь провести на нашем празднике. Бот в телеграмме тебе в этом поможет чуть позже. Количество мест ограничено 🌝", replyMarkup: keyboard);
            }
            else if (data == "2Text") {
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Референсы для девочек", url: "https://ru.pinterest.com/chizhuliya/girls-dress-code-moodboard/?invite_code=ec74c929c3a24755bba643f72eacc6e7&sender=375558193834645871") }, new[] { InlineKeyboardButton.WithUrl("Референсы для мальчиков", url: "https://ru.pinterest.com/chizhuliya/boys-dress-code-moodboard/?invite_code=2e3ccf3a119648fdbe6efa8b76352032&sender=375558193834645871") }, new[] { InlineKeyboardButton.WithCallbackData("Далее", "3Text") } });
               await client.SendTextMessageAsync(message.Chat.Id, "2. Все-таки это свадьба, поэтому у нее будет стилистика. По-английски это звучит как «Comfort chic». Камфарт шикъ - это что-то удобное, в чем тебе будет приятно (и морально, и физически) в течение 8-12 часов. При этом по 10-балльной шкале нарядности это будет ближе к 7-8 баллам. То есть вроде бы солидно и торжественно, но при этом и не скажешь, что ты все 2 недели ломал голову. Для мужчин - это что-то среднее между Дикаприо в Гэтсби и в Выжившем. Для девочек хорошим ориентиром будет Эмили в Париже на пару градусов потише🥖. Ах, и да - конкретной цветовой гаммы нет, потому что это КАМФАРТ шикъ. Enjoy!", replyMarkup: keyboard);

            }
            else if (data == "3Text") {
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Далее", "4Text") } });
               await client.SendTextMessageAsync(message.Chat.Id, "3. Праздновать будем в баре, потому будь готов ко всему - большому количеству коктейлей, музыки и активностей. И даже, возможно, к танцам на барной стойке, потому что кроме нас, там никого не будет, а значит позориться не перед кем, все свои.", replyMarkup: keyboard);
            }
            else if (data == "4Text") {
               InlineKeyboardMarkup last = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Вишлист", url: "https://mywishboard.com/collection/wedding-wishlist-1144138") }, new[] { InlineKeyboardButton.WithCallbackData("Хочу сюрприз и не боюсь позора", "MakeSurprise") }, new[] { InlineKeyboardButton.WithCallbackData("Заказать свою песню", "MusicGov") }, new[] { InlineKeyboardButton.WithCallbackData("Оставить предпочтения по меню", "FavFood") }, new[] { InlineKeyboardButton.WithCallbackData("Саммери", "Sammeri") }, new[] { InlineKeyboardButton.WithCallbackData("Техника безопасности", "Safety") }, new[] { InlineKeyboardButton.WithCallbackData("FAQ", "FAQ") }, new[] { InlineKeyboardButton.WithCallbackData("Кружочек молодоженам на будущее", "VideoNote") } });
               await client.SendTextMessageAsync(message.Chat.Id, "Поздравляем, теперь ты владеешь всеми базовыми навыками комфортного выживания на нашей свадьбе - следущий уровень для супер-мотивированных\n\n*Данное меню можно вызвать командой /menu*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: last);
               Connect.Query("update `User` set last = 'yes' where id = '" + message.Chat.Id + "';");
            }
            else if (data.Contains("Four")) {
               await client.SendTextMessageAsync(message.Chat.Id, "Первая часть начинается в 15:30 в ЗАГСе Birds - башня Око. После росписи мы отправляемся на вечернюю часть в Prosecco Bar PR11 на Сретенке, 7 и там уже говорится, пока силы не покинут нас :)");
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("1", "One") }, new[] { InlineKeyboardButton.WithCallbackData("2", "Two") }, new[] { InlineKeyboardButton.WithCallbackData("3", "Three") }, new[] { InlineKeyboardButton.WithCallbackData("4", "Four") } });
               await Task.Delay(200);
               await client.SendTextMessageAsync(message.Chat.Id, "Отлично, на какую часть именно?\n\n1. И на роспись в ЗАГС, и на вечеринку в бар - я, знаете ли, беру от жизни все!\n\n2. Только на вечеринку в бар - умею правильно расставлять приоритеты.\n\n3. Только на роспись в ЗАГС, у меня очень серьезное оправдание😁\n\n4. Огласите весь список, такое сложное решение нужно принимать взвешенно.\n\n*Выбери цифру подходяющую для тебя*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: keyboard);
               await Task.Delay(200);
               await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
            }
            else if (data == "MakeSurprise") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Написать Владу", url: "https://t.me/vladislav_sapunov") } });
               await client.SendTextMessageAsync(message.Chat.Id, "Отлично, жизнь любит смелых! Напиши свою идею интерактива нашему ведущему Владу, и он поможет. Если идеи нет, все равно пиши Владу, он получается у нас в ментальных заложниках до 2 апреля :)", replyMarkup: keyboard);
            }
            else if (data == "MusicGov") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               await client.SendTextMessageAsync(message.Chat.Id, "Ооо, привееет ромашки! Напиши какую песню ты хочешь закзать", replyMarkup: cancel);
               Connect.Query("update `User` set message = 'waitmusic' where id = '" + message.Chat.Id + "';");
            }
            else if (data == "FAQ") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               await client.SendTextMessageAsync(message.Chat.Id, "Будет ли кто-то еще в баре? \nНет, мы полностью снимаем бар, поэтому атмосфера будет максимально свободная и комфортная🐒\n\nНужно/можно ли дарить цветы? \nСпасибо, мы очень любим цветы, но в этот день их и так будет оооочень много. Нам будет приятно, если вы переведете деньги в цветочный фонд (смотри вишлист), а мы оформим цветочную подписку с доставкой одного букета в месяца, чтобы растянуть воспоминания об этом дне надолго🌻\n\nОбязательно ли дарить что-то памятное? \nЕсли вдруг вам не приглянулся ни один из материальных подарков из нашего вишлиста, то мы будем очень рады денежному вкладу в наш французский свадебный трип летом💸");
            }
            else if (data == "Safety") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               await client.SendTextMessageAsync(message.Chat.Id, "Для достижения максимального комфорта и ощущения счастья в этот день, непременно выспись, обязательно позаботься об удобной сменной обуви, перед выездом выпей побольше угля и возьми с собой тюбик энтеросгеля.");
            }
            else if (data == "VideoNote") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               await client.SendTextMessageAsync(message.Chat.Id, "Передай пожелание жениху и невесте в будущее через 10 лет брака?\n\n*Отправь кружочек следующим сообщением*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: cancel);
               Connect.Query("update `User` set message = 'waitnote_1' where id = '" + message.Chat.Id + "';");
            }
            else if (data == "Cancel") {
               Connect.Query("update `User` set message = 'none' where id = '" + message.Chat.Id + "';");
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
            }
            else if (data == "FavFood") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData("Все ок, я всеядный 😄", "AllFood") }, new[] { InlineKeyboardButton.WithCallbackData("⛔️ Отмена", "Cancel") } });
               await client.SendTextMessageAsync(message.Chat.Id, "Напиши, какие продукты и ингредиенты ты не хочешь видеть в своей тарелке следующим сообщением", replyMarkup: keyboard);
               Connect.Query("update `User` set message = 'waitexeption' where id = '" + message.Chat.Id + "';");
            }
            else if (data == "AllFood") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               AllFood(message);
            }
            else if (data == "Prosecco")
               SendDrink(message, "prosecco");
            else if (data == "Aperol")
               SendDrink(message, "aperol");
            else if (data == "Coctails")
               SendDrink(message, "coctails");
            else if (data == "PlayDrink")
               SendDrink(message, "playing");
            else if (data == "WhiteVine")
               SendDrink(message, "whitevine");
            else if (data == "RedVine")
               SendDrink(message, "redvine");
            else if (data == "Strong")
               SendDrink(message, "strong");
            else if (data == "NoAlko")
               SendDrink(message, "noalko");
            else if (data == "CompleteDrink") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               Connect.LoadUser(users);
               var user = users.Find(x => x.id == message.Chat.Id.ToString());
               string[] list = GetFood(user);
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData(list[0], "Minion") }, new[] { InlineKeyboardButton.WithCallbackData(list[1], "Duck") }, new[] { InlineKeyboardButton.WithCallbackData(list[2], "Tunec") }, new[] { InlineKeyboardButton.WithCallbackData(list[3], "Losos") }, new[] { InlineKeyboardButton.WithCallbackData(list[4], "Pasta") }, new[] { InlineKeyboardButton.WithCallbackData("Готово", "CompleteFood") } });
               await client.SendTextMessageAsync(message.Chat.Id, "Какие предпочтения по горячему у тебя есть?\n\n*Можно выбрать несколько вариантов", replyMarkup: keyboard);
            }
            else if (data == "Minion")
               SendFood(message, "minion");
            else if (data == "Duck")
               SendFood(message, "duck");
            else if (data == "Tunec")
               SendFood(message, "tunec");
            else if (data == "Losos")
               SendFood(message, "losos");
            else if (data == "Pasta")
               SendFood(message, "pasta");
            else if (data == "CompleteFood") {
               try {
                  await client.DeleteMessageAsync(message.Chat.Id, message.MessageId);
               } catch { }
               await client.SendTextMessageAsync(message.Chat.Id, "Твои пожелания учтены 😉");
            }
         } catch { }
      }

      private static async void AllFood(Telegram.Bot.Types.Message message)
      {
         try {
            Connect.Query("update `User` set message = 'none' where id = '" + message.Chat.Id + "';");
            Connect.LoadUser(users);
            var user = users.Find(x => x.id == message.Chat.Id.ToString());
            if (user != null) {
               string[] list = GetDrink(user);
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData(list[0], "Prosecco") }, new[] { InlineKeyboardButton.WithCallbackData(list[1], "Aperol") }, new[] { InlineKeyboardButton.WithCallbackData(list[2], "Coctails") }, new[] { InlineKeyboardButton.WithCallbackData(list[3], "PlayDrink") }, new[] { InlineKeyboardButton.WithCallbackData(list[4], "WhiteVine") }, new[] { InlineKeyboardButton.WithCallbackData(list[5], "RedVine") }, new[] { InlineKeyboardButton.WithCallbackData(list[6], "Strong") }, new[] { InlineKeyboardButton.WithCallbackData(list[7], "NoAlko") }, new[] { InlineKeyboardButton.WithCallbackData("Готово", "CompleteDrink") } });
               await client.SendTextMessageAsync(message.Chat.Id, "Какие напитки ты планируешь пить?\n\n*Можно выбрать несколько вариантов", replyMarkup: keyboard);
            }
         } catch { }
      }

      private static async void SendFood(Telegram.Bot.Types.Message message, string food)
      {
         try {
            Connect.LoadUser(users);
            var user = users.Find(x => x.id == message.Chat.Id.ToString());
            string foods = string.Empty;
            if (user.fav_food.Contains(food)) {
               for (int i = 0; i < user.fav_food.Split('|').Length; i++)
                  if (user.fav_food.Split('|')[i] != food)
                     foods += user.fav_food.Split('|')[i] + "|";
            }
            else foods = user.fav_food + "|" + food;
            foods = foods.Trim('|');
            Connect.Query("update `User` set fav_food = '" + foods + "' where id = '" + message.Chat.Id + "';");
            Connect.LoadUser(users);
            user = null;
            user = users.Find(x => x.id == message.Chat.Id.ToString());
            if (user != null) {
               string[] list = GetFood(user);
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData(list[0], "Minion") }, new[] { InlineKeyboardButton.WithCallbackData(list[1], "Duck") }, new[] { InlineKeyboardButton.WithCallbackData(list[2], "Tunec") }, new[] { InlineKeyboardButton.WithCallbackData(list[3], "Losos") }, new[] { InlineKeyboardButton.WithCallbackData(list[4], "Pasta") }, new[] { InlineKeyboardButton.WithCallbackData("Готово", "CompleteFood") } });
               try {
                  await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId, replyMarkup: keyboard);
               } catch { await client.SendTextMessageAsync(message.Chat.Id, "Какие предпочтения по горячему у тебя есть?\n\n*Можно выбрать несколько вариантов", replyMarkup: keyboard); }
            }
         } catch { }
      }

      private static async void SendDrink(Telegram.Bot.Types.Message message, string drink)
      {
         try {
            Connect.LoadUser(users);
            var user = users.Find(x => x.id == message.Chat.Id.ToString());
            string drinks = string.Empty;
            if (user.fav_drink.Contains(drink)) {
               for (int i = 0; i < user.fav_drink.Split('|').Length; i++)
                  if (user.fav_drink.Split('|')[i] != drink)
                     drinks += user.fav_drink.Split('|')[i] + "|";
            }
            else drinks = user.fav_drink + "|" + drink;
            drinks = drinks.Trim('|');
            Connect.Query("update `User` set fav_drink = '" + drinks + "' where id = '" + message.Chat.Id + "';");
            Connect.LoadUser(users);
            user = null;
            user = users.Find(x => x.id == message.Chat.Id.ToString());
            if (user != null) {
               string[] list = GetDrink(user);
               InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithCallbackData(list[0], "Prosecco") }, new[] { InlineKeyboardButton.WithCallbackData(list[1], "Aperol") }, new[] { InlineKeyboardButton.WithCallbackData(list[2], "Coctails") }, new[] { InlineKeyboardButton.WithCallbackData(list[3], "PlayDrink") }, new[] { InlineKeyboardButton.WithCallbackData(list[4], "WhiteVine") }, new[] { InlineKeyboardButton.WithCallbackData(list[5], "RedVine") }, new[] { InlineKeyboardButton.WithCallbackData(list[6], "Strong") }, new[] { InlineKeyboardButton.WithCallbackData(list[7], "NoAlko") }, new[] { InlineKeyboardButton.WithCallbackData("Готово", "CompleteDrink") } });
               try {
                  await client.EditMessageReplyMarkupAsync(message.Chat.Id, message.MessageId, replyMarkup: keyboard);
               } catch { await client.SendTextMessageAsync(message.Chat.Id, "Какие напитки ты планируешь пить?\n\n*Можно выбрать несколько вариантов", replyMarkup: keyboard); }
            }
         } catch { }
      }

      private static string[] GetFood(User user)
      {
         try {
            string[] list = { "Филе миньон", "Утиная ножка", "Тунец", "Лосось", "Паста" };
            if (user.fav_food.Contains("minion")) list[0] = "✅ Филе миньон";
            if (user.fav_food.Contains("duck")) list[1] = "✅ Утиная ножка";
            if (user.fav_food.Contains("tunec")) list[2] = "✅ Тунец";
            if (user.fav_food.Contains("losos")) list[3] = "✅ Лосось";
            if (user.fav_food.Contains("pasta")) list[4] = "✅ Паста";
            return list;
         } catch { return null; }
      }

      private static string[] GetDrink(User user)
      {
         try {
            string[] list = { "Prosecco", "Aperol", "Coctails", "Игристое", "Белое вино", "Красное вино", "Крепкие напитки", "Безалкогольные напитки" };
            if (user.fav_drink.Contains("prosecco")) list[0] = "✅ Prosecco";
            if (user.fav_drink.Contains("aperol")) list[1] = "✅ Aperol";
            if (user.fav_drink.Contains("playing")) list[3] = "✅ Игристое";
            if (user.fav_drink.Contains("coctails")) list[2] = "✅ Coctails";
            if (user.fav_drink.Contains("whitevine")) list[4] = "✅ Белое вино";
            if (user.fav_drink.Contains("redvine")) list[5] = "✅ Красное вино";
            if (user.fav_drink.Contains("strong")) list[6] = "✅ Крепкие напитки";
            if (user.fav_drink.Contains("noalko")) list[7] = "✅ Безалкогольные напитки";
            return list;
         } catch { return null; }
      }

      private static string GetEat(string str)
      {
         try {
            string result = string.Empty;
            if (str.Length > 0) {
               if (str.Contains("minion")) result += "Филе миньон, ";
               if (str.Contains("duck")) result += "Утиная ножка, ";
               if (str.Contains("tunec")) result += "Тунец, ";
               if (str.Contains("losos")) result += "Лосось, ";
               if (str.Contains("pasta")) result += "Паста, ";
               if (str.Contains("prosecco")) result += "Prosecco, ";
               if (str.Contains("aperol")) result += "Aperol, ";
               if (str.Contains("coctails")) result += "Coctails, ";
               if (str.Contains("playing")) result += "Игристое, ";
               if (str.Contains("whitevine")) result += "Белое вино, ";
               if (str.Contains("redvine")) result += "Красное вино, ";
               if (str.Contains("string")) result += "Крепкие напитки, ";
               if (str.Contains("noalko")) result += "Безалкогольные напитки, ";
               result = result.Trim(' ').Trim(',');
            }
            else result = "Не указано";
            return result;
         } catch { return null; }
      }

      private static async void UpdateData(object sender, UpdateEventArgs e)
      {
         try {
            var update = e.Update;
            Connect.LoadUser(users);
            try {
               if (update.Type == Telegram.Bot.Types.Enums.UpdateType.ChannelPost && update.ChannelPost.Chat.Id == channel) {
                  if (update.ChannelPost.Text == "/excel") {
                     try {
                        File.Delete(Path.GetFullPath("data.csv"));
                     } catch { }
                     string file = "Пользователь;Присутствие;Песни;Исключительные ингредиенты;Напитки;Горячее\n";
                     for (int i = 0; i < users.Count; i++) {
                        if (users[i].id != "885185553") {
                           string food = GetEat(users[i].fav_food), drink = GetEat(users[i].fav_drink);
                           file += users[i].username + " (id: " + users[i].id + ");" + users[i].visit + ";" + users[i].music + ";" + users[i].exeption + ";" + drink + ";" + food + "\n";
                        }
                     }
                     File.WriteAllText(Path.GetFullPath("data.csv"), file, Encoding.UTF8);
                     using (var stream = File.OpenRead(Path.GetFullPath("data.csv"))) {
                        InputOnlineFile doc = new InputOnlineFile(stream);
                        doc.FileName = "data.csv";
                        await client.SendDocumentAsync(channel, doc);
                     }
                     File.Delete(Path.GetFullPath("data.csv"));
                  }
                  return;
               }
            } catch { }
            var user = users.Find(x => x.id == update.Message.Chat.Id.ToString());
            if (user != null) {
               if (user.message == "whatloss" && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote) {
                  Connect.Query("update `User` set visit = 'Не приду' where id = '" + update.Message.Chat.Id + "';");
                  await client.SendTextMessageAsync(channel, "(" + user.username + " `" + user.id + "`) Самое обидное, почему не смог присутствовать на свадьбе:", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  await Task.Delay(200);
                  await client.SendVideoNoteAsync(channel, update.Message.VideoNote.FileId);
                  await Task.Delay(200);
                  await client.SendTextMessageAsync(update.Message.Chat.Id, "Супер, а что пожелаешь мололдоженам?\n\n*Отправь следующее сообщение кружочком*", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  Connect.Query("update `User` set message = 'whatwish' where id = '" + update.Message.Chat.Id + "';");
               }
               else if (user.message == "whatwish" && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote) {
                  InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(new[] { new[] { InlineKeyboardButton.WithUrl("Открыть вишлист", url: "https://mywishboard.com/collection/wedding-wishlist-1144138") } });
                  await client.SendTextMessageAsync(channel, "(" + user.username + " `" + user.id + "`) Пожаления молодоженам: ", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  await Task.Delay(200);
                  await client.SendVideoNoteAsync(channel, update.Message.VideoNote.FileId);
                  await Task.Delay(200);
                  await client.SendTextMessageAsync(update.Message.Chat.Id, "Ну и раз ты такой хороший друг, то ты можешь порадовать гостей подарком. Вишлист по кнопке ниже - с молодожен фото-благодарность для тебя с подарком)", replyMarkup: keyboard);
                  Connect.Query("update `User` set message = 'none' where id = '" + update.Message.Chat.Id + "';");
               }
               else if (user.message == "waitnote" && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote) {
                  await client.SendTextMessageAsync(channel, "(" + user.username + " `" + user.id + "`) Пожаления молодоженам: ", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  await Task.Delay(200);
                  await client.SendVideoNoteAsync(channel, update.Message.VideoNote.FileId);
                  await Task.Delay(200);
                  await client.SendTextMessageAsync(update.Message.Chat.Id, "Спасибо! Твоё пожелание посмотрим прямо на свадьбе 🙃");
                  Connect.Query("update `User` set message = 'none' where id = '" + update.Message.Chat.Id + "';");
               }
               else if (user.message == "waitnote_1" && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote) {
                  await client.SendTextMessageAsync(channel, "(" + user.username + " `" + user.id + "`) Пожелание жениху и невесте в будущее через 10 лет брака: ", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  await Task.Delay(200);
                  await client.SendVideoNoteAsync(channel, update.Message.VideoNote.FileId);
                  await client.SendTextMessageAsync(update.Message.Chat.Id, "Как ты думаешь будет выглядеть утро молодоженов спустя 50 лет?\n\n*Отправь кружочек следующим сообщением*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: cancel);
                  Connect.Query("update `User` set message = 'waitnote_2' where id = '" + update.Message.Chat.Id + "';");
               }
               else if (user.message == "waitnote_2" && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote) {
                  await client.SendTextMessageAsync(channel, "(" + user.username + " `" + user.id + "`) Как ты думаешь будет выглядеть утро молодоженов спустя 50 лет: ", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  await Task.Delay(200);
                  await client.SendVideoNoteAsync(channel, update.Message.VideoNote.FileId);
                  await client.SendTextMessageAsync(update.Message.Chat.Id, "Какой самый дурацкий подарок можно подарить на свадьбу?\n\n*Отправь кружочек следующим сообщением*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: cancel);
                  Connect.Query("update `User` set message = 'waitnote_3' where id = '" + update.Message.Chat.Id + "';");
               }
               else if (user.message == "waitnote_3" && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote) {
                  await client.SendTextMessageAsync(channel, "(" + user.username + " `" + user.id + "`) Какой самый дурацкий подарок можно подарить на свадьбу: ", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  await Task.Delay(200);
                  await client.SendVideoNoteAsync(channel, update.Message.VideoNote.FileId);
                  await client.SendTextMessageAsync(update.Message.Chat.Id, "А как выглядит идеальный свадебный подарок?\n\n*Отправь кружочек следующим сообщением*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: cancel);
                  Connect.Query("update `User` set message = 'waitnote_4' where id = '" + update.Message.Chat.Id + "';");
               }
               else if (user.message == "waitnote_4" && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote) {
                  await client.SendTextMessageAsync(channel, "(" + user.username + " `" + user.id + "`) Как выглядит идеальный свадебный подарок: ", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  await Task.Delay(200);
                  await client.SendVideoNoteAsync(channel, update.Message.VideoNote.FileId);
                  await client.SendTextMessageAsync(update.Message.Chat.Id, "Опиши самый дурацкий конкурс, который ты встречал на свадьбах?\n\n*Отправь кружочек следующим сообщением*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: cancel);
                  Connect.Query("update `User` set message = 'waitnote_5' where id = '" + update.Message.Chat.Id + "';");
               }
               else if (user.message == "waitnote_5" && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote) {
                  await client.SendTextMessageAsync(channel, "(" + user.username + " `" + user.id + "`) Самый дурацкий конкурс, который ты встречал на свадьбах: ", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  await Task.Delay(200);
                  await client.SendVideoNoteAsync(channel, update.Message.VideoNote.FileId);
                  await client.SendTextMessageAsync(update.Message.Chat.Id, "Любой другой кружок, который ты хочешь записать молодоженам 😊\n\n*Отправь кружочек следующим сообщением*", Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: cancel);
                  Connect.Query("update `User` set message = 'waitnote_6' where id = '" + update.Message.Chat.Id + "';");
               }
               else if (user.message == "waitnote_6" && update.Message.Type == Telegram.Bot.Types.Enums.MessageType.VideoNote) {
                  await client.SendTextMessageAsync(channel, "(" + user.username + " `" + user.id + "`) Любой другой кружок, который хочется записать молодоженам: ", Telegram.Bot.Types.Enums.ParseMode.Markdown);
                  await Task.Delay(200);
                  await client.SendVideoNoteAsync(channel, update.Message.VideoNote.FileId);
                  await client.SendTextMessageAsync(update.Message.Chat.Id, "Спасибо🤗 Твои кружки посмотрим на свадьбе!");
                  Connect.Query("update `User` set message = 'none' where id = '" + update.Message.Chat.Id + "';");
               }
            }
         } catch { }
      }
   }
}
