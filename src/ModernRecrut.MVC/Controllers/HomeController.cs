using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernRecrut.MVC.Models;
using System.Diagnostics;

namespace ModernRecrut.MVC.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CodeStatus(int code)
        {
            CodeStatusViewModel codeStatusViewModel = new CodeStatusViewModel();
            codeStatusViewModel.IdRequete = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            codeStatusViewModel.CodeStatus = code;

            codeStatusViewModel.MessageErreur = code switch
            {
                404 => "La page demandée est introuvable.",
                
                500 => "Plateforme en cours de maintenance.",
                _ => "Une erreur c'est produite lors de l'exécution de la requête.",
            };
            codeStatusViewModel.MessageCode = code switch
            {
                404 => "Page introuvable",
                500 => "Plateforme en maintenance",
                _ => "Une erreur générique",
            };
            return View(codeStatusViewModel);
        }
    }
}