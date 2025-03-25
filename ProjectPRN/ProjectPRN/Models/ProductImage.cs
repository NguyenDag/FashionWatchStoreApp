﻿using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class ProductImage
{
    public int ImageId { get; set; }

    public int? ProductId { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsMain { get; set; }

    public bool? Status { get; set; }

    public virtual Product? Product { get; set; }
}
