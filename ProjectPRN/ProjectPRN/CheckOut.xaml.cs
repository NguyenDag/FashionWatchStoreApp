using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
    /// Interaction logic for CheckOut.xaml
    /// </summary>
    public partial class CheckOut : Window
    {
        public ObservableCollection<Cart> CheckoutCarts { get; set; }
        public CheckOut(ObservableCollection<Cart> selectedCarts)
        {
            InitializeComponent();
            CheckoutCarts = selectedCarts;
            decimal total = 0; // Change the type of total to decimal
            foreach (var item in selectedCarts)
            {
                total += (item.Product.PriceSell * item.Quantity);
            }
            cmbDiscount.ItemsSource = GetDiscount(SessionManager.account.AccountRankId, total); // Convert double to string            
            txtTotalCost.Text = total.ToString(); // Convert decimal to string
                                                  //decimal ActualCost = (total - (total * ((decimal)GetDiscount(1) / 100)));
            txtRecieveName.Text = SessionManager.account.Username;
            txtRecievePhone.Text = SessionManager.account.Phone;
            GetShip();
            DataContext = this;
            LoadUserAddresses();
        }
        public List<Discount> GetDiscount(int? id, decimal totalCost)
        {
            List<Discount> discounts = ProjectPrnContext.Ins.Discounts
                .Where(x => x.AccountRankId == id || totalCost >= x.MinCost).OrderByDescending(x => x.Percent).ToList();
            return discounts;
        }

        public void GetShip()
        {
            cmbShip.Items.Clear();
            List<Shipping> list = ProjectPrnContext.Ins.Shippings.ToList();

            foreach (var item in list)
            {
                cmbShip.Items.Add(item); // Thêm trực tiếp đối tượng vào ComboBox
            }

        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            string RecieveName = txtRecieveName.Text.Trim();
            string RecievePhone = txtRecievePhone.Text.Trim();
            string RecieveAddress = "";
            RadioButton selectedRadio = AddressPanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);
            if (selectedRadio != null)
            {
                RecieveAddress = selectedRadio.Content.ToString();
                if (RecieveAddress == "Other")
                {
                    MessageBox.Show("Vui lòng lưu địa chỉ mới hoặc chọn một địa chỉ có sẵn!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn địa chỉ nhận hàng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int PaymentType = txtPaymentType.Text.Trim() == "Online" ? 1 : 2;
            string Note = txtNote.Text.Trim();

            int shipId = 0;
            if (cmbShip.SelectedItem is Shipping selectedShip)
            {
                shipId = selectedShip.ShipId;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phương thức vận chuyển!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Shipping shipping = ProjectPrnContext.Ins.Shippings
                .FirstOrDefault(x => x.ShipId == shipId);
            int shipTime = shipping?.ShipTime ?? 0;
            int shipCost = (int)(shipping?.ShipCost ?? 0);

            if (cmbDiscount.SelectedItem is Discount selectedDiscount)
            {
                // Discount đã được chọn từ ComboBox
            }
            else
            {
                MessageBox.Show("Vui lòng chọn mã giảm giá!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int discountId = selectedDiscount.DiscountId;
            decimal totalCost = decimal.Parse(txtTotalCost.Text);
            decimal actualCost = decimal.Parse(txtActualCost.Text, System.Globalization.NumberStyles.Currency);

            Order order = new Order()
            {
                AccountId = SessionManager.account.AccountId,
                ReceiveName = RecieveName,
                ReceivePhone = RecievePhone,
                ReceiveAddress = RecieveAddress,
                PaymentType = PaymentType,
                ShipId = shipId,
                DiscountId = discountId,
                TotalCost = totalCost,
                ActualCost = actualCost,
                OrderDate = DateTime.Now,
                Status = "Pending",
                Note = Note,
                ReceiveDate = DateTime.Now.AddDays(shipTime),
            };

            using (var transaction = ProjectPrnContext.Ins.Database.BeginTransaction())
            {
                try
                {
                    ProjectPrnContext.Ins.Orders.Add(order);
                    ProjectPrnContext.Ins.SaveChanges();
                    foreach (var cart in CheckoutCarts)
                    {
                        OrderDetail orderDetail = new OrderDetail()
                        {
                            OrderId = order.OrderId, // Lấy OrderId vừa tạo
                            ProductId = cart.Product.ProductId, // Giả sử Product có thuộc tính ProductId
                            Quantity = cart.Quantity,
                            Price = cart.Product.PriceSell // Giả sử PriceSell là giá bán của sản phẩm
                        };
                        ProjectPrnContext.Ins.OrderDetails.Add(orderDetail);
                        // Trừ số lượng sản phẩm trong bảng Product
                        var product = ProjectPrnContext.Ins.Products
                            .FirstOrDefault(p => p.ProductId == cart.Product.ProductId);
                        if (product != null)
                        {
                            if (product.Quantity < cart.Quantity) // Kiểm tra số lượng tồn kho
                            {
                                throw new Exception($"Sản phẩm {product.ProductName} không đủ số lượng trong kho!");
                            }
                            product.Quantity -= cart.Quantity; // Trừ số lượng
                        }
                        else
                        {
                            throw new Exception($"Không tìm thấy sản phẩm với ID {cart.Product.ProductId}!");
                        }
                    }
                    ProjectPrnContext.Ins.SaveChanges();

                    ProjectPrnContext.Ins.Discounts.Find(discountId).Amount -= 1;
                    ProjectPrnContext.Ins.SaveChanges();

                    var cartIdsToRemove = CheckoutCarts.Select(c => c.CartId).ToList();
                    var cartsToRemove = ProjectPrnContext.Ins.Carts
                        .Where(c => cartIdsToRemove.Contains(c.CartId) && c.AccountId == SessionManager.account.AccountId)
                        .ToList();

                    if (cartsToRemove.Count != cartIdsToRemove.Count)
                    {
                        throw new Exception("Một số mục trong giỏ hàng không tồn tại hoặc không thuộc về tài khoản này!");
                    }

                    if (cartsToRemove.Any())
                    {
                        ProjectPrnContext.Ins.Carts.RemoveRange(cartsToRemove);
                        ProjectPrnContext.Ins.SaveChanges();
                    }

                    transaction.Commit();

                    SendOrderConfirmationEmail(order, RecieveName, totalCost, actualCost, shipCost, order.ReceiveDate.Value);

                    MessageBox.Show("Bạn đã thanh toán thành công đơn hàng. Giỏ hàng đã được làm trống!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    Home home = new Home();
                    this.Close();
                    home.ShowDialog();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Có lỗi khi thanh toán: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void SendOrderConfirmationEmail(Order order, string receiveName, decimal totalCost, decimal actualCost, int shipCost, DateTime receiveDate)
        {
            try
            {
                string fromEmail = "dinhthanhtung2442k4@gmail.com"; // Thay bằng email của bạn
                string password = "mibk egxk uqtj vpbi"; // Thay bằng mật khẩu ứng dụng (nếu dùng Gmail)
                string toEmail = ProjectPrnContext.Ins.Accounts
                    .Where(a => a.AccountId == SessionManager.account.AccountId)
                    .Select(a => a.Email)
                    .FirstOrDefault(); // Lấy email của người dùng từ database

                if (string.IsNullOrEmpty(toEmail))
                {
                    MessageBox.Show("Không tìm thấy email của khách hàng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Tạo nội dung email
                string subject = "Xác nhận đơn hàng thành công - Mã đơn hàng: " + order.OrderId;
                string body = $"Kính gửi {receiveName},\n\n" +
                              $"Cảm ơn bạn đã đặt hàng tại cửa hàng của chúng tôi! Dưới đây là chi tiết đơn hàng của bạn:\n\n" +
                              $"Mã đơn hàng: {order.OrderId}\n" +
                              $"Tổng giá trị sản phẩm: {totalCost:C}\n" +
                              $"Phí vận chuyển: {shipCost:C}\n" +
                              $"Tổng tiền thanh toán: {actualCost:C}\n" +
                              $"Ngày đặt hàng: {order.OrderDate:dd/MM/yyyy HH:mm}\n" +
                              $"Ngày nhận hàng dự kiến: {receiveDate:dd/MM/yyyy}\n" +
                              $"Địa chỉ nhận hàng: {order.ReceiveAddress}\n" +
                              $"Ghi chú: {(string.IsNullOrEmpty(order.Note) ? "Không có" : order.Note)}\n\n" +
                              $"Nếu bạn có bất kỳ câu hỏi nào, vui lòng liên hệ với chúng tôi qua email {fromEmail}.\n\n" +
                              $"Trân trọng,\nĐội ngũ cửa hàng máy tính";

                // Cấu hình SMTP
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(fromEmail);
                    mail.To.Add(toEmail);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = false; // Đặt true nếu muốn dùng HTML trong email

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(fromEmail, password);

                        // Gửi email bất đồng bộ để không làm treo UI
                        await smtp.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi khi gửi email: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cmbDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateActualCost();
        }

        // Sự kiện khi chọn Shipping
        private void cmbShip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateActualCost();
        }

        // Hàm tính và cập nhật ActualCost
        private void UpdateActualCost()
        {
            // Chỉ tính ActualCost nếu cả Discount và Shipping đã được chọn
            if (cmbDiscount.SelectedItem is Discount selectedDiscount && cmbShip.SelectedItem is Shipping selectedShipping)
            {
                decimal discountPercent = (decimal)selectedDiscount.Percent / 100;
                decimal shipCost = selectedShipping.ShipCost ?? 0;
                decimal totalCost = decimal.Parse(txtTotalCost.Text); // Convert string to decimal
                decimal actualCost = totalCost - (totalCost * discountPercent) + shipCost;
                txtActualCost.Text = actualCost.ToString("C"); // Hiển thị ActualCost với định dạng tiền tệ
            }
        }

        private void btnBackCart_Click(object sender, RoutedEventArgs e)
        {
            ShoppingCart cartWindow = new ShoppingCart();
            this.Close();
            cartWindow.ShowDialog();
        }

        private void LoadUserAddresses()
        {
            // Xóa các RadioButton cũ (nếu có) để tránh trùng lặp
            AddressPanel.Children.Clear();

            // Lấy RecieveAddress từ SessionManagement
            string recieveAddress = SessionManager.account.Address;

            if (!string.IsNullOrEmpty(recieveAddress))
            {
                string[] addresses = recieveAddress.Split('|', StringSplitOptions.RemoveEmptyEntries); // Split và bỏ các phần tử rỗng
                foreach (string address in addresses)
                {
                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        RadioButton radio = new RadioButton
                        {
                            Content = address.Trim(), // Loại bỏ khoảng trắng thừa
                            Margin = new Thickness(0, 0, 0, 5),
                            GroupName = "Addresses"
                        };
                        radio.Checked += RadioButton_Checked; // Attach event handler using +=
                        AddressPanel.Children.Add(radio);
                    }
                }
            }

            // Thêm tùy chọn "Other"
            RadioButton otherRadio = new RadioButton
            {
                Content = "Other",
                Margin = new Thickness(0, 0, 0, 5),
                GroupName = "Addresses"
            };
            otherRadio.Checked += RadioButton_Checked; // Attach event handler using +=
            AddressPanel.Children.Add(otherRadio);

            // Chọn địa chỉ đầu tiên mặc định (nếu có)
            if (AddressPanel.Children.Count > 1)
                ((RadioButton)AddressPanel.Children[0]).IsChecked = true;
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton selectedRadio = sender as RadioButton;
            if (selectedRadio != null && selectedRadio.Content.ToString() == "Other")
            {
                OtherAddressBorder.Visibility = Visibility.Visible;
            }
            else
            {
                OtherAddressBorder.Visibility = Visibility.Collapsed;
                txtOtherAddress.Text = ""; // Xóa nội dung khi không chọn "Other"
            }
        }
        private void btnSaveAddress_Click(object sender, RoutedEventArgs e)
        {
            string newAddress = txtOtherAddress.Text.Trim();
            if (string.IsNullOrWhiteSpace(newAddress))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ mới!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Cập nhật RecieveAddress vào SessionManagement
            if (string.IsNullOrEmpty(SessionManager.account.Address))
                SessionManager.account.Address = newAddress;
            else if (!SessionManager.account.Address.Split('|').Contains(newAddress))
                SessionManager.account.Address += "|" + newAddress;

            // Lưu vào database
            try
            {
                var user = ProjectPrnContext.Ins.Accounts
                    .FirstOrDefault(a => a.AccountId == SessionManager.account.AccountId);
                if (user != null)
                {
                    user.Address = SessionManager.account.Address;
                    ProjectPrnContext.Ins.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu địa chỉ vào database: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Tải lại danh sách địa chỉ
            LoadUserAddresses();

            // Ẩn TextBox và chọn địa chỉ vừa thêm
            OtherAddressBorder.Visibility = Visibility.Collapsed;
            txtOtherAddress.Text = "";
            var newRadio = AddressPanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.Content.ToString() == newAddress);
            if (newRadio != null)
                newRadio.IsChecked = true;
        }
    }
}
