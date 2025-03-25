using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ProjectPRN.Models;
namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly ProjectPrnContext context = new ProjectPrnContext();
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            // Kiểm tra nhập liệu
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin đăng nhập!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Account account = AuthenticateUser(username, password);
            if (account == null)
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (account.Status == false)
            {
                MessageBox.Show("Tài khoản này tạm thời bị khóa!", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Lưu trạng thái đăng nhập
            SessionManager.AccountId = account.AccountId;
            SessionManager.Role = account.Role;

            MessageBox.Show($"Đăng nhập thành công! Vai trò: {account.Role}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Mở cửa sổ Home và cập nhật navbar
            Home mainWindow = new Home();
            mainWindow.Show();

            // Đóng cửa sổ LoginWindow
            this.Close();
        }


        private Account? AuthenticateUser(string username, string password)
        {
            // Trong thực tế, bạn sẽ kiểm tra thông tin đăng nhập từ cơ sở dữ liệu
            var account = context.Accounts.FirstOrDefault(x => x.Username == username);
            if (account != null && SecurityHelper.VerifyPassword(password, account.Password))
            {
                return account;
            }
            return null;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Mở form đăng ký
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void lnkForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            // Mở form khôi phục mật khẩu
            ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.ShowDialog();
        }
    }
}
