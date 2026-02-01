using backend.Data;
using backend.Models;

namespace backend.Repositories;

public class OrderItemRepository
{
    private readonly AppDbContext _context;

    public OrderItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task NewOrderItems(List<OrderItem> orderItems)
    {
        await _context.OrderItems.AddRangeAsync(orderItems);
        await _context.SaveChangesAsync();
    }
}
