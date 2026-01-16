using System;
using System.Collections.Generic;

namespace demko140126.Models;

public partial class MaterialSupplier
{
    public int Id { get; set; }

    public int? MaterialId { get; set; }

    public int? SupplierId { get; set; }

    public virtual Material? Material { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
