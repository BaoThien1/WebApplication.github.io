namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InvoiceDetail")]
    public partial class InvoiceDetail
    {
        public int InvoiceDetailID { get; set; }

        public int? InvoiceID { get; set; }

        public int? ProductID { get; set; }

        public int? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public virtual Invoice Invoice { get; set; }

        public virtual Product Product { get; set; }
    }
}
