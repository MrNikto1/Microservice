using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public OrderController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _httpClient.GetAsync("http://orderservice:8080/api/order");
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _httpClient.GetAsync($"http://orderservice:8080/api/order/{id}");
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] object order)
    {
        var json = JsonSerializer.Serialize(order);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://orderservice:8080/api/order", content);
        if (!response.IsSuccessStatusCode) return StatusCode((int)response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        return Content(result, "application/json");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] object order)
    {
        var json = JsonSerializer.Serialize(order);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"http://orderservice:8080/api/order/{id}", content);
        return StatusCode((int)response.StatusCode);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"http://orderservice:8080/api/order/{id}");
        return StatusCode((int)response.StatusCode);
    }
}