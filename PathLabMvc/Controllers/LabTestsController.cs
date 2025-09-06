using Microsoft.AspNetCore.Mvc;
using PathLabMvc.Models.ResponseDto;
using System.Text.Json;
using System.Net.Http.Json;

namespace PathLabMvc.Controllers
{
    public class LabTestsController : Controller
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

        public LabTestsController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("PathLabApi");
        }

        public async Task<IActionResult> Index()
        {
            // GET https://localhost:7129/api/LabTests
            var tests = await _http.GetFromJsonAsync<List<LabTestDto>>("LabTests", _jsonOpts)
                        ?? new List<LabTestDto>();
            return View(tests);
        }

        public async Task<IActionResult> Details(int id)
        {
            var resp = await _http.GetAsync($"LabTests/{id}");
            if (!resp.IsSuccessStatusCode) return NotFound();
            var dto = await resp.Content.ReadFromJsonAsync<LabTestDto>(_jsonOpts);
            return View(dto);
        }

        

    }
}
