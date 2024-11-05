using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fridgeplus_server.Controllers
{
    [Route("api/intelligence/[action]")]
    [ApiController]
    public class IntelligenceController : ControllerBase
    {
        [HttpPost]
        public IActionResult ImportFromReceipt(IFormFile image)
        {
            return Ok();
        }
    }
}
