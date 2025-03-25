using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string BrandName { get; set; } = null!;

    public string? BrandAddress { get; set; }

    public string? BrandPhone { get; set; }

    public string? BrandEmail { get; set; }

    public bool? BrandStatus { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
