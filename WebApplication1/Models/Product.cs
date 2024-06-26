namespace WebApplication1.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Product")]
    public partial class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [StringLength(5000)]
        public string ImageURL { get; set; }

        public int? CategoryID { get; set; }

        public decimal? SaleOff { get; set; }

        public int? Quantity { get; set; }

        public bool? BestSell { get; set; }

        public bool? IsHot { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }
}
