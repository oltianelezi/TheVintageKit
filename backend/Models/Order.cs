using System;

namespace backend.Models;

public class Order
{
    public int OrderId { get; set; }
    public decimal TotalPrice { get; set; }
    public string Email { get; set; } = string.Empty;
    public int AddressId { get; set; }
}
