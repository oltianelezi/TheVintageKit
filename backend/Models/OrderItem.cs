using System;

namespace backend.Models;

public class OrderItem
{
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public string Size { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }

}
