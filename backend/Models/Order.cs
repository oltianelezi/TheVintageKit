namespace backend.Models;

public class Order
{
    public int OrderId { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Email { get; set; }
    public int AddressId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Address? Address { get; set; }
}
