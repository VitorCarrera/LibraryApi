﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Models
{
    [Table("Genres")]
    public class Genre
    {
        public Genre()
        {
            Books = new Collection<Book>();
        }
        [Key]
        public int GenreId { get; set; }
        [Required]
        [StringLength(80)]
        public string? Name { get; set; }
        [Required]
        [StringLength(300)]
        public string? UrlImage { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
