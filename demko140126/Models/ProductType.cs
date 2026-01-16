using System;
using System.Collections.Generic;

namespace demko140126.Models;

public partial class ProductType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
