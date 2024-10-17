using System;
using System.ComponentModel.DataAnnotations;

namespace LMS_LibraryManagementSystem_.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Author { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Genre { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }
    }
}
