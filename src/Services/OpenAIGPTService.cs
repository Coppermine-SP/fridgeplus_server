using OpenAI;
namespace fridgeplus_server.Services
{
    public class OpenAIGPTService : IChatCompletionService
    {
        private ILogger _logger;

        public OpenAIGPTService(ILogger<OpenAIGPTService> logger)
        {
            _logger = logger;

            _logger.Log(LogLevel.Information, "Init OpenAIGPTService..");
            string? key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (key is null)
            {
                _logger.Log(LogLevel.Critical, "OpenAI API Key is null. Check environment variables.");
                throw new ArgumentException();
            }


        }
    }
}
