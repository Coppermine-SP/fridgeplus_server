using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace fridgeplus_server.Models
{
    public record ReceiptItem
    {
        public int categoryId { get; set; }
        public string itemDescription { get; set; }
        public int itemQuantity { get; set; }
    }
}
