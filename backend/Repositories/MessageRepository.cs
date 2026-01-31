using backend.Data;
using backend.Models;

namespace backend.Repositories;

public class MessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateMessage(Message newMessage)
    {
        _context.Messages.Add(newMessage);
        await _context.SaveChangesAsync();
    }
}
