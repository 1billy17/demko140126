using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;

namespace demko140126.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int? ProductTypeId { get; set; }

    public string ArticleNumber { get; set; } = null!;

    public string? Image { get; set; }
    
    public Bitmap ParseImage => new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + Image);

    public int? ProductionPersonCount { get; set; }

    public int? ProductionWorkshopNumber { get; set; }

    public decimal MinCostForAgent { get; set; }
    
    public decimal CalculatedCost
    {
        get
        {
            if (ProductMaterials == null)
                return 0;

            return ProductMaterials
                .Where(pm => pm.Material != null && pm.Count != null)
                .Sum(pm => pm.Material!.Cost * pm.Count!.Value);
        }
    }
    
    public string MaterialsLine
    {
        get
        {
            if (ProductMaterials == null)
                return string.Empty;

            return string.Join(", ",
                ProductMaterials
                    .Where(pm => pm.Material != null)
                    .Select(pm => pm.Material!.Title));
        }
    }

    public virtual ICollection<ProductCostHistory> ProductCostHistories { get; set; } = new List<ProductCostHistory>();

    public virtual ICollection<ProductMaterial> ProductMaterials { get; set; } = new List<ProductMaterial>();

    public virtual ICollection<ProductSale> ProductSales { get; set; } = new List<ProductSale>();

    public virtual ProductType? ProductType { get; set; }
}
