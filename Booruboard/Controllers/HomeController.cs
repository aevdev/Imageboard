using Azure;
using Booruboard.Models;
using Booruboard.Services;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
namespace Booruboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly DanbooruService _danbooruService;

        public HomeController(DanbooruService danbooruService, HttpClient httpClient)
        {
            _danbooruService = danbooruService;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var images = await _danbooruService.GetImagePostsAsync("solo", 10);
            foreach (var image in images) 
            {
                Console.WriteLine($"Image (id): {image.Id}\nPreviewUrl: {image.preview_file_url}\n");
            }
            return View(images);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string tag, int page = 1, int limit = 10)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return View();
            }

            var offset = (page - 1) * limit;
            var images = await _danbooruService.GetImagePostsAsync(tag, 10, page);

            ViewBag.CurrentPage = page;
            ViewBag.Tag = tag;
            return View("Index", images);
        }
    }
}
