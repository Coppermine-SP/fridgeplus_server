using fridgeplus_server.Models;

namespace fridgeplus_server.Services
{
    public interface IReceiptRecognizeService
    {
        public record ReceiptItem(int categoryId, string? itemDescription, int itemQuantity);
        public IEnumerable<ReceiptItem>? ImportFromReceipt(string taskId, IFormFile image);
    }

}
