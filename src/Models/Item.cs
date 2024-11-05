﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fridgeplus_server.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }

        [Required]
        public string ItemOwner { get; set; }

        public string? ItemDescription { get; set; }

        [Required]
        public DateTime? ItemImportDate { get; set; }

        public DateTime? ItemExpireDate { get; set; }
    }
}
