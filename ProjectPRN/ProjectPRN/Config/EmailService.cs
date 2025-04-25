using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectPRN.Config
{
    internal class EmailService
    {
        //internal static void SendVerificationCode(string email, string generatedCode)
        //{
        //    throw new NotImplementedException();
        //}

        public static void SendVerificationCode(string email, string newPassword)
        {
            try
            {
                string fromEmail = "hongphongbs7b@gmail.com"; // Email của bạn
                string fromPassword = "beek ewdu clto sxhl"; // Mật khẩu ứng dụng (App Password)

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = "Mật khẩu mới của bạn",
                    Body = $"Mật khẩu mới của bạn là: {newPassword}\nHãy đổi lại mật khẩu sau khi đăng nhập!",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi email thất bại: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
