using System.ComponentModel.DataAnnotations;

namespace BookCatalog.API.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Range(1900, 2100)]
        public int BirthYear { get; set; }
    }
}
