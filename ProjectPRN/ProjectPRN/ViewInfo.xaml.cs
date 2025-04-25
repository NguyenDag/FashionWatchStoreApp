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
                    .Include(a => a.AccountRank)  
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
                    if (!string.IsNullOrEmpty(user.Address))
                    {
                        List<string> addressList = user.Address.Split('|').ToList();
                        cbAddress.ItemsSource = addressList;
                        if (!addressList.Contains("Other"))
                        {
                            addressList.Add("Other");
                        }
                        cbAddress.SelectedItem = addressList.FirstOrDefault();
                        txtAddress.Text = cbAddress.SelectedItem?.ToString();
                    }

                    // Gán ảnh (nếu có)
                    // imgAvatar.Source = new BitmapImage(new Uri(user.Avatar));  // Giả sử Avatar là một đường dẫn đến ảnh
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Gán giá trị từ TextBlock sang TextBox trước khi hiển thị ô nhập
            tbName.Text = txtName.Text;
            tbEmail.Text = txtEmail.Text;
            tbPhone.Text = txtPhone.Text;
            // Chuyển địa chỉ từ txtAddress sang cbAddress
            if (!string.IsNullOrEmpty(txtAddress.Text))
            {
                cbAddress.SelectedItem = txtAddress.Text;
            }

            // Chuyển đổi ngày sinh từ dd/MM/yyyy sang DatePicker
            if (DateTime.TryParseExact(txtDob.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                dpDob.SelectedDate = parsedDate;
            }
            else
            {
                dpDob.SelectedDate = null; // Nếu lỗi, để rỗng
            }

            // Xử lý giới tính (ComboBox)
            foreach (ComboBoxItem item in cbGender.Items)
            {
                if (item.Content.ToString() == txtGender.Text)
                {
                    cbGender.SelectedItem = item;
                    break;
                }
            }
            // Hiển thị các ô nhập liệu
            txtName.Visibility = Visibility.Collapsed;
            tbName.Visibility = Visibility.Visible;

            txtEmail.Visibility = Visibility.Collapsed;
            tbEmail.Visibility = Visibility.Visible;

            txtDob.Visibility = Visibility.Collapsed;
            dpDob.Visibility = Visibility.Visible;

            txtAddress.Visibility = Visibility.Collapsed;
            cbAddress.Visibility = Visibility.Visible;

            txtPhone.Visibility = Visibility.Collapsed;
            tbPhone.Visibility = Visibility.Visible;

            txtGender.Visibility = Visibility.Collapsed;
            cbGender.Visibility = Visibility.Visible;


            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
            btnChangePassword.Visibility = Visibility.Collapsed;

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
                var user = context.Accounts.FirstOrDefault(a => a.AccountId == accountId);

                if (user != null)
                {
                    // Cập nhật thông tin từ các TextBox
                    user.Fullname = tbName.Text;
                    user.Email = email;
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

                    // Xử lý địa chỉ
                    bool isAddressUpdated = false;

                    if (cbAddress.SelectedItem != null)
                    {
                        string selectedAddress = cbAddress.SelectedItem.ToString();

                        // Lấy danh sách địa chỉ hiện có (tách từ chuỗi user.Address)
                        List<string> addressList = string.IsNullOrEmpty(user.Address)
                            ? new List<string>()
                            : user.Address.Split('|').ToList();

                        if (selectedAddress == "Other")
                        {
                            string newAddress = txtOtherAddress.Text.Trim();

                            // Kiểm tra nếu địa chỉ mới hợp lệ
                            if (string.IsNullOrEmpty(newAddress))
                            {
                                MessageBox.Show("Vui lòng nhập địa chỉ mới!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            // Kiểm tra nếu địa chỉ mới đã tồn tại
                            if (addressList.Contains(newAddress))
                            {
                                MessageBox.Show("Địa chỉ này đã tồn tại, vui lòng nhập địa chỉ khác!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                            // Thêm địa chỉ mới vào danh sách
                            addressList.Add(newAddress);
                            user.Address = string.Join("|", addressList); // Lưu vào cơ sở dữ liệu

                            // Cập nhật ComboBox (không bị lỗi ItemsSource)
                            cbAddress.ItemsSource = null;
                            cbAddress.ItemsSource = addressList.Concat(new List<string> { "Other" }).ToList(); // Luôn giữ "Other"
                            cbAddress.SelectedItem = newAddress; 
                            txtAddress.Text = newAddress; // Cập nhật hiển thị

                            isAddressUpdated = true;
                        }
                        else if (user.Address != selectedAddress)
                        {
                            user.Address = selectedAddress; // Chỉ cập nhật nếu chọn địa chỉ khác
                            isAddressUpdated = false;
                        }
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu nếu có cập nhật
                    if (isAddressUpdated)
                    {
                        context.SaveChanges();
                    }


                    MessageBox.Show("Thông tin đã được cập nhật!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Hiển thị thông tin mới
                    txtName.Text = user.Fullname;
                    txtEmail.Text = user.Email;
                    txtDob.Text = user.Dob?.ToString("dd/MM/yyyy");
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
            cbAddress.Visibility = Visibility.Collapsed;
            txtOtherAddress.Visibility = Visibility.Collapsed;

            txtPhone.Visibility = Visibility.Visible;
            tbPhone.Visibility = Visibility.Collapsed;

            txtGender.Visibility = Visibility.Visible;
            cbGender.Visibility = Visibility.Collapsed;

            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
            btnChangePassword.Visibility = Visibility.Visible;

        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ đổi mật khẩu
            ChangePassword changePasswordWindow = new ChangePassword(accountId);
            changePasswordWindow.ShowDialog();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            // Chuyển về chế độ chỉ hiển thị
            txtName.Visibility = Visibility.Visible;
            tbName.Visibility = Visibility.Collapsed;

            txtEmail.Visibility = Visibility.Visible;
            tbEmail.Visibility = Visibility.Collapsed;

            txtDob.Visibility = Visibility.Visible;
            dpDob.Visibility = Visibility.Collapsed;

            txtAddress.Visibility = Visibility.Visible;
            cbAddress.Visibility = Visibility.Collapsed;
            txtOtherAddress.Visibility = Visibility.Collapsed;


            txtPhone.Visibility = Visibility.Visible;
            tbPhone.Visibility = Visibility.Collapsed;

            txtGender.Visibility = Visibility.Visible;
            cbGender.Visibility = Visibility.Collapsed;

            btnEdit.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
            btnChangePassword.Visibility = Visibility.Visible;
        }

        private void cbAddress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbAddress.SelectedItem != null)
            {
                string selectedAddress = cbAddress.SelectedItem.ToString();

                if (selectedAddress == "Other")
                {
                    txtOtherAddress.Visibility = Visibility.Visible;
                    txtOtherAddress.Text = "";
                }
                else
                {
                    txtOtherAddress.Visibility = Visibility.Collapsed;
                    txtAddress.Text = selectedAddress; // Hiển thị địa chỉ đã chọn
                }
            }
        }


    }

}
