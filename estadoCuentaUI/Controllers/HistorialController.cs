using estadoCuentaAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace estadoCuentaUI.Controllers
{
    public class HistorialController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HistorialController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public  IActionResult Index(int tarjetaCreditoId)
        {
            ViewBag.Id = tarjetaCreditoId;

            return View();

        }
    }
}
