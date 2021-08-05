using AnimalFarmsMarket.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnimalFarmsMarket.Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new HomePageViewModel
            {
                Slides = new ListOfSlides().Slides
            };
            return View(model);
        }

        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
