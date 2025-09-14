using backend.DAOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> NewOrder([FromBody] OrderRequest request)
        {
            

            return Ok();
        }
    }
}
