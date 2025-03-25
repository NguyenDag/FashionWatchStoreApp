using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        private DispatcherTimer countdownTimer;
        private int countdownSeconds = 60;
        private string currentEmail;
        private string generatedCode; // Mã xác nhận được tạo
        public ForgotPasswordWindow()
        {
            InitializeComponent();
            InitializeCountdownTimer();
        }
        private void InitializeCountdownTimer()
        {
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            countdownSeconds--;
            txtCountdown.Text = countdownSeconds.ToString();

            if (countdownSeconds <= 0)
            {
                countdownTimer.Stop();
                lnkResendCode.IsEnabled = true;
            }
        }

        private void StartCountdown()
        {
            countdownSeconds = 60;
            txtCountdown.Text = countdownSeconds.ToString();
            lnkResendCode.IsEnabled = false;
            countdownTimer.Start();
        }

        private void ShowLoadingState(bool isLoading)
        {
            gridLoading.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            gridSteps.IsEnabled = !isLoading;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }

        private string GenerateRandomCode()
        {
            // Tạo mã xác nhận ngẫu nhiên 6 chữ số
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        // Giả lập việc gửi mã xác nhận qua email
        private void SendVerificationCode(string emailOrUsername)
        {
            // Trong thực tế, bạn sẽ kiểm tra xem email/tên đăng nhập có tồn tại không
            // và gửi mã thông qua dịch vụ email

            // Giả lập việc kiểm tra email trong cơ sở dữ liệu
            bool userExists = true; // Giả định người dùng tồn tại

            if (!userExists)
            {
                MessageBox.Show("Email hoặc tên đăng nhập không tồn tại trong hệ thống!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Tạo mã xác nhận
            generatedCode = GenerateRandomCode();
            currentEmail = emailOrUsername;

            // Trong thực tế, bạn sẽ gửi mã này qua email tại đây
            // EmailService.SendVerificationCode(emailOrUsername, generatedCode);

            // Hiển thị thông báo cho mục đích demo
            MessageBox.Show($"Mã xác nhận: {generatedCode}\n\n(Trong ứng dụng thực tế, mã này sẽ được gửi đến email của người dùng)",
                "Thông tin", MessageBoxButton.OK, MessageBoxImage.Information);

            // Bắt đầu đếm ngược để gửi lại mã
            StartCountdown();
        }

        // Xử lý sự kiện nút Gửi mã xác nhận
        private void btnSendCode_Click(object sender, RoutedEventArgs e)
        {
            string emailOrUsername = txtEmailOrUsername.Text.Trim();

            if (string.IsNullOrEmpty(emailOrUsername))
            {
                MessageBox.Show("Vui lòng nhập email hoặc tên đăng nhập!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Hiển thị trạng thái loading
            ShowLoadingState(true);

            // Giả lập thời gian xử lý
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                timer.Stop();

                // Gửi mã xác nhận
                SendVerificationCode(emailOrUsername);

                // Chuyển sang bước 2
                borderStep1.Visibility = Visibility.Collapsed;
                borderStep2.Visibility = Visibility.Visible;

                // Ẩn trạng thái loading
                ShowLoadingState(false);
            };
            timer.Start();
        }

        // Xử lý sự kiện nút Gửi lại mã
        private void lnkResendCode_Click(object sender, RoutedEventArgs e)
        {
            if (lnkResendCode.IsEnabled)
            {
                // Hiển thị trạng thái loading
                ShowLoadingState(true);

                // Giả lập thời gian xử lý
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += (s, args) =>
                {
                    timer.Stop();

                    // Tạo mã xác nhận mới
                    generatedCode = GenerateRandomCode();

                    // Hiển thị thông báo cho mục đích demo
                    MessageBox.Show($"Mã xác nhận mới: {generatedCode}\n\n(Trong ứng dụng thực tế, mã này sẽ được gửi đến email của người dùng)",
                        "Thông tin", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Bắt đầu đếm ngược để gửi lại mã
                    StartCountdown();

                    // Ẩn trạng thái loading
                    ShowLoadingState(false);
                };
                timer.Start();
            }
        }

        // Xử lý sự kiện nút Xác nhận mã
        private void btnVerifyCode_Click(object sender, RoutedEventArgs e)
        {
            string code = txtVerificationCode.Text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Vui lòng nhập mã xác nhận!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Hiển thị trạng thái loading
            ShowLoadingState(true);

            // Giả lập thời gian xử lý
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, args) =>
            {
                timer.Stop();

                // Kiểm tra mã xác nhận
                if (code == generatedCode)
                {
                    // Mã xác nhận hợp lệ, chuyển sang bước 3
                    borderStep2.Visibility = Visibility.Collapsed;
                    borderStep3.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Mã xác nhận không chính xác!",
                        "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Ẩn trạng thái loading
                ShowLoadingState(false);
            };
            timer.Start();
        }

        // Xử lý sự kiện nút Đặt lại mật khẩu
        private void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmNewPassword.Password;

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!",
                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Hiển thị trạng thái loading
            ShowLoadingState(true);

            // Giả lập thời gian xử lý
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, args) =>
            {
                timer.Stop();

                // Trong thực tế, bạn sẽ cập nhật mật khẩu trong cơ sở dữ liệu
                // UserService.UpdatePassword(currentEmail, newPassword);

                // Chuyển sang bước 4
                borderStep3.Visibility = Visibility.Collapsed;
                borderStep4.Visibility = Visibility.Visible;

                // Ẩn trạng thái loading
                ShowLoadingState(false);
            };
            timer.Start();
        }

        // Xử lý sự kiện nút Quay lại đăng nhập
        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ hiện tại
            this.Close();
        }

        // Xử lý sự kiện nút Quay lại (Bước 2 -> Bước 1)
        private void btnBackStep1_Click(object sender, RoutedEventArgs e)
        {
            // Dừng đếm ngược nếu đang hoạt động
            if (countdownTimer.IsEnabled)
            {
                countdownTimer.Stop();
            }

            borderStep2.Visibility = Visibility.Collapsed;
            borderStep1.Visibility = Visibility.Visible;
        }

        // Xử lý sự kiện nút Quay lại (Bước 3 -> Bước 2)
        private void btnBackStep2_Click(object sender, RoutedEventArgs e)
        {
            borderStep3.Visibility = Visibility.Collapsed;
            borderStep2.Visibility = Visibility.Visible;
        }

        // Xử lý sự kiện nút Hủy
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ hiện tại
            this.Close();
        }
    }
}
