using Microsoft.AspNetCore.Mvc;
using OrderService.Models;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private static List<Cart> _carts = new();

    [HttpGet("{userId}")]
    public ActionResult<Cart> GetCart(int userId)
    {
        var cart = _carts.FirstOrDefault(c => c.UserId == userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _carts.Add(cart);
        }
        return cart;
    }

    [HttpPost("{userId}/items")]
    public ActionResult<Cart> AddItem(int userId, [FromBody] CartItem item)
    {
        var cart = _carts.FirstOrDefault(c => c.UserId == userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _carts.Add(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            cart.Items.Add(item);
        }

        return cart;
    }

    [HttpPut("{userId}/items/{productId}")]
    public ActionResult<Cart> UpdateItem(int userId, int productId, [FromBody] CartItem updatedItem)
    {
        var cart = _carts.FirstOrDefault(c => c.UserId == userId);
        if (cart == null) return NotFound();

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return NotFound();

        item.Quantity = updatedItem.Quantity;
        item.Price = updatedItem.Price;

        return cart;
    }

    [HttpDelete("{userId}/items/{productId}")]
    public ActionResult<Cart> RemoveItem(int userId, int productId)
    {
        var cart = _carts.FirstOrDefault(c => c.UserId == userId);
        if (cart == null) return NotFound();

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return NotFound();

        cart.Items.Remove(item);
        return cart;
    }

    [HttpDelete("{userId}")]
    public IActionResult ClearCart(int userId)
    {
        var cart = _carts.FirstOrDefault(c => c.UserId == userId);
        if (cart == null) return NotFound();

        cart.Items.Clear();
        return NoContent();
    }
}
