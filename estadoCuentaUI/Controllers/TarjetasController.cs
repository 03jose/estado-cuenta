using estadoCuentaAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace estadoCuentaUI.Controllers;

public class TarjetasController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public TarjetasController(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        var backendUrl = _configuration["BackendUrl"];
        var tarjetas = await _httpClient.GetFromJsonAsync<List<ClienteTarjetaDTO>>($"{backendUrl}/TarjetaCredito/ObtenerListaTarjetas");
        return View(tarjetas);
    }
}    
