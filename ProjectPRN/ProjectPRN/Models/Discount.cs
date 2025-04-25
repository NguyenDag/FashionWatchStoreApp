using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string DiscountName { get; set; } = null!;

    public double Percent { get; set; }

    public int? AccountRankId { get; set; }

    public decimal? MinCost { get; set; }

    public DateOnly? EndDate { get; set; }

    public int Amount { get; set; }

    public bool? Status { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public string MyStatus => Status == true ? "Active" : "Inactive";

}
