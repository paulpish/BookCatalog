using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookCatalog.API.Models
{
    public class BookCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public List<Author> Authors { get; set; } = new List<Author>();

        [Required]
        [StringLength(100)]
        public string Publisher { get; set; }

        [StringLength(50)]
        public string Edition { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }
    }
}
