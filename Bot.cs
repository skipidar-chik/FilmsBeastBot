using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using System.Net;
using Newtonsoft.Json;
using FilmsInfoBot.Constant;
using FilmsInfoBot.Model;
using FilmsInfoBot.Client;

namespace FilmsInfoBot
{
    public class Bot
    {
        TelegramBotClient botClient = new TelegramBotClient("5425174482:AAGf8ysKSTS0I2LV0Vo9H5Ro72lc326wdqM");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

        bool searchByName;
        bool searchByGenre;
        bool popular = false;
        bool Name = false;
        string name1="";
        string name2 = "";
        bool genre = false;

        static int id3 = 0;

        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateAsync, HandlerError, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
            Console.WriteLine($"Bot {botMe.Username} starts working");
            Console.ReadKey();
        }

        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Ecseption in telegram bot API:\n {apiRequestException.ErrorCode}" +
                $"\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
        }
        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (searchByName == true)
            {
                try
                {
                    int number1 = Convert.ToInt32(message.Text);
                    FilmsClient fc = new FilmsClient();
                    int id = fc.GetFilmsByNameAsync(name1).Result.Results[number1 - 1].ID;
                    await SendFilmDetailsAsync(botClient, message, id);
                    searchByName = false;
                }
                catch (Exception exe)
                {
                    Console.WriteLine(exe.Message);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "There is no such number here.");
                }

                return;
            }
            if (searchByGenre == true)
            {
                try
                {
                    int number1 = Convert.ToInt32(message.Text);
                    FilmsClient fc = new FilmsClient();
                    int id = fc.GetFilmsByGenreAsync(id3).Result.Results[number1 - 1].ID;
                    await SendFilmDetailsAsync(botClient, message, id);
                    searchByGenre = false;
                }
                catch (Exception exr)
                {
                    Console.WriteLine(exr.Message);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "There is no such number here.");
                }

                return;
            }
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose command /keyboard");
                return;
            }
            else
            if (message.Text == "/keyboard")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                        {
                        new KeyboardButton[] { "Search by name", "Search by genre", "Popular" },
                        new KeyboardButton[] { "Actors", "Directors", "Favourite" }
                        }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Select a menu item", replyMarkup: replyKeyboardMarkup);
                return;
            }
            else
            if (message.Text == "Search by name")
            {                               
                await botClient.SendTextMessageAsync(message.Chat.Id, "Enter film title");
                Name = true;
                return;
            }
            else
            if (message.Text == "Search by genre")
            {
                genre = true;
                await botClient.SendTextMessageAsync(message.Chat.Id, "Enter film genre");
                return;
            }
            else
            if (message.Text == "Popular")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, $"{Popular()}");
                await botClient.SendTextMessageAsync(message.Chat.Id, $"Enter film number from 1 to 20");
                popular = true;
                return;
            }
            else
            if (message.Text == "Actors")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Feature in development ");
                return;
            }
            else
            if (message.Text == "Directors")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Feature in development ");
                return;
            }
            else
            if (message.Text == "Favourite")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Feature in development ");
                InlineKeyboardMarkup keyboardMarkup = new
                    (
                        new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Films", $"favFilms"),
                                InlineKeyboardButton.WithCallbackData("Actors", $"favActors"),
                                InlineKeyboardButton.WithCallbackData("Directors", $"favDirectors")
                            }
                        }
                    );
                await botClient.SendTextMessageAsync(message.Chat.Id, "Choose film genre", replyMarkup: keyboardMarkup);
                return;
            }
            if (popular == true)
            {
                try
                {
                    int number = Convert.ToInt32(message.Text);
                    FilmsClient fc = new FilmsClient();
                    int id = fc.GetPopularFilmsAsync().Result.Results[number - 1].ID;
                    SendFilmDetailsAsync(botClient, message, id);
                    popular = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "There is no such number here.");
                }
                return;
            }
            if (Name == true)
            {
                try
                {
                    name1 = message.Text;
                    searchByName = true;
                    if (ByName(name1) == "empty")
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "I'm sorry but I don't know this movie.");
                        return;
                    }
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"{ByName(name1)}");
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Enter film number");
                    Name = false;
                }
                catch(Exception ex)
                {

                    searchByName = false;
                    Console.WriteLine(ex.Message);
                    await botClient.SendTextMessageAsync(message.Chat.Id, "I'm sorry but I don't know this movie.");
                }
                return;
            } 
            if (genre == true)
            {                               
                try
                {
                    genre = false;
                    name2 = message.Text;
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"{ByGenre(name2)}");
                    searchByGenre = true;
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Enter film number from 1 to 20");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    searchByGenre = false;
                    await botClient.SendTextMessageAsync(message.Chat.Id, "I'm sorry but I don't know this genre.");
                }
                return;
            }
        
        }
        public static string Popular()
        {
            FilmsClient fc = new FilmsClient();
            var result = fc.GetPopularFilmsAsync().Result.Results;
            string films = "";
            for (int i = 0; i < result.Count; i++)
            {
                films += $"{i + 1}. {result[i].Title} ({result[i].Release_date.Replace("-", ".")})\n";
            }
            return films;
        }
        public static string ByName(string name)
        {
            FilmsClient fc = new FilmsClient();
            for (int i = 0; i < fc.GetFilmsByNameAsync(name).Result.Results.Count; i++)
            {
                if (fc.GetFilmsByNameAsync(name).Result.Results[i].Title == null)
                    return "empty";
            }
            
            var result = fc.GetFilmsByNameAsync(name).Result.Results;
            string films = "";
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].Release_date == null) films += $"{i + 1}. {result[i].Title}";
                else films += $"{i + 1}. {result[i].Title} ({result[i].Release_date.Replace("-", ".")})\n";
            }
            return films;
        }
        public static string ByGenre (string name)
        {
            FilmsClient fc = new FilmsClient();
            string films = "";
            for (int i = 0; i < fc.GetGenresListAsync().Result.genres.Count; i++)
            {
                if (name == fc.GetGenresListAsync().Result.genres[i].Name)
                {
                    id3 = fc.GetGenresListAsync().Result.genres[i].ID;
                    break;
                }
            }
            var result = fc.GetFilmsByGenreAsync(id3).Result.Results;
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].Release_date == null) films += $"{i + 1}. {result[i].Title}";
                else films += $"{i + 1}. {result[i].Title} ({result[i].Release_date.Replace("-", ".")})\n";
            }
            return films;
        }
        private async Task SendFilmDetailsAsync(ITelegramBotClient botClient, Message message, int id)
        {   
            FilmsClient fc = new FilmsClient();
            var result = fc.GetFilmByIDAsync(id).Result;
            string genres = "";
            string companies = "";
            string countries = "";

            for (int i = 0; i < result.Genres.Count; i++)
            {
                if (i < result.Genres.Count - 1)
                    genres += result.Genres[i].Name + ", ";
                else genres += result.Genres[i].Name;
            }
            for (int i = 0; i < result.Production_companies.Count; i++)
            {
                if (i < result.Production_companies.Count - 1)
                    companies += result.Production_companies[i].Name + ", ";
                else companies += result.Production_companies[i].Name;
            }
            for (int i = 0; i < result.Production_countries.Count; i++)
            {
                if (i < result.Production_countries.Count - 1)
                    countries += result.Production_countries[i].Name + ", ";
                else countries += result.Production_countries[i].Name;
            }

            string films = $"<b>{result.Title}</b>\n\n" +
                $"{char.ConvertFromUtf32(0x1F3AD)}Genres: {genres}\n\n" +
                $"{char.ConvertFromUtf32(0x1F55C)}Runtime: {result.Runtime} minutes\n\n" +
                $"<em>{result.Tagline}</em>\n\n" +
                $"{result.Overview}\n\n" +
                $"{char.ConvertFromUtf32(0x1F3AC)}Production companies: {companies}\n\n" +
                $"{char.ConvertFromUtf32(0x1F30D)}Production countries: {countries}\n\n" +
                $"{char.ConvertFromUtf32(0x2B50)}Rating: {result.Vote_average}";

            await botClient.SendPhotoAsync(chatId: message.Chat.Id,
                photo: $"https://image.tmdb.org/t/p/w300{fc.GetFilmByIDAsync(id).Result.Poster_path}",
                caption: $"{films}",
                parseMode: ParseMode.Html);
        }        
    }
}