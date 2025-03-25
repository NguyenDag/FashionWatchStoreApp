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
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;

namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for AccountManagementWindow.xaml
    /// </summary>
    public partial class AccountManagementWindow : Window
    {
        private readonly ProjectPrnContext context;
        private readonly List<Account> accounts;
        private Account selectedAccount;
        public AccountManagementWindow()
        {
            context = new ProjectPrnContext();
            InitializeComponent();
        }

        private void DgvAccounts_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDB();
        }

        private void DgvAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedAccount = DgvAccounts.SelectedItem as Account;
            if (selectedAccount != null)
            {
                TxtUsername.Text = selectedAccount.Username;
                TxtFullname.Text = selectedAccount.Fullname;
                TxtAddress.Text = selectedAccount.Address;
                TxtPhone.Text = selectedAccount.Phone;
                TxtEmail.Text = selectedAccount.Email;
                //DpkDob.SelectedDate = DateOnly.FromDateTime(account.Dob.Value);
                //DpkDob.SelectedDate = account.Dob.ToDateTime(TimeOnly.MinValue);
                DpkDob.Text = selectedAccount.Dob.ToString();
                TxtGender.Text = selectedAccount.MyGender;
                CbxStatus.Text = selectedAccount.StatusDisplay;
                TxtAccountRank.Text = selectedAccount.AccountRank.AccountRankName;
                BtnUpdate.IsEnabled = true;
                BtnDelete.IsEnabled = true;
            }
        }
        private void LoadDB()
        {
            var accounts = context.Accounts.Where(x => x.Role == 1).Include(x => x.AccountRank).ToList();
            DgvAccounts.ItemsSource = accounts;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TxtUsername.Text = string.Empty;
            TxtFullname.Text = string.Empty;
            TxtAddress.Text = string.Empty;
            TxtPhone.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            DpkDob.Text = string.Empty;
            TxtGender.Text = string.Empty;
            CbxStatus.SelectedIndex = 0;

            selectedAccount = null;
            BtnUpdate.IsEnabled = false;
            BtnDelete.IsEnabled = false;
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Account? acc = context.Accounts.Find(selectedAccount.AccountId);
            if (acc == null)
            {
                return;
            }
            selectedAccount.Status = CbxStatus.Text == "Đang hoạt động" ? true : false;
            context.Accounts.Update(acc);
            context.SaveChanges();
            LoadDB();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAccount == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            MessageBoxResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa tài khoản này?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Account? acc = context.Accounts.Find(selectedAccount.AccountId);
                    if (acc != null)
                    {
                        context.Accounts.Remove(acc);
                        context.SaveChanges();
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadDB(); // Cập nhật danh sách sau khi xóa
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy tài khoản để xóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            //bắt buộc chọn ngày sinh, không thể chọn trong tương lai
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(TxtNewFullname.Text) ||
                string.IsNullOrWhiteSpace(TxtNewUsername.Text) ||
                string.IsNullOrWhiteSpace(TxtNewPassword.Password) ||
                !DpkNewDob.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(TxtNewPhone.Text) ||
                string.IsNullOrWhiteSpace(TxtNewEmail.Text) ||
                string.IsNullOrWhiteSpace(TxtNewAddress.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ các thông tin bắt buộc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Kiểm tra tài khoản này tồn tại chưa
            if (IsExistAccount(TxtNewUsername.Text))
            {
                MessageBox.Show("Tài khoản đã tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtNewUsername.Focus();
                return;
            }

            //Kiểm tra phone đã được đăng ký
            if (IsExistPhone(TxtNewPhone.Text))
            {
                MessageBox.Show("Số điện thoại đã được đăng ký!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtNewPhone.Focus();
                return;
            }

            // Kiểm tra định dạng số điện thoại
            if (!IsValidPhoneNumber(TxtNewPhone.Text))
            {
                MessageBox.Show("Định dạng số điện thoại không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtNewPhone.Focus();
                return;
            }

            //Kiểm tra email đã được đăng ký
            if (IsExistEmail(TxtNewEmail.Text))
            {
                MessageBox.Show("Email đã được đăng ký!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtNewEmail.Focus();
                return;
            }

            // Kiểm tra định dạng email
            if (!IsValidEmail(TxtNewEmail.Text))
            {
                MessageBox.Show("Định dạng email không hợp lệ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                TxtNewEmail.Focus();
                return;
            }

            // Kiểm tra ngày sinh
            if (DpkNewDob.SelectedDate.Value >= DateTime.Today)
            {
                MessageBox.Show("Ngày sinh phải là ngày trong quá khứ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                DpkNewDob.Focus();
                return;
            }

            try
            {
                string hashedPassword = SecurityHelper.HashPassword(TxtNewPassword.Password);
                // Lấy thông tin người dùng
                Account account = new Account()
                {
                    Username = TxtNewUsername.Text,
                    Password = hashedPassword, // cần mã hóa mật khẩu
                    Role = 1,
                    Fullname = TxtNewFullname.Text,
                    Phone = TxtNewPhone.Text,
                    Email = TxtNewEmail.Text,
                    Dob = DpkNewDob.SelectedDate.HasValue ? DateOnly.FromDateTime(DpkNewDob.SelectedDate.Value) : default,
                    Gender = CbxNewGender.SelectedValue != null && CbxNewGender.SelectedValue.ToString() == "0",
                    Address = TxtNewAddress.Text,
                    Status = CbxNewStatus.SelectedValue != null && CbxNewStatus.SelectedValue?.ToString() == "1",
                    Avatar = null,
                    //AccountRankId = int.Parse(CbxNewAccountRank.SelectedValue.ToString())
                    AccountRankId = CbxNewAccountRank.SelectedValue != null ? int.Parse(CbxNewAccountRank.SelectedValue.ToString()) : 0,
                };

                // Lưu thông tin người dùng vào cơ sở dữ liệu
                context.Accounts.Add(account);
                context.SaveChanges();
                LoadDB();

                // Giả lập đăng ký thành công
                MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Chuyển đến màn hình đăng nhập
                //LoginWindow loginWindow = new LoginWindow();
                //loginWindow.Show();
                //this.Close();
                AddAccountModal.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsExistAccount(string username)
        {
            return context.Accounts.FirstOrDefault(x => x.Username == username) != null;
        }
        private bool IsExistEmail(string email)
        {
            return context.Accounts.FirstOrDefault(x => x.Email == email) != null;
        }
        private bool IsExistPhone(string phone)
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

        private void BtnCancelAddAccount_Click(object sender, RoutedEventArgs e)
        {
            AddAccountModal.Visibility = Visibility.Collapsed;
        }

        private void BtnOpenAddAccountModal_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị modal
            AddAccountModal.Visibility = Visibility.Visible;

            // Reset form
            TxtNewUsername.Text = string.Empty;
            TxtNewPassword.Password = string.Empty;
            TxtNewFullname.Text = string.Empty;
            TxtNewAddress.Text = string.Empty;
            TxtNewPhone.Text = string.Empty;
            TxtNewEmail.Text = string.Empty;
            DpkNewDob.SelectedDate = null;
            CbxNewGender.SelectedValue = 0;
            CbxNewStatus.SelectedValue = 1;
            // Load danh sách cấp độ tài khoản vào combobox
            LoadAccountRanks();
        }
        private void LoadAccountRanks()
        {
            try
            {
                var accountRanks = context.AccountRanks.ToList();
                CbxNewAccountRank.ItemsSource = accountRanks;
                CbxNewAccountRank.DisplayMemberPath = "AccountRankName";
                CbxNewAccountRank.SelectedValuePath = "AccountRankId";

                if (accountRanks.Count > 0)
                {
                    CbxNewAccountRank.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
