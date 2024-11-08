using System.Text.Json;
using System.Text.Json.Nodes;
using OpenAI.Chat;
using OpenAI.Models;

namespace fridgeplus_server.Services
{
    public interface IChatCompletionService
    {
        public JsonDocument? ChatCompletion(string taskId, string prompt, object data, Type responseFormat);
    }
}
