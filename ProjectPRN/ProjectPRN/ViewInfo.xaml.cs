using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjectPRN.Models;

namespace ProjectPRN
{
    public partial class ViewInfo : Window
    {
        private int accountId;

        public ViewInfo()
        {
            InitializeComponent();
            accountId = SessionManager.AccountId;
            LoadAccountInfo(accountId);  
        }

        private void LoadAccountInfo(int accountId)
        {
            using (var context = new ProjectPrnContext())
            {
                var user = context.Accounts
                    .Include(a => a.AccountRank)  // Tải thông tin về AccountRank
                    .FirstOrDefault(a => a.AccountId == accountId);

                if (user == null)
                {
                    MessageBox.Show("Không tìm thấy tài khoản!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                else
                {
                    // Gán dữ liệu vào các TextBlock
                    txtName.Text = user.Fullname;
                    txtEmail.Text = user.Email;
                    txtDob.Text = user.Dob?.ToString("dd/MM/yyyy"); 

                    txtAddress.Text = user.Address;
                    txtPhone.Text = user.Phone;
                    txtGender.Text = user.MyGender;

                    if (user.AccountRank != null)
                    {
                        txtRank.Text = user.AccountRank.AccountRankName;  
                    }
                    else
                    {
                        txtRank.Text = "Không có cấp bậc";  
                    }

                    // Gán ảnh (nếu có)
                    // imgAvatar.Source = new BitmapImage(new Uri(user.Avatar));  // Giả sử Avatar là một đường dẫn đến ảnh
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị các ô nhập liệu
            txtName.Visibility = Visibility.Collapsed;
            tbName.Visibility = Visibility.Visible;

            txtEmail.Visibility = Visibility.Collapsed;
            tbEmail.Visibility = Visibility.Visible;

            txtDob.Visibility = Visibility.Collapsed;
            dpDob.Visibility = Visibility.Visible;

            txtAddress.Visibility = Visibility.Collapsed;
            tbAddress.Visibility = Visibility.Visible;

            txtPhone.Visibility = Visibility.Collapsed;
            tbPhone.Visibility = Visibility.Visible;

            txtGender.Visibility = Visibility.Collapsed;
            cbGender.Visibility = Visibility.Visible;

            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string email = tbEmail.Text.Trim();
            string phone = tbPhone.Text.Trim();

            // Kiểm tra định dạng email
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$"))
            {
                MessageBox.Show("Email không hợp lệ! Email phải có đuôi @gmail.com.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra số điện thoại
            if (!Regex.IsMatch(phone, @"^0\d{9}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Số điện thoại phải có 10 số và bắt đầu bằng 0.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new ProjectPrnContext())
            {
                var user = context.Accounts.FirstOrDefault(a => a.AccountId == 2);

                if (user != null)
                {
                    // Cập nhật thông tin từ các TextBox
                    user.Fullname = tbName.Text;
                    user.Email = email;
                    user.Address = tbAddress.Text;
                    user.Phone = phone;

                    // Cập nhật ngày sinh từ DatePicker
                    if (dpDob.SelectedDate.HasValue)
                    {
                        user.Dob = DateOnly.FromDateTime(dpDob.SelectedDate.Value);
                    }

                    // Cập nhật giới tính từ ComboBox
                    var selectedGender = cbGender.SelectedItem as ComboBoxItem;
                    if (selectedGender != null)
                    {
                        user.Gender = bool.Parse(selectedGender.Tag.ToString());
                    }

                    context.SaveChanges();

                    MessageBox.Show("Thông tin đã được cập nhật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Hiển thị thông tin mới
                    txtName.Text = user.Fullname;
                    txtEmail.Text = user.Email;
                    txtDob.Text = user.Dob?.ToString("dd/MM/yyyy");
                    txtAddress.Text = user.Address;
                    txtPhone.Text = user.Phone;
                    txtGender.Text = user.MyGender;
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tài khoản!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Chuyển về chế độ chỉ hiển thị
            txtName.Visibility = Visibility.Visible;
            tbName.Visibility = Visibility.Collapsed;

            txtEmail.Visibility = Visibility.Visible;
            tbEmail.Visibility = Visibility.Collapsed;

            txtDob.Visibility = Visibility.Visible;
            dpDob.Visibility = Visibility.Collapsed;

            txtAddress.Visibility = Visibility.Visible;
            tbAddress.Visibility = Visibility.Collapsed;

            txtPhone.Visibility = Visibility.Visible;
            tbPhone.Visibility = Visibility.Collapsed;

            txtGender.Visibility = Visibility.Visible;
            cbGender.Visibility = Visibility.Collapsed;

            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Collapsed;
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ đổi mật khẩu
            ChangePassword changePasswordWindow = new ChangePassword(accountId);
            changePasswordWindow.ShowDialog();
        }

    }
}
