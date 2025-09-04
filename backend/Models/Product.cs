using System;

namespace backend.Models;

public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string League { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageURL { get; set; } = string.Empty;
    public double Price { get; set; }
}
