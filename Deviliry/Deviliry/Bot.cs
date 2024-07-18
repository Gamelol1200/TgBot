using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using System.Collections;
using DataBase;




namespace Deviliry
{
    public class Bot
    {
        private const string _token = "7469164812:AAFxy9q6HQb7V303DpXW0OaPYqGDv2mgJWM";
        private TelegramBotClient _client = new TelegramBotClient(_token);
        string _record = string.Empty;
        public async Task Start()
        {
            using CancellationTokenSource cts = new CancellationTokenSource();
            ReceiverOptions options = new ReceiverOptions()
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery
                }
            };
            _client.StartReceiving(
                updateHandler: OnMessageReceived,
                pollingErrorHandler: OnErrorOccured,
                receiverOptions: options,
                cancellationToken: cts.Token);
            var bot = await _client.GetMeAsync(cancellationToken: cts.Token);
            Console.WriteLine($"bot{bot.Username} запущен\nДля остановки нажмите esc...");
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                cts.Cancel();
            }
        }
        private Task OnErrorOccured(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                 $"Telegram api error {apiRequestException.ErrorCode} {apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }
        private async Task SendPhoto(long chatId, string pathPhoto, string text, IReplyMarkup? markup = null)
        {
            using var photoStream = new FileStream(pathPhoto, FileMode.Open, FileAccess.Read, FileShare.Read);
            var photo = new InputOnlineFile(photoStream);
            await _client.SendPhotoAsync(chatId, photo, caption: text, replyMarkup: markup);
        }
        private async Task OnMessageReceived(ITelegramBotClient client, Update update, CancellationToken token)
        {
            try
            {
                InlineKeyboardMarkup mainMenu = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Заказать еду или напиток",callbackData:"Заказать еду или напиток"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Устроиться на работу",callbackData:"Устроиться на работу")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Проблемы с ботом",callbackData:"Проблемы с ботом")
                    },
                    new[]
                    {
                         InlineKeyboardButton.WithCallbackData(text:"О нас",callbackData:"О нас")
                    }

                });
                InlineKeyboardMarkup aboutUs = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Локация",callbackData:"Локация"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Номер телефона",callbackData:"Номер телефона")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Описание",callbackData:"Описание")
                    },
                    new[]
                    {
                         InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData:"Назад")
                    }
                });
                InlineKeyboardMarkup menu = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Напитки",callbackData:"Напитки"),
                        InlineKeyboardButton.WithCallbackData(text:"Еда",callbackData:"Еда"),
                    }
                });
                InlineKeyboardMarkup drink = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Водичка",callbackData:"Водичка"),
                        InlineKeyboardButton.WithCallbackData(text:"Кокалёка",callbackData:"Кокалёка"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Кофе",callbackData:"Кофе"),
                        InlineKeyboardButton.WithCallbackData(text:"Сок",callbackData:"Сок")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData:"Назад")
                    }

                });
                InlineKeyboardMarkup Eat = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Салатик",callbackData:"Салатик"),
                        InlineKeyboardButton.WithCallbackData(text:"Котлетки",callbackData:"Котлетки"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Бургер",callbackData:"Бургер"),
                        InlineKeyboardButton.WithCallbackData(text:"Картошечка",callbackData:"Картошечка"),
                    },
                    new[]
                    {


                        InlineKeyboardButton.WithCallbackData(text:"Шоколадка",callbackData:"Шоколадка"),
                        InlineKeyboardButton.WithCallbackData(text:"Пицца",callbackData:"Пицца"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Суши",callbackData:"Суши"),
                        InlineKeyboardButton.WithCallbackData(text:"Шаурма",callbackData:"Шаурма"),
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData:"Назад")
                    }

                });


                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            var message = update.Message;
                            if (message?.Text is not { } messageText)
                            {
                                return;
                            }
                            var chatId = message.Chat.Id;
                            Console.WriteLine($"Полученно сообщение в чате {chatId}:{messageText}");
                            switch (messageText)
                            {
                                case "/start":
                                    {
                                        await client.SendTextMessageAsync(chatId,
                                            "Добро пожаловать в  Deviliry Club!\nВведите аддрес(Начиная с /address)");

                                        return;
                                    }

                            }
                            if (messageText.Contains("/address"))
                            {
                                using (ApplicationContext db = new ApplicationContext())
                                {
                                    var userId = update.Message.From.Id;
                                    if (!db.Users.Any(x => x.IdTelegram == userId))
                                    {
                                        DataBase.User user = new DataBase.User
                                        {
                                            Name = update.Message.From.FirstName,
                                            IdTelegram = userId,
                                            Address = messageText[9..]
                                        };
                                        db.Users.Add(user);
                                        
                                    }
                                    else
                                    {
                                        db.Users.FirstOrDefault(x=> x.IdTelegram == userId).Address = messageText[9..];
                                    }
                                    db.SaveChanges();
                                }
                                await client.SendTextMessageAsync(chatId,"Аддрес добавлен ,выберите следующие действия",replyMarkup:mainMenu);
                            }
                            return;
                        }
                    case UpdateType.CallbackQuery:
                        {
                            var callbackQuery = update.CallbackQuery;
                            var chatId = callbackQuery.Message.Chat.Id;
                            switch (callbackQuery.Data)
                            {
                                case "Заказать еду и напиток":
                                    {                                       
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendTextMessageAsync(chatId, "Выберите еду", replyMarkup: menu);
                                        return;

                                    }
                                case "Устроиться на работу":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendTextMessageAsync(chatId, "Чтобы устроиться на работу приходите на собеседование к нам в офис или звоните", replyMarkup: mainMenu);
                                        return;
                                    }
                                case "О нас":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId, replyMarkup: aboutUs);
                                        return;
                                    }
                                case "Проблемы с ботом":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendTextMessageAsync(chatId, "Перезапустите бота,если не помогло напишите в поддержку", replyMarkup: mainMenu);
                                        return;
                                    }
                                case "Локация":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendVenueAsync(
                                            chatId: chatId,
                                            latitude: 55.780614,
                                            longitude: 37.590993,
                                            title: "Офис организации",
                                            address: "Лесная улица, 43, Москва",
                                            replyMarkup: aboutUs
                                            );
                                        return;
                                    }
                                case "Номер телефона":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendContactAsync(
                                            chatId: chatId,
                                            phoneNumber: "+1234567890",
                                            firstName: "Горячия линия",
                                            replyMarkup: aboutUs
                                            );
                                        return;
                                    }
                                case "Назад":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId, replyMarkup: mainMenu);
                                        return;
                                    }
                                case "Напитки":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId, replyMarkup: drink);
                                        return;
                                    }
                                case "Еда":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId, replyMarkup: Eat);
                                        return;
                                    }

                            }
                            return;


                        }



                }
            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
