using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksSpring2024_sec02.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [DisplayName("Category Name: ")]
        [Required(ErrorMessage ="The name for the category MUST be provided")]
        public string Name { get; set; }

        [DisplayName("Category Description: ")]
        [Required(ErrorMessage = "The category description MUST be provided")]
        public string Description { get; set; }

    }
}
