using System;

namespace backend.DTOs;

public class NewMessageRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

}
