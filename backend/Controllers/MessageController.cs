using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MessageRepository _messageRepository;

        public MessageController(MessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }


        [HttpPost("sendMessage")]
        public async Task<IActionResult> CreateMessage([FromBody] NewMessageRequest request)
        {
            var message = new Message
            {
                Name = request.Name,
                Email = request.Email,
                Comment = request.Comment
            };

            await _messageRepository.CreateMessage(message);

            return Ok();
        }
    }
}
