using System;

namespace backend.Models;

public class Message
{
    public int MessageId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

}
