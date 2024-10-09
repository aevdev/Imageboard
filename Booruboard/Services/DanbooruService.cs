using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Booruboard.Services
{
    public class DanbooruService
    {
        private readonly HttpClient _httpClient;

        public DanbooruService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //Установка загаловка User-Agent чтобы запрос не блокировался сервером
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Booruboard/1.0");

            // Установка заголовка для авторизации
            var username = "ZoesPantsu";
            var apiKey = "F8q9tMhfJtqws5NMRxnr7Mpg";
            var byteArray = new System.Text.ASCIIEncoding().GetBytes($"{username}:{apiKey}");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<List<ImagePost>> GetImagePostsAsync(string tags, int limit)
        {
            var response = await _httpClient.GetAsync($"https://safebooru.donmai.us/posts.json?tags={tags}&limit={limit}");
            Console.WriteLine($"HTTP RESPONSE:\n{response}\n");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"CONTENT AFTER HTTP-PARSE:\n{content}\n");
            return JsonConvert.DeserializeObject<List<ImagePost>>(content);
        }

        public async Task<List<ImagePost>> GetImagePostsAsync(string tags, int limit, int page)
        {
            var response = await _httpClient.GetAsync($"https://safebooru.donmai.us/posts.json?page={page}&tags={tags}&limit={limit}");
            Console.WriteLine($"HTTP RESPONSE:\n{response}\n");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"CONTENT AFTER HTTP-PARSE:\n{content}\n");
            return JsonConvert.DeserializeObject<List<ImagePost>>(content);
        }
    }

    public class ImagePost
    {
        public int Id { get; set; }
        public string file_url { get; set; }
        public string preview_file_url { get; set; }
    }
}
