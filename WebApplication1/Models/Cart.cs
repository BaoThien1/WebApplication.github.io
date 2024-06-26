using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebApplication1.Models;

[Table("Cart")]
public partial class Cart
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Cart()
    {
        CartDetails = new HashSet<CartDetail>();
    }

    public int CartID { get; set; }

    public int? CustomerID { get; set; }

    public DateTime? CreatedDate { get; set; }

    public decimal? Total { get; set; }

    public virtual Customer Customer { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public virtual ICollection<CartDetail> CartDetails { get; set; }

    
}
