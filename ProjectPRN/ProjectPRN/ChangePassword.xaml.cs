using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using ProjectPRN.Models;

namespace ProjectPRN
{
    public partial class ChangePassword : Window
    {
        private int accountId; 

        public ChangePassword(int id)
        {
            InitializeComponent();
            accountId = id;  
        }

        private void btnSavePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = pbOldPassword.Password;
            string newPassword = pbNewPassword.Password;
            string confirmPassword = pbConfirmPassword.Password;

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu mới không khớp!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra độ mạnh của mật khẩu
            if (!IsValidPassword(newPassword))
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự, chứa ít nhất 1 chữ in hoa và 1 chữ số!",
                    "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new ProjectPrnContext())
            {
                var user = context.Accounts.FirstOrDefault(a => a.AccountId == accountId);
                if (user != null)
                {
                    // Kiểm tra mật khẩu cũ
                    if (!SecurityHelper.VerifyPassword(oldPassword, user.Password))
                    {
                        MessageBox.Show("Mật khẩu hiện tại không đúng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }


                    // Cập nhật mật khẩu mới
                    user.Password = SecurityHelper.HashPassword(newPassword);
                    context.SaveChanges();
                    MessageBox.Show("Mật khẩu đã được cập nhật!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    MessageBox.Show("Tài khoản không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d).{8,}$");
        }
    }
}
