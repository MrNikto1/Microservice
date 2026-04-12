using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private static List<Order> _orders = new();

    [HttpGet]
    public IEnumerable<Order> GetAll() => _orders;

    [HttpGet("{id}")]
    public ActionResult<Order> GetById(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return NotFound();
        return order;
    }

    [HttpPost]
    public ActionResult<Order> Create(Order order)
    {
        order.Id = _orders.Any() ? _orders.Max(o => o.Id) + 1 : 1;
        _orders.Add(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Order updatedOrder)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return NotFound();
        order.UserId = updatedOrder.UserId;
        order.ProductIds = updatedOrder.ProductIds;
        order.Total = updatedOrder.Total;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return NotFound();
        _orders.Remove(order);
        return NoContent();
    }
}