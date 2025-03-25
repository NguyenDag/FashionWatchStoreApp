using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectPRN.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public int BrandId { get; set; }

    public string? ProductDescription { get; set; }

    public int Quantity { get; set; }

    public decimal PriceBuy { get; set; }

    public decimal PriceSell { get; set; }

    public int? Status { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();


}
