/*
    IntelligenceController - fridgeplus_server
    Copyright (C) 2024-2025 Coppermine-SP
 */


using System.Text.Encodings.Web;
using fridgeplus_server.Context;
using fridgeplus_server.Models;
using fridgeplus_server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authorization;

namespace fridgeplus_server.Controllers
{
    [Route("api/intelligence/[action]")]
    [ApiController]
    public class IntelligenceController : ControllerBase
    {
        private ILogger _logger;
        private IReceiptRecognizeService _receipt;
        private IChatCompletionService _chatCompletion;
        private ServerDbContext _context;

        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        public IntelligenceController(ILogger<IntelligenceController> logger, IReceiptRecognizeService receipt, IChatCompletionService chatCompletion, ServerDbContext context)
        {
            _logger = logger;
            _receipt = receipt;
            _chatCompletion = chatCompletion;
            _context = context;
        }

        record ReceiptImportResult
        {
            public ReceiptItem[] items { get; set; }
        }
        record ReceiptImportRequest
        {
            public ReceiptItem[] items { get; set; }
            public Category[] categories { get; set; }
        }
        record InsightResult
        {
            public string result { get; set; }
        }

        [HttpPost]
        [Authorize]
        public IActionResult ImportFromReceipt(IFormFile image)
        {
            string taskId = Guid.NewGuid().ToString().Split('-')[0];
            _logger.LogInformation($"#{taskId}: New ImportFromReceipt request.");

            const string prompt =
                "마트에서 구매한 내역이 담긴 JSON 데이터를 사용하여, 냉동되지 않은 신선 식품만을 추출하고 정보 업데이트를 수행하세요.고객이 냉동되지 않은 신선 식품을 찾기 용이하도록 해당 제품에 대해 적절한 카테고리 식별자와 상품 이름의 정규화 작업을 수행합니다.\r\n\r\n# Steps\r\n\r\n1. **필터링: 냉동되지 않은 신선 식품 찾기**\r\n   - JSON에서 제공된 모든 구매 내역을 탐색하고, 상품의 `description` 필드를 검사하여 신선 식품을 찾습니다.\r\n   - 신선 식품인지 여부는 상품 이름(description)을 기반으로 판단하며, 냉동 식품은 제외합니다.\r\n\r\n2. **카테고리 매칭 및 업데이트**\r\n   - 신선 식품으로 분류된 상품에 대하여 제공된 카테고리 데이터에서 적절한 `categoryId`를 찾습니다.\r\n   - 상품 이름과 가장 일치하는 카테고리를 찾아, 해당 `categoryId` 필드를 업데이트하세요.\r\n\r\n3. **상품 이름 정규화**\r\n   - 상품 이름에서 불필요한 태그나 특수 문자를 제거하여 고객이 쉽게 인식할 수 있도록 정규화합니다.\r\n   - 상품의 원래 의미를 훼손하지 않도록, 가능한 한 실제 상품명에 충실히 유지하여 변경합니다.";
            var azureResult = _receipt.ImportFromReceipt(taskId, image);
            var gptResult = _chatCompletion.ChatCompletion(taskId, prompt,
                new ReceiptImportRequest() {items = azureResult.ToArray(), categories = _context.Categories.ToArray()}, 
                typeof(ReceiptImportResult));

            if (gptResult is null)
            {
                _logger.LogWarning($"#{taskId}: Failed.");
                return BadRequest();
            }

            _logger.LogInformation($"#{taskId}: Completed.\n" + JsonSerializer.Serialize(gptResult, _serializerOptions));
            return new JsonResult(gptResult);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Insight()
        {
            string taskId = Guid.NewGuid().ToString().Split('-')[0];
            _logger.LogInformation($"#{taskId}: New Insight request.");


            return Ok();
        }

    }
}
