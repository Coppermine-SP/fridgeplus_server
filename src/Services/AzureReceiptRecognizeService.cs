using System.Configuration.Internal;
using System.Text;
using Azure;
using Azure.AI.DocumentIntelligence;
using fridgeplus_server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using MySql.Data.MySqlClient;

namespace fridgeplus_server.Services
{
    public class AzureReceiptRecognizeService : IReceiptRecognizeService
    {
        private ILogger _logger;
        private DocumentIntelligenceClient _client;
        public AzureReceiptRecognizeService(ILogger<AzureReceiptRecognizeService> logger)
        {
            _logger = logger;

            _logger.Log(LogLevel.Information, "Init ReceiptRecognizeService..");
            string? key = Environment.GetEnvironmentVariable("AZURE_DOCUMENT_API_KEY");
            string? endpoint = Environment.GetEnvironmentVariable("AZURE_DOCUMENT_API_ENDPOINT");

            if (key is null || endpoint is null)
            {
                _logger.Log(LogLevel.Critical, "Azure API Key or Endpoint is null. Check environment variables.");
                throw new ArgumentException();
            }

            var credential = new AzureKeyCredential(key);
            _client = new DocumentIntelligenceClient(new Uri(endpoint), credential);
        }

        public List<ReceiptItem>? ImportFromReceipt(string taskId, IFormFile image)
        {
            _logger.LogInformation($"#{taskId}: New ImportFromReceiptTask.");
            List<ReceiptItem> results = new List<ReceiptItem>();
            
            using var stream = image.OpenReadStream();
            using var ms = new MemoryStream();
            stream.CopyTo(ms);

            try
            {
                _logger.LogInformation($"#{taskId}: Request to Azure API..");
                var content = new AnalyzeDocumentContent() { Base64Source = BinaryData.FromBytes(ms.ToArray()) };
                var operation = _client.AnalyzeDocument(WaitUntil.Completed, "prebuilt-receipt", content);
                var result = operation.Value;

                if (result.Documents.Count == 0)
                {
                    _logger.LogWarning($"#{taskId}: Azure api returns no documents!");
                    throw new Exception();
                }
                var document = result.Documents[0];

                foreach (var item in document.Fields["Items"].ValueList)
                {
                    var x = item.ValueDictionary;
                    if(!x.Keys.Contains("Description") || !x.ContainsKey("Quantity")) continue;
                    string description = x["Description"].Content;
                    string quantity = x["Quantity"].Content;

                    results.Add(new ReceiptItem()
                    {
                        categoryId = 1,
                        itemDescription = description,
                        itemQuantity = Int32.Parse(quantity)
                    });
                }
                _logger.LogInformation($"#{taskId}: Complete.");
            }
            catch (Exception e)
            {
                _logger.LogWarning($"#{taskId}: Exception in ImportFromReceipt: " + e.ToString());
                return null;
            }

            return results;

        }
    }
}
