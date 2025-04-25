using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Role { get; set; }

    public string Fullname { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateOnly? Dob { get; set; }

    public string? Avatar { get; set; }

    public bool? Gender { get; set; }

    public bool? Status { get; set; }

    public int? AccountRankId { get; set; }

    public virtual AccountRank? AccountRank { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public string RoleDisplay => Role == 0 ? "Admin" : "User";
    public string MyGender => Gender == false ? "Nam" : "Nữ";
    public string StatusDisplay => (bool)Status ? "Đang hoạt động" : "Vô hiệu hóa";

    public DateTime? DOB
    {
        get =>DateTime.Parse(Dob.Value.ToString()); 
    }

    

}
