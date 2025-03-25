using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using ProjectPRN.Models;
namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private string selectedAvatarPath;
        private readonly ProjectPrnContext context = new ProjectPrnContext();
        public RegisterWindow()
        {
            InitializeComponent();
            cboGender.SelectedIndex = 0;
        }
        private void btnChooseAvatar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Tệp hình ảnh|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.Title = "Chọn ảnh đại diện";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Lấy tên file
                    string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);

                    // Tạo thư mục lưu ảnh nếu chưa có
                    string appFolder = AppDomain.CurrentDomain.BaseDirectory;
                    string imagesFolder = System.IO.Path.Combine(appFolder, "Images", "Avatars");
                    if (!Directory.Exists(imagesFolder))
                    {
                        Directory.CreateDirectory(imagesFolder);
                    }

                    // Copy ảnh vào thư mục của ứng dụng
                    string newFilePath = System.IO.Path.Combine(imagesFolder, fileName);

                    // Giải phóng ảnh cũ trước khi ghi đè
                    imgAvatar.Source = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    File.Copy(openFileDialog.FileName, newFilePath, true);

                    // Lưu đường dẫn tương đối vào biến (chỉ lưu "Images/Avatars/fileName")
                    selectedAvatarPath = System.IO.Path.Combine("Images", "Avatars", fileName);

                    // Hiển thị ảnh mới
                    SetImageSource(newFilePath);
                    txtNoAvatar.Visibility = Visibility.Collapsed;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể tải ảnh: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SetImageSource(string imagePath)
        {
            BitmapImage image = new BitmapImage();
            using (FileStream stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // Load vào bộ nhớ và giải phóng file
                image.StreamSource = stream;
                image.EndInit();
            }
            imgAvatar.Source = image;
        }

        private void btnRemoveAvatar_Click(object sender, RoutedEventArgs e)
        {
            imgAvatar.Source = null;
            txtNoAvatar.Visibility = Visibility.Visible;
            selectedAvatarPath = null;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(txtUsername.Text) ||
                string.IsNullOrEmpty(txtPassword.Password) ||
                string.IsNullOrEmpty(txtFullName.Text) ||
                string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các thông tin bắt buộc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Kiểm tra tài khoản này tồn tại chưa
            if (IsExitsAccount(txtUsername.Text))
            {
                MessageBox.Show("Tài khoản đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Focus();
                return;
            }

            // Kiểm tra mật khẩu và xác nhận mật khẩu
            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtConfirmPassword.Focus();
                return;
            }

            //Kiểm tra email đã được đăng ký
            if (IsExitsEmail(txtEmail.Text))
            {
                MessageBox.Show("Email đã được đăng ký!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return;
            }

            // Kiểm tra định dạng email
            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Định dạng email không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return;
            }

            //Kiểm tra phone đã được đăng ký
            if (IsExitsPhone(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại đã được đăng ký!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhone.Focus();
                return;
            }

            // Kiểm tra định dạng số điện thoại
            if (!IsValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Định dạng số điện thoại không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhone.Focus();
                return;
            }

            // Kiểm tra điều khoản
            if (chkAgree.IsChecked != true)
            {
                MessageBox.Show("Bạn cần đồng ý với các điều khoản và điều kiện để tiếp tục!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                chkAgree.Focus();
                return;
            }
            string hashedPassword = SecurityHelper.HashPassword(txtPassword.Password);
            // Lấy thông tin người dùng
            Account account = new Account()
            {
                Username = txtUsername.Text,
                Password = hashedPassword, // cần mã hóa mật khẩu
                Role = 1,
                Fullname = txtFullName.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,
                Dob = DateOnly.FromDateTime(dpDob.SelectedDate.Value),
                Gender = cboGender.SelectedItem.ToString() == "Nam" ? true : false,
                Address = txtAddress.Text,
                Status = true,
                Avatar = selectedAvatarPath,
            };

            // Lưu thông tin người dùng vào cơ sở dữ liệu
            context.Accounts.Add(account);
            context.SaveChanges();

            // Giả lập đăng ký thành công
            MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Chuyển đến màn hình đăng nhập
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private bool IsExitsAccount(string username)
        {
            return context.Accounts.FirstOrDefault(x => x.Username == username) != null;
        }
        private bool IsExitsEmail(string email)
        {
            return context.Accounts.FirstOrDefault(x => x.Email == email) != null;
        }
        private bool IsExitsPhone(string phone)
        {
            return context.Accounts.FirstOrDefault(x => x.Phone == phone) != null;
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(0|\+84)(\d{9,10})$";
            return Regex.IsMatch(phoneNumber, pattern);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn hủy đăng ký?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Quay lại màn hình đăng nhập
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        private void lnkLogin_Click(object sender, RoutedEventArgs e)
        {
            // Chuyển đến màn hình đăng nhập
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}
