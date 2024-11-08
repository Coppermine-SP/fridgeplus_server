using fridgeplus_server.Models;

namespace fridgeplus_server.Services
{
    public interface IReceiptRecognizeService
    {
        public List<ReceiptItem> ImportFromReceipt(string taskId, IFormFile image);
    }

}
