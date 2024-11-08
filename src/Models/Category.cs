using System.ComponentModel.DataAnnotations;

namespace fridgeplus_server.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string? CategoryName { get; set; }

        public TimeSpan? Expires { get; set; }

    }
}
