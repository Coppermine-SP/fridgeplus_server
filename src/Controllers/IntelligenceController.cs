using fridgeplus_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fridgeplus_server.Controllers
{
    [Route("api/intelligence/[action]")]
    [ApiController]
    public class IntelligenceController : ControllerBase
    {
        private ILogger _logger;
        private IReceiptRecognizeService _receipt;

        public IntelligenceController(ILogger<IntelligenceController> logger, IReceiptRecognizeService receipt)
        {
            _logger = logger;
            _receipt = receipt;
        }

        [HttpPost]
        [Authorize]
        public IActionResult ImportFromReceipt(IFormFile image)
        {
            var result = _receipt.ImportFromReceipt(image);
            return new JsonResult(result);
        }
    }
}
