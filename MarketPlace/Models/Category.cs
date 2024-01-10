using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
	public class Category
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string? Name { get; set; }
       
        public ICollection<Product> Products { get; set; } = new List<Product>();
    } 
}

