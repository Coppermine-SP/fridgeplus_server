﻿using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Mysqlx.Datatypes;
using OpenAI;
using OpenAI.Chat;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace fridgeplus_server.Services
{
    public class OpenAIGPTService : IChatCompletionService
    {
        private readonly ILogger _logger;
        private readonly ChatClient _chatClient;
        private readonly Dictionary<Type, BinaryData> _schemaDictionary;
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        private BinaryData _getSchemaFromCache(Type type)
        {
            if (_schemaDictionary.TryGetValue(type, out BinaryData? value)) return value;
            
            _logger.Log(LogLevel.Information, $"Type {type} is not in json schema cache! Generating json schema...");
                
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(type);
            schema.AdditionalProperties = JSchema.Parse("false");

            BinaryData binary = BinaryData.FromString(schema.ToString());
            _schemaDictionary.Add(type, binary);
            return binary;
        }
        
        public OpenAIGPTService(ILogger<OpenAIGPTService> logger)
        {
            _logger = logger;

            _logger.Log(LogLevel.Information, "Init OpenAIGPTService..");
            string? key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            string? model = Environment.GetEnvironmentVariable("OPENAI_API_MODEL");

            if (key is null || model is null)
            {
                _logger.Log(LogLevel.Critical, "OpenAI API Key or Model is null. Check environment variables.");
                throw new ArgumentException();
            }

            var client = new OpenAIClient(key);
            _chatClient = client.GetChatClient(model);
            _schemaDictionary = new Dictionary<Type, BinaryData>();
        }

        public JsonDocument? ChatCompletion(string taskId, string prompt, object data, Type responseFormat)
        {
            _logger.LogInformation($"#{taskId}: New ChatCompletionTask.");
            
            try
            {
                ChatMessage[] messages =
                {
                    new SystemChatMessage(prompt),
                    new UserChatMessage(JsonSerializer.Serialize(data, _serializerOptions))
                };
                
                ChatCompletionOptions options = new()
                {
                    ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                        jsonSchemaFormatName: responseFormat.Name,
                        jsonSchemaIsStrict: false,
                        jsonSchema: _getSchemaFromCache(responseFormat))
                };

                _logger.LogInformation($"#{taskId}: Waiting response from OpenAI..");
                var completion = _chatClient.CompleteChat(messages, options);

                _logger.LogInformation($"#{taskId}: Complete.");
                return JsonDocument.Parse(completion.Value.Content[0].Text.ToString());

            }
            catch (Exception e)
            {
                _logger.LogWarning($"#{taskId}: Exception in ChatCompletion: " + e.ToString());
                return null;
            }
        }
    
    }
}
