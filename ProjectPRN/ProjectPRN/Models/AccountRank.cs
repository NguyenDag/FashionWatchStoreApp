using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class AccountRank
{
    public int AccountRankId { get; set; }

    public string? AccountRankName { get; set; }

    public decimal? MinAmount { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
