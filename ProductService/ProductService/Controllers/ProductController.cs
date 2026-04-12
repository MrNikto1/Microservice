using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private static List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 1000 },
        new Product { Id = 2, Name = "Phone", Price = 500 }
    };

    [HttpGet]
    public IEnumerable<Product> GetAll() => _products;

    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public ActionResult<Product> Create(Product product)
    {
        product.Id = _products.Max(p => p.Id) + 1;
        _products.Add(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Product updatedProduct)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        _products.Remove(product);
        return NoContent();
    }
}