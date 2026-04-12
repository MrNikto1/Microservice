namespace OrderService.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public List<int> ProductIds { get; set; } = new();
    public decimal Total { get; set; }
}