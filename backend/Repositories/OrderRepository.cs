using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class OrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateOrder(Order newOrder)
    {
        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();

        return newOrder.OrderId;
    }

}
