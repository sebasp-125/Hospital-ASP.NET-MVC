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
        public IActionResult Landing()
        {
            Querys informatioDoctor = new Querys();
            var listaDoctores = informatioDoctor.Doctores();

            Random random = new Random();
            var MedicoAleatorio = listaDoctores[random.Next(listaDoctores.Count)];
            var viewModel = new LandingViewModel
            {
                InformacionDoctores = listaDoctores,
                MedicoDeFamilia = MedicoAleatorio
            };

            string nombre = HttpContext.Session.GetString("NombreUsuario"); ViewBag.Nombre=nombre;
            string apellido = HttpContext.Session.GetString("ApellidoUsuario"); ViewBag.Apellido=apellido;
            string usuarioid = HttpContext.Session.GetString("UsuarioID"); ViewBag.UsuarioID=usuarioid;
            string correo = HttpContext.Session.GetString("Correo"); ViewBag.Correo=correo;
            string telefono = HttpContext.Session.GetString("Telefono"); ViewBag.Telefono=telefono;
            string enfermedad = HttpContext.Session.GetString("Enfermedad"); ViewBag.Enfermedad=enfermedad;
            string descripcionE = HttpContext.Session.GetString("DescripcionEnfermedad"); ViewBag.DescripcionEnf=descripcionE;
            string identificacion = HttpContext.Session.GetString("Identificacion"); ViewBag.Identificacion = identificacion;
            string genero = HttpContext.Session.GetString("Genero"); ViewBag.Genero = genero;

            return View("Main/Landing", viewModel);
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
                HttpContext.Session.SetString("NombreUsuario", usuarioLogin.NombreUsuarioI);
                HttpContext.Session.SetString("ApellidoUsuario", usuarioLogin.ApellidoUsuarioI);
                HttpContext.Session.SetString("UsuarioID", usuarioLogin.idUsuario);
                HttpContext.Session.SetString("Correo", usuarioLogin.EmailUsuarioI);
                HttpContext.Session.SetString("Telefono", usuarioLogin.Telefono);
                HttpContext.Session.SetString("Enfermedad", usuarioLogin.NombreEnfermedad);
                HttpContext.Session.SetString("DescripcionEnfermedad", usuarioLogin.DescripcionEnfermedad);
                HttpContext.Session.SetString("Identificacion", usuarioLogin.Identificacion);
                HttpContext.Session.SetString("Genero", usuarioLogin.Genero);
                return RedirectToAction("Landing");
            }
            return RedirectToAction("Login");
        }

        public IActionResult AsignarCitaSedes()
        {
            return View("Main/AsignarCitaSedes");
        }

        [HttpPost]
        public IActionResult AsignarCita(Querys model)
        {   
            ViewData ["Centro"] = model.Sede;
            HttpContext.Session.SetString("Sede", model.Sede);

            string nombre = HttpContext.Session.GetString("NombreUsuario"); ViewBag.Nombre=nombre;
            string apellido = HttpContext.Session.GetString("ApellidoUsuario"); ViewBag.Apellido=apellido;
            string usuarioid = HttpContext.Session.GetString("UsuarioID"); ViewBag.UsuarioID=usuarioid;
            string correo = HttpContext.Session.GetString("Correo"); ViewBag.Correo=correo;
            string telefono = HttpContext.Session.GetString("Telefono"); ViewBag.Telefono=telefono;
            string identificacion = HttpContext.Session.GetString("Identificacion"); ViewBag.Identificacion = identificacion;
            return View("Main/AsignarCita");
        }

        public IActionResult CitaAsignada(Querys model)
        {
            string sede = HttpContext.Session.GetString("Sede"); 
            string usuarioid = HttpContext.Session.GetString("UsuarioID");
            model.AsignarLaCita(sede, usuarioid);
            return RedirectToAction("Landing");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
