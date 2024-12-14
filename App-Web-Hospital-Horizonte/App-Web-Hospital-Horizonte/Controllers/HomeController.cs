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

        //Aterrizaje del Usuario
        public IActionResult Landing(InformationUser usuarioLogin)
        {
            return View("Main/Landing", usuarioLogin);
        }
        [HttpPost]
        public IActionResult RegisterUsuario(Querys model)
        {
            model.GuardarUsuario();
            return RedirectToAction("Index");
        }

        
        public IActionResult RegisterUsuarioRef() {
            return View("/Views/Home/Register.cshtml");
        }

        [HttpPost]
        public IActionResult LoginUsuario(Querys model)
        {
            InformationUser usuarioLogin = model.SearchInformation();
            if(usuarioLogin!=null)
            {
                return RedirectToAction("Landing", usuarioLogin);
            }
             return RedirectToAction("Login");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
