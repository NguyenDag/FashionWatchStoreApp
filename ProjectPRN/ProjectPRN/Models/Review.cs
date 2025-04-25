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

    public string ReviewStarsFormatted
    {
        get
        {
            int rating = ReviewRating ?? 0; // Nếu null, mặc định 0 sao
            return new string('★', rating) + new string('☆', 5 - rating);
        }
    }
}
