using System;
using System.Collections.Generic;

namespace demko140126.Models;

public partial class MaterialType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
