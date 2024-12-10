using App_Web_Hospital_Horizonte.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App_Web_Hospital_Horizonte.Controllers
{
    public class ControllerRouter : Controller{
        private readonly ILogger<ControllerRouter> _logger;
        public ControllerRouter(ILogger<ControllerRouter> logger)
        {
            _logger = logger;
        }



        
    }
}