namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Invoice")]
    public class Invoice
    {
        public int InvoiceID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Shipping Address")]
        public string ShipAddress { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string Phone { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [Display(Name = "Date Created")]
        // Navigation property for related entities
        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
        // Navigation property for related entities

        // Navigation property to customer
        public Customer Customer { get; set; }
    }

}