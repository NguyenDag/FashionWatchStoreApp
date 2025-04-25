using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;

namespace ProjectPRN
{
    public partial class ShoppingCart : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Cart> CartItems { get; set; } = new ObservableCollection<Cart>();
        public ObservableCollection<Cart> SelectedItems { get; set; } = new ObservableCollection<Cart>();

        private int accountId = SessionManager.AccountId;
        public ShoppingCart()
        {
            InitializeComponent();
            DataContext = this;
            UpdateNavbar();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCartItems();
        }



        private void LoadCartItems()
        {
            using (var context = new ProjectPrnContext())
            {
                var cartItems = context.Carts
                    .Where(c => c.AccountId == accountId)
                    .Include(c => c.Product)
                    .ThenInclude(p => p.ProductImages)
                    .ToList();

                CartItems.Clear();
                foreach (var item in cartItems)
                {
                    var cart = new Cart
                    {
                        CartId = item.CartId,
                        AccountId = item.AccountId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Product = item.Product,
                        IsSelected = false
                    };

                    cart.PropertyChanged += Cart_PropertyChanged;
                    CartItems.Add(cart);
                }
            }
        }


        public decimal CheckoutTotal => SelectedItems.Sum(item => item.Quantity * item.Product.PriceSell);
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Cart_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Cart.IsSelected))
            {
                SelectedItems.Clear();
                foreach (var item in CartItems.Where(c => c.IsSelected))
                {
                    SelectedItems.Add(item);
                }
                OnPropertyChanged(nameof(CheckoutTotal));
                OnPropertyChanged(nameof(SelectedItems)); 
            }
        }


        // Tăng số lượng
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int productId)
            {
                UpdateCartQuantity(productId, 1);
            }
        }

        // Giảm số lượng
        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int productId)
            {
                UpdateCartQuantity(productId, -1);
            }
        }

        // Cập nhật số lượng sản phẩm trong giỏ hàng
        private void UpdateCartQuantity(int productId, int change)
        {
            using (var context = new ProjectPrnContext())
            {
                var cartItem = context.Carts.Include(c => c.Product).FirstOrDefault(c => c.ProductId == productId && c.AccountId == accountId);
                if (cartItem != null)
                {
                    int newQuantity = cartItem.Quantity + change;
                    if (newQuantity >= 1 && newQuantity <= cartItem.Product.Quantity)
                    {
                        cartItem.Quantity = newQuantity;
                        context.SaveChanges();
                    }
                }
            }

            LoadCartItems();
        }

        // Xử lý xóa sản phẩm khỏi giỏ hàng
        private void RemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int productId)
            {
                using (var context = new ProjectPrnContext())
                {
                    // Tìm sản phẩm trong giỏ hàng của người dùng
                    var cartItem = context.Carts.FirstOrDefault(c => c.ProductId == productId && c.AccountId == accountId);
                    if (cartItem != null)
                    {
                        context.Carts.Remove(cartItem);
                        context.SaveChanges();
                        LoadCartItems();
                    }
                }
            }
        }



        // Xử lý các sự kiện của Navbar
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            if (window.ShowDialog() == true && SessionManager.IsLoggedIn)
            {
                accountId = SessionManager.AccountId; // Lấy ID người dùng sau khi đăng nhập
                LoadCartItems();
                UpdateNavbar();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow();
            window.Show();
            this.Close();
        }

        private void btnHome_Click_1(object sender, RoutedEventArgs e)
        {
            Home homeWindow = new Home();
            homeWindow.Show();
            this.Close();
        }

        private void UpdateNavbar()
        {
            if (SessionManager.IsLoggedIn)
            {
                // Nếu đã đăng nhập -> Ẩn Đăng nhập / Đăng ký, Hiện thông tin tài khoản & Đăng xuất
                btnLogin.Visibility = Visibility.Collapsed;
                btnRegister.Visibility = Visibility.Collapsed;

                btnViewInfo.Visibility = Visibility.Visible;
                btnLogout.Visibility = Visibility.Visible;

            }
            else
            {
                // Nếu chưa đăng nhập -> Hiện Đăng nhập / Đăng ký, Ẩn Đăng xuất / Thông tin tài khoản
                btnLogin.Visibility = Visibility.Visible;
                btnRegister.Visibility = Visibility.Visible;

                btnViewInfo.Visibility = Visibility.Collapsed;
                btnLogout.Visibility = Visibility.Collapsed;

            }
        }

        private void btnViewInfo_Click(object sender, RoutedEventArgs e)
        {
            ViewInfo profileWindow = new ViewInfo();
            profileWindow.Show();
            this.Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.Logout();
            MessageBox.Show("Bạn đã đăng xuất thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            accountId = 0; // Đặt lại accountId khi đăng xuất
            CartItems.Clear(); // Xóa giỏ hàng khi đăng xuất
            UpdateNavbar();
        }


        private void btnShop_Click(object sender, RoutedEventArgs e)
        {
            Shop shop = new Shop();
            shop.Show();
            this.Close();
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            int accountId = SessionManager.AccountId;
            ShoppingCart cart = new ShoppingCart();
            cart.Show();
            this.Close();

        }
        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            CheckOut checkOut = new CheckOut(SelectedItems);
            this.Close();
            checkOut.ShowDialog();
        }
    }
}
