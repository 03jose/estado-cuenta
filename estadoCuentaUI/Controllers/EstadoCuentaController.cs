using Microsoft.AspNetCore.Mvc;

namespace estadoCuentaUI.Controllers
{
    public class EstadoCuentaController : Controller
    {
        public IActionResult Index(int tarjetaCreditoId)
        {
            ViewBag.Id = tarjetaCreditoId;

            return View();

        }
    }
}
