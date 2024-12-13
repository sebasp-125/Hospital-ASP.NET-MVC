using App_Web_Hospital_Horizonte.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App_Web_Hospital_Horizonte.Controllers
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
            return View();
        }
        
        //Loging
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUsuario(RegisterUsuario model)
        {
            
            var usuarioGuardado = model.GuardarUsuario();

            if (usuarioGuardado != null)
            {
                    
                Console.WriteLine("SE HIZO");
                return RedirectToAction("Index"); 
            }
            else
            {
                    
                Console.WriteLine("NO SE HIZO");
            }
           
            
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
