using System;
using System.Collections.Generic;

namespace demko140126.Models;

public partial class Material
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int CountInPack { get; set; }

    public string Unit { get; set; } = null!;

    public int? CountInStock { get; set; }

    public int? MinCount { get; set; }

    public decimal Cost { get; set; }

    public string? Image { get; set; }

    public int? MaterialTypeId { get; set; }

    public virtual ICollection<MaterialCountHistory> MaterialCountHistories { get; set; } = new List<MaterialCountHistory>();

    public virtual ICollection<MaterialSupplier> MaterialSuppliers { get; set; } = new List<MaterialSupplier>();

    public virtual MaterialType? MaterialType { get; set; }

    public virtual ICollection<ProductMaterial> ProductMaterials { get; set; } = new List<ProductMaterial>();
}
