using System;
using System.Collections.Generic;

namespace demko140126.Models;

public partial class ProductSale
{
    public int Id { get; set; }

    public int? AgentId { get; set; }

    public int? ProductId { get; set; }

    public DateOnly SaleDate { get; set; }

    public int ProductCount { get; set; }

    public virtual Agent? Agent { get; set; }

    public virtual Product? Product { get; set; }
}
