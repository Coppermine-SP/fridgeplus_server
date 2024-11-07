using fridgeplus_server.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace fridgeplus_server.Controllers
{
    [Route("api/fridge/[action]")]
    [ApiController]
    public class FridgeController : ControllerBase
    {
        private ILogger _logger;
        private ServerDbContext _dbContext;

        public FridgeController(ILogger<FridgeController> logger, ServerDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }
    }
}
