using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProjectPRN.Models
{
    public static class SessionManager
    {
        public static int AccountId { get; set; }
        public static int? Role { get; set; }

        public static string? Username { get; set; } = null;

        //public static bool? Status { get; set; }

        public static bool IsLoggedIn => AccountId > 0;

        public static Account account { get => ProjectPrnContext.Ins.Accounts.Find(AccountId); }

        public static void Logout()
        {
            AccountId = 0;
            Role = null;
            Username = null;
        }

    }
}
