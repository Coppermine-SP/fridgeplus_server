/*
    FridgeController - fridgeplus_server
    Copyright (C) 2024-2025 Coppermine-SP
 */


using fridgeplus_server.Context;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Authorize]
        public IActionResult CategoryList()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ItemList()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddItems()
        {
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public IActionResult DeleteItem(string id)
        {
            return Ok();
        }


    }
}
