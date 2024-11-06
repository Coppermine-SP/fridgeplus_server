using fridgeplus_server.Models;

namespace fridgeplus_server.Services
{
    public interface IReceiptRecognizeService
    {
        public List<Item> ImportFromReceipt(IFormFile image);
    }

}
