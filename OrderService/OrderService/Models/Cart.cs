namespace OrderService.Models;

public class Cart
{
    public int UserId { get; set; }
    public List<CartItem> Items { get; set; } = new();
}

public class CartItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
