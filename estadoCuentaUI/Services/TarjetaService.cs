using estadoCuentaAPI.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace estadoCuentaUI.Services
{
    public class TarjetaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public TarjetaService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BackendAPI");
        }
        public async Task<List<TarjetaCreditoDTO>> GetTarjetasAsync()
        {
            var response = await _httpClient.GetAsync("/ObtenerListaTarjetas");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<TarjetaCreditoDTO>>();
        }
    }
}
