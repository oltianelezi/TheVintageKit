using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        public class PaymentRequest
        {
            public decimal paymentAmount { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> paymentCheck([FromBody] PaymentRequest request)
        {
            System.Console.WriteLine(request.paymentAmount);
            return Ok();
        }
    }
}
