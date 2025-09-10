using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> paymentCheck([FromBody] int paymentAmount)
        {
            System.Console.WriteLine(paymentAmount);
            return Ok();
        }
    }
}
