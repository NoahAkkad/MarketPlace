using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(30)]
		[DisplayName("Product Name")]
		public string? Name { get; set; }

		[DisplayName("Product Price")]
		[Range(1,1000000, ErrorMessage = "Price must be between 1-1000000")]
		public int Price { get; set; }


        // Foreign key property
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        // Navigation property
        public Category? Category { get; set; }

    }
	
}

