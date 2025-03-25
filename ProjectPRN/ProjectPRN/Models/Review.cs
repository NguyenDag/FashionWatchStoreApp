using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int? ProductId { get; set; }

    public int? AccountId { get; set; }

    public string? ReviewText { get; set; }

    public int? ReviewRating { get; set; }

    public DateTime? ReviewDate { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Product? Product { get; set; }
}
