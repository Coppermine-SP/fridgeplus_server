/*
    IntelligenceController - fridgeplus_server
    Copyright (C) 2024-2025 Coppermine-SP
 */

using System.Net;
using System.Security.Claims;
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
    public class IntelligenceController(ILogger<IntelligenceController> logger, IReceiptRecognizeService receipt, IChatCompletionService chatCompletion, ServerDbContext context) : ControllerBase
    {
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        private string _getCurrentUserId() => HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Sid))?.Value ?? "null";    

        record ReceiptImportResult(IReceiptRecognizeService.ReceiptItem[]? items);
        record ReceiptImportRequest(IReceiptRecognizeService.ReceiptItem[]? Items, Category[]? Categories);

        record InsightRequest(Item[] items);
        record InsightResult(string name, string description, string[] steps);
            
        [HttpPost]
        [Authorize]
        public IActionResult ImportFromReceipt(IFormFile image)
        {
            string taskId = Guid.NewGuid().ToString().Split('-')[0];
            logger.LogInformation($"#{taskId}: New ImportFromReceipt request.");

            const string prompt =
                "마트에서 구매한 내역이 담긴 JSON 데이터를 사용하여, 냉동되지 않은 신선 식품만을 추출하고 정보 업데이트를 수행하세요.고객이 냉동되지 않은 신선 식품을 찾기 용이하도록 해당 제품에 대해 적절한 카테고리 식별자와 상품 이름의 정규화 작업을 수행합니다.\r\n\r\n# Steps\r\n\r\n1. **필터링: 냉동되지 않은 신선 식품 찾기**\r\n   - JSON에서 제공된 모든 구매 내역을 탐색하고, 상품의 `description` 필드를 검사하여 신선 식품을 찾습니다.\r\n   - 신선 식품인지 여부는 상품 이름(description)을 기반으로 판단하며, 냉동 식품은 제외합니다.\r\n\r\n2. **카테고리 매칭 및 업데이트**\r\n   - 신선 식품으로 분류된 상품에 대하여 제공된 카테고리 데이터에서 적절한 `categoryId`를 찾습니다.\r\n   - 상품 이름과 가장 일치하는 카테고리를 찾아, 해당 `categoryId` 필드를 업데이트하세요.\r\n\r\n3. **상품 이름 정규화**\r\n   - 상품 이름에서 불필요한 태그나 특수 문자를 제거하여 고객이 쉽게 인식할 수 있도록 정규화합니다.\r\n   - 상품의 원래 의미를 훼손하지 않도록, 가능한 한 실제 상품명에 충실히 유지하여 변경합니다.";
            var azureResult = receipt.ImportFromReceipt(taskId, image);
            
            if (azureResult is null)
            {
                logger.LogWarning($"#{taskId}: Failed. (azureResult was null.)");
                return BadRequest();
            }
            
            var gptResult = chatCompletion.ChatCompletion(taskId, prompt,
                new ReceiptImportRequest(azureResult.ToArray(), context.Categories.ToArray()),
                typeof(ReceiptImportResult));

            if (gptResult is null)
            {
                logger.LogWarning($"#{taskId}: Failed. (gptResult was null.)");
                return BadRequest();
            }

            logger.LogInformation($"#{taskId}: Completed.\n" + JsonSerializer.Serialize(gptResult, _serializerOptions));
            return new JsonResult(gptResult);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Insight()
        {
            string taskId = Guid.NewGuid().ToString().Split('-')[0];
            logger.LogInformation($"#{taskId}: New Insight request.");

            const string prompt = "사용자의 냉장고에 있는 식품을 기반으로 만들 수 있는 요리를 추천하세요. 추천하는 요리는 재료를 최대한 활용하며, 사용자가 쉽게 따라할 수 있도록 단계별로 조리 과정을 설명해야 합니다. 단계에는 1. 2.와 같은 첨자를 달지 마세요. 기초적인 조미료(예: 소금, 간장 등)는 추가해도 괜찮습니다.# Steps\n\n1. **재료 확인**: 냉장고 내부의 JSON 데이터를 분석하여 사용 가능한 재료를 파악합니다.\n2. **요리 결정**: 분석한 재료로 만들 수 있는 간단하고 빠른 요리를 추천합니다.\n3. **단계별 조리법 구성**: 재료가 최대한 활용될 수 있도록 간단한 조리 순서를 생각하고, 그 과정을 사용자가 이해하기 쉽게 나열합니다.\n -사용 가능한 기초 조미료를 추가할 수 있습니다.\n -단계는 순서대로 쉽게 따라할 수 있도록 명료하게 서술합니다.";
            string uid = _getCurrentUserId();

            var gptResult = chatCompletion.ChatCompletion(taskId, prompt,
                new InsightRequest(context.Items.Where(x => x.ItemOwner == uid).OrderByDescending(x => x.ItemImportDate).Take(20).ToArray()),
                typeof(InsightResult));

            if (gptResult is null)
            {
                logger.LogWarning($"#{taskId}: Failed. (gptResult was null.)");
                return BadRequest();
            }
            
            logger.LogInformation($"#{taskId}: Completed.\n" + JsonSerializer.Serialize(gptResult, _serializerOptions));
            return new JsonResult(gptResult);
        }

    }
}
