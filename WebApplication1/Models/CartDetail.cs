namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CartDetail")]
    public partial class CartDetail
    {
        public int CartDetailID { get; set; }

        public int? CartID { get; set; }

        public int? ProductID { get; set; }

        public int? Quantity { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual Product Product { get; set; }
    }
}
