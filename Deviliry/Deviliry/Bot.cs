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
using System.Net;
using Microsoft.EntityFrameworkCore;




namespace Deviliry
{
    public class Bot
    {
        private const string _token = "7469164812:AAFxy9q6HQb7V303DpXW0OaPYqGDv2mgJWM";
        private TelegramBotClient _client = new TelegramBotClient(_token);
        string _cart = string.Empty;
        private long _id;
        private DataBase.ApplicationContext db = new DataBase.ApplicationContext();
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
                        InlineKeyboardButton.WithCallbackData(text:"Заказать еду и напиток",callbackData:"Заказать еду и напиток"),
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
                        InlineKeyboardButton.WithCallbackData(text:"Газировка",callbackData:"Газировка"),
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
                InlineKeyboardMarkup pizza = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Маргарита",callbackData:"Маргарита")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Четыре сыра",callbackData:"Четыре сыра")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Пеперони",callbackData:"Пеперони")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData:"Назад")
                    }
                });
                InlineKeyboardMarkup juice = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Апельсиновый",callbackData:"Апельсиновый")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Яблочный",callbackData:"Яблочный")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Вишнёвый",callbackData:"Вишнёвый")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData:"Назад")
                    },
                });
                InlineKeyboardMarkup gazirovka = new(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Спрайт",callbackData:"Спрайт"),
                        InlineKeyboardButton.WithCallbackData(text:"Фанта",callbackData:"Фанта")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData:"Назад"),

                    }
                });
                InlineKeyboardMarkup coffee = new(new[]
               {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Американо",callbackData:"Американо"),
                        InlineKeyboardButton.WithCallbackData(text:"Капучино",callbackData:"Капучино")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData:"Назад"),

                    }
                });
                InlineKeyboardMarkup back = new(new[]
                {
                    InlineKeyboardButton.WithCallbackData(text:"Завершить покупки",callbackData:"Завершить покупки"),
                    InlineKeyboardButton.WithCallbackData(text:"Назад",callbackData:"back"),
                });


                
                switch (update.Type)
                {
                    case UpdateType.Message:
                        {
                            _id = update.Message.From.Id;
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
                                case "/shop":
                                    {
                                        db.Users.FirstOrDefault(x => x.IdTelegram == update.Message.From.Id).Products.Clear();
                                        db.SaveChanges();
                                        await _client.SendTextMessageAsync(chatId,"Выберите следующую категорию",replyMarkup: mainMenu);
                                        return;
                                    }

                            }
                            if (messageText.StartsWith("/address") && messageText.Length > 9)
                            {
                                
                                var address = messageText.Substring(9);
                                if (!db.Users.Any(x => x.IdTelegram == _id))
                                {
                                    DataBase.User user = new DataBase.User
                                    {
                                        Name = update.Message.From.FirstName,
                                        IdTelegram = _id,
                                        Address = address
                                    };
                                    db.Users.Add(user);

                                }
                                else
                                {
                                    var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                    if (user != null)
                                    {
                                        user.Address = address;
                                    }

                                }
                                db.SaveChanges();
                                await client.SendTextMessageAsync(chatId, "Адрес добавлен ,введите ваше имя");

                            }
                            else if (!messageText.StartsWith("/name") && !messageText.StartsWith("/address"))
                            {
                                await client.SendTextMessageAsync(chatId, "Пожалуйста ,введите адрес в формате /address [Ваш адрес]");
                            }
                            if (messageText.StartsWith("/name"))
                            {                           
                                string name = messageText.Substring(5);
                                var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                if (user != null)
                                {
                                    user.Name = name;
                                }
                                db.SaveChanges();
                                await client.SendTextMessageAsync(chatId, "Учётная запись создана ,выберите следующие действия",
                                    replyMarkup: mainMenu);

                            }
                            else
                            {
                                await client.SendTextMessageAsync(chatId, "Пожалуйста ,введите Имя в формате /name [Ваше имя]");
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
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId,
                                            replyMarkup: mainMenu);
                                        return;
                                    }
                                case "Напитки":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendTextMessageAsync(chatId, "Выберите напиток", replyMarkup: drink);
                                        return;
                                    }
                                case "Еда":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId,
                                             replyMarkup: Eat);
                                        return;
                                    }
                                case "Пицца":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId
                                             );
                                        await _client.SendTextMessageAsync(chatId, "Выберите пиццу", replyMarkup: pizza);
                                        return;
                                    }
                                case "Кофе":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId
                                             );
                                        await _client.SendTextMessageAsync(chatId, "Выберите кофе", replyMarkup: coffee);
                                        return;
                                    }
                                case "Сок":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId
                                             );
                                        await _client.SendTextMessageAsync(chatId, "Выберите сок", replyMarkup: juice);
                                        return;
                                    }
                                case "Газировка":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendTextMessageAsync(chatId, "Выберите газировку", replyMarkup: gazirovka);
                                        return;
                                    }
                                case "Салатик":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Салатик");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Салатик' not found.");
                                            return;
                                        }

                                        var user = db.Users.Include(u => u.Products).FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine(_id);
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Салатик - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/салат.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "back":
                                    {
                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendTextMessageAsync(chatId, "Продолжить покупки", replyMarkup: Eat);
                                        return;
                                    }
                                case "Картошечка":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Картошечка");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Картошечка' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            Console.WriteLine(callbackQuery.Message.From.Id);
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Картошечка - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/картошка.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Четыре сыра":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Четыре сыра");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Четыре сыра' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Четыре сыра - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/четыре сыра.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Пеперони":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Пеперони");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Пеперони' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Пеперони - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/peperoni.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Маргарита":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Маргарита");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Маргарита' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Маргарита - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/маргарита.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Суши":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Суши");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Суши' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Суши - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/суши.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Шаурма":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Шаурма");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Шаурма' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Шаурма - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/шаурма.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Бургер":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Бургер");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Бургер' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Бургер - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/бургер.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Котлетки":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Котлетки");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Котлетки' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Котлетки - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/котлеты.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Шоколадка":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Шоколадка");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Шоколадка' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Шоколадка - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/шоколадка.jpg", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Водичка":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Водичка");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Водичка' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Водичка - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/вода.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Спрайт":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Спрайт");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Спрайт' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Спрайт - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/спра.jpg", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Фанта":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Фанта");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Фанта' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Фанта - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/фанта.jpg", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Американо":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Американо");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Американо' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Американо - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/американо.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Капучино":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Капучино");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Капучино' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Капучино - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        SendPhoto(chatId, "./photo/капучино.webp", $"Текущий чек\n\n{_cart}", back).Wait();
                                        return;
                                    }
                                case "Вишнёвый":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Вишнёвый");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Вишнёвый' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Вишнёвый - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendTextMessageAsync(chatId, $"Текущий чек\n\n{_cart}", replyMarkup: back);
                                        return;
                                    }
                                case "Апельсиновый":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Апельсиновый");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Апельсиновый' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Апельсиновый - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendTextMessageAsync(chatId, $"Текущий чек\n\n{_cart}", replyMarkup: back);
                                        return;
                                    }
                                case "Яблочный":
                                    {
                                        var product = db.Products.FirstOrDefault(x => x.Name == "Яблочный");
                                        if (product == null)
                                        {
                                            await _client.SendTextMessageAsync(chatId, "Product 'Яблочный' not found.");
                                            return;
                                        }

                                        var user = db.Users.FirstOrDefault(x => x.IdTelegram == _id);
                                        if (user == null)
                                        {
                                            Console.WriteLine("Пользователь не найден");
                                            await _client.SendTextMessageAsync(chatId, "User not found.");
                                            return;
                                        }

                                        user.Products.Add(product);
                                        db.SaveChanges();

                                        var productPrice = product.Price; // Cache the price to avoid another query
                                        _cart += $"Яблочный - {productPrice}руб.\n";

                                        await _client.EditMessageReplyMarkupAsync(chatId, callbackQuery.Message.MessageId);
                                        await _client.SendTextMessageAsync(chatId, $"Текущий чек\n\n{_cart}", replyMarkup: back);
                                        return;
                                    }
                                case "Завершить покупки":
                                    {
                                        _cart = string.Empty;
                                        await _client.SendTextMessageAsync(chatId, $"" +
                                            $"Заказ создан,если хотите совершить новую покупку напишите '/shop'" );
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
