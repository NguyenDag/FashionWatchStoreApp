using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace ProjectPRN.Models
{
    public class SecurityHelper
    {
        //Mã hóa mật khẩu với BCrypt
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12); // Work factor = 12: số vòng lặp của thuật toán băm
        }

        //Kiểm tra mật khẩu trước khi đăng nhập
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
