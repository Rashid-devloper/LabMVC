using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using PathLabMvc.Models;              // <-- MVC model namespace
using System.Text.Json;

namespace PathLabMvc.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOpts = new() { PropertyNameCaseInsensitive = true };

        public DoctorsController(IHttpClientFactory httpFactory)
        {
            _http = httpFactory.CreateClient("PathLabApi"); // register this in Program.cs
        }

        // GET: /Doctors
        public async Task<IActionResult> Index()
        {
            try
            {
                var doctors = await _http.GetFromJsonAsync<List<Doctor>>("Doctors", _jsonOpts) ?? new();
                return View(doctors);
            }
            catch (HttpRequestException ex)
            {
                // Log if you have logging, show friendly message
                TempData["Error"] = "Unable to load doctors: " + ex.Message;
                return View(new List<Doctor>());
            }
        }

        // GET: /Doctors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var resp = await _http.GetAsync($"Doctors/{id}");
            if (!resp.IsSuccessStatusCode) return NotFound();
            var doc = await resp.Content.ReadFromJsonAsync<Doctor>(_jsonOpts);
            return View(doc);
        }

        // GET: /Doctors/Create
        public IActionResult Create() => View();

        // POST: /Doctors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor model)
        {
            if (!ModelState.IsValid) return View(model);

            var resp = await _http.PostAsJsonAsync("Doctors", model);
            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "API error while creating doctor");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Doctors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var resp = await _http.GetAsync($"Doctors/{id}");
            if (!resp.IsSuccessStatusCode) return NotFound();
            var doc = await resp.Content.ReadFromJsonAsync<Doctor>(_jsonOpts);
            return View(doc);
        }

        // POST: /Doctors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var resp = await _http.PutAsJsonAsync($"Doctors/{id}", model);
            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "API error while updating doctor");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Doctors/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _http.GetAsync($"Doctors/{id}");
            if (!resp.IsSuccessStatusCode) return NotFound();
            var doc = await resp.Content.ReadFromJsonAsync<Doctor>(_jsonOpts);
            return View(doc);
        }

        // POST: /Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resp = await _http.DeleteAsync($"Doctors/{id}");
            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = "API error while deleting doctor";
                return RedirectToAction(nameof(Delete), new { id });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
