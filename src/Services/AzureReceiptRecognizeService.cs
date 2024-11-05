using Azure;
using Azure.AI.DocumentIntelligence;

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
    }
}
