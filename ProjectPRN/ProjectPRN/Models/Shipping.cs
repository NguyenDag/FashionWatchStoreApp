using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class Shipping
{
    public int ShipId { get; set; }

    public string? ShipMethod { get; set; }

    public decimal? ShipCost { get; set; }

    public int? ShipTime { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
