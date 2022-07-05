using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmsInfoBot.Constant;
using FilmsInfoBot.Model;
using Newtonsoft.Json;

namespace FilmsInfoBot.Client
{
    public class FilmsClient
    {
        private HttpClient _client;
        private static string _adress;
        private static string _apikey;

        public FilmsClient()
        {
            _adress = Constants.adress;
            _apikey = Constants.apikey;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_adress);
        }
        public async Task<PreviewModel> GetPopularFilmsAsync()
        {
            var responce = await _client.GetAsync($"PopularFilms?ApiKey={_apikey}");
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<PreviewModel>(content);
            return result;
        }
        public async Task<PreviewModel> GetFilmsByGenreAsync(int genre)
        {
            var responce = await _client.GetAsync($"FilmsByGenre?ApiKey={_apikey}&Genre={genre}");
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<PreviewModel>(content);
            return result;
        }
        public async Task<DetailsModel> GetFilmByIDAsync(int ID)
        {
            var responce = await _client.GetAsync($"FilmByID?ID={ID}&ApiKey={_apikey}");
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<DetailsModel>(content);
            return result;
        }
        public async Task<PreviewModel> GetFilmsByNameAsync(string Name)
        {
            var responce = await _client.GetAsync($"FilmsByName?Name={Name}&ApiKey={_apikey}");
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<PreviewModel>(content);
            return result;
        }
        public async Task<GenresModel> GetGenresListAsync()
        {
            var responce = await _client.GetAsync($"GetGenresList?ApiKey={_apikey}");
            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<GenresModel>(content);
            return result;
        }
    }
}
