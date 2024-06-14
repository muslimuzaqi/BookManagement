using System.ComponentModel.DataAnnotations;

namespace BookMVC.Models
{
    public class BookDto
    {
        public string Title { get; set; } = " ";
        [Required, MaxLength(100)]
        public string Description { get; set; } = " ";
        [Required, MaxLength(100)]
        public string Author { get; set; } = " ";
        [Required,MaxLength(100)]
        public string Brand { get; set; } = " ";
        [Required,MaxLength(100)]

        public string Category { get; set; } = " ";

        [   Required]
        public string Price { get; set; } = " ";
      

        public IFormFile? ImageFileName { get; set; } 
    }
}
