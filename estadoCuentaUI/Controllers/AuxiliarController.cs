using Microsoft.AspNetCore.Mvc;

namespace estadoCuentaUI.Controllers
{
    public class AuxiliarController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuxiliarController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("api/config/backendurl")]
        public string GetBackendUrl()
        {
             
            return _configuration["BackendUrl"];
        }
    }
}
