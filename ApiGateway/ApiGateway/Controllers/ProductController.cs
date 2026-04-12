using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ProductController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _httpClient.GetAsync("http://localhost:5001/api/product");
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5001/api/product/{id}");
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] object product)
    {
        var json = JsonSerializer.Serialize(product);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5001/api/product", content);
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        return Content(result, "application/json");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] object product)
    {
        var json = JsonSerializer.Serialize(product);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"http://localhost:5001/api/product/{id}", content);
        return StatusCode((int)response.StatusCode);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"http://localhost:5001/api/product/{id}");
        return StatusCode((int)response.StatusCode);
    }
}