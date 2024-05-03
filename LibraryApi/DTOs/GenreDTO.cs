using System.ComponentModel.DataAnnotations;

namespace LibraryApi.DTOs
{
    public class GenreDTO
    {


        public int GenreId { get; set; }
        [Required]
        [StringLength(80)]
        public string? Name { get; set; }
        [Required]
        [StringLength(300)]
        public string? UrlImage { get; set; }
    }
}
