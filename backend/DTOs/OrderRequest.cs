using System;

namespace backend.DAOs;

public class OrderRequest
{
    public AddressDto Address { get; set; }
    public string Email { get; set; }
    public List<OrderItemDto> Order { get; set; }
    public decimal PaymentAmount { get; set; }
}

public class AddressDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }
}

public class OrderItemDto
{
    public int ProductId { get; set; }
    public string Size { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

