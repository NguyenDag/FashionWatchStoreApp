using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? AccountId { get; set; }

    public DateTime? OrderDate { get; set; }

    public decimal? TotalCost { get; set; }

    public decimal? ActualCost { get; set; }

    public string? Status { get; set; }

    public int? PaymentType { get; set; }

    public int? ShipId { get; set; }

    public DateTime? ShipDate { get; set; }

    public DateTime? ReceiveDate { get; set; }

    public string? ReceiveName { get; set; }

    public string? ReceivePhone { get; set; }

    public string? ReceiveAddress { get; set; }

    public int? DiscountId { get; set; }

    public string? Note { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Shipping? Ship { get; set; }
}
