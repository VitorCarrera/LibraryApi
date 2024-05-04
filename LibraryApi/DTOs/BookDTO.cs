using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs
{
    public class BookDTO
    {

        public int BookId { get; set; }
        [Required]
        [StringLength(80)]
        public string? Name { get; set; }
        [Required]
        [StringLength(300)]
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [StringLength(300)]
        public string? UrlImage { get; set; }
        public int GenreId { get; set; }

    }
}
