/*
    FridgeController - fridgeplus_server
    Copyright (C) 2024-2025 Coppermine-SP
 */

using System.Diagnostics.CodeAnalysis;
using fridgeplus_server.Context;
using fridgeplus_server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fridgeplus_server.Controllers
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Route("api/fridge/[action]")]
    [ApiController]
    public class FridgeController(ILogger<FridgeController> logger, ServerDbContext context) : ControllerBase
    {
        record CategoryListResult(Category[] categories);
        record ItemListResult(Item[] items);
        public record AddItem(int categoryId, string itemDescription, int itemQuantity, DateTime expires);
        public record AddItemsRequest(AddItem[]? items);
        
        private string _getCurrentUserId() => HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Sid))?.Value ?? "null";       
            
        [HttpGet]
        [Authorize]
        public IActionResult CategoryList() => new JsonResult(new CategoryListResult(context.Categories.ToArray()));

        [HttpGet]
        [Authorize]
        public IActionResult ItemList()
        {
            string uid = _getCurrentUserId();
            return new JsonResult(new ItemListResult(context.Items.Where(x => x.ItemOwner == uid).OrderBy(x => x.ItemExpireDate).ToArray()));
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddItems(AddItemsRequest data)
        {
            if (data.items is null) return BadRequest("item field should be not null.");
            foreach (var x in data.items)
            {
                context.Items.Add(new Item()
                {
                    CategoryId = x.categoryId,
                    ItemDescription = x.itemDescription,
                    ItemExpireDate = x.expires,
                    ItemQuantity = x.itemQuantity,
                    ItemImportDate = DateTime.Today,
                    ItemOwner = _getCurrentUserId()
                });
            }

            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteItem([FromForm]int id)
        {
            string uid = _getCurrentUserId();

            var target = context.Items.SingleOrDefault(x => x.ItemOwner == uid && x.ItemId == id);
            if (target is null)
            {
                logger.LogInformation($"There is no item #{id} with owner {uid}!");
                return BadRequest();
            }

            context.Items.Remove(target);
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Reset()
        {
            string uid = _getCurrentUserId();

            var target = context.Items.Where(x => x.ItemOwner == uid);

            foreach (var x in target) context.Items.Remove(x);
            context.SaveChanges();
            logger.LogInformation($"Reset user {uid}. (deleted {target.Count()} records.)");

            return Ok();
        }
    }
}
