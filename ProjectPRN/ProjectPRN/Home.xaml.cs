using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;

namespace ProjectPRN
{
    public partial class Home : Window
    {
        public ObservableCollection<Product> FeaturedProducts { get; set; }
        public ObservableCollection<Product> NewProducts { get; set; }
        public ObservableCollection<Review> Reviews { get; set; }


        public Home()
        {
            InitializeComponent();
            LoadData();
            DataContext = this;
            UpdateNavbar();

        }

        private void LoadData()
        {
            using (var context = new ProjectPrnContext())
            {
                var productList = context.Products
                     .Include(p => p.Category)
                     .Include(p => p.Brand)
                     .Include(p => p.Reviews)
                     .Where(p => p.Status == 1 && p.Reviews.Any()) 
                     .OrderByDescending(p => p.Reviews.Average(r => r.ReviewRating)) 
                     .Take(7)
                     .ToList();

                FeaturedProducts = new ObservableCollection<Product>(productList);

                // Lấy 10 sản phẩm mới nhất dựa trên ID sản phẩm (từ ID lớn nhất xuống)
                var newProductList = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Where(p => p.Status == 1)
                    .OrderByDescending(p => p.ProductId) 
                    .Take(10)
                    .ToList();

                NewProducts = new ObservableCollection<Product>(newProductList);

                var reviewList = context.Reviews
                  .Include(r => r.Product)
                  .Include(r => r.Account)
                  .OrderByDescending(r => r.ReviewRating) 
                  .ThenByDescending(r => r.ReviewId) 
                  .Take(10)
                  .Select(r => new Review
                  {
                      ReviewId = r.ReviewId,
                      ProductId = r.ProductId,
                      AccountId = r.AccountId,
                      ReviewText = r.ReviewText,
                      ReviewRating = r.ReviewRating,
                      Account = new Account
                      {
                          Username = r.Account.Username
                      }
                  })
                  .ToList();

                Reviews = new ObservableCollection<Review>(reviewList);

            }
        }



        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            if (window.ShowDialog() == true)
            {
                if (SessionManager.IsLoggedIn)
                {
                    UpdateNavbar(); 
                }
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
                btnLogin.Visibility = Visibility.Collapsed;
                btnRegister.Visibility = Visibility.Collapsed;

                btnViewInfo.Visibility = Visibility.Visible;
                btnLogout.Visibility = Visibility.Visible;

            }
            else
            {
                btnLogin.Visibility = Visibility.Visible;
                btnRegister.Visibility = Visibility.Visible;

                btnViewInfo.Visibility = Visibility.Collapsed;
                btnLogout.Visibility = Visibility.Collapsed;

            }
        }

        private void btnViewInfo_Click(object sender, RoutedEventArgs e)
        {
            ViewInfo profileWindow = new ViewInfo();
            profileWindow.ShowDialog();
            
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.Logout(); 
            MessageBox.Show("Bạn đã đăng xuất thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateNavbar(); 
        }

        private void btnCart_Click(object sender, RoutedEventArgs e)
        {
            int accountId = SessionManager.AccountId;
            ShoppingCart cart = new ShoppingCart();
            cart.Show();
            this.Close();
        }

        private void btnShop_Click(object sender, RoutedEventArgs e)
        {
            Shop shop = new Shop();
            shop.ShowDialog();
            this.Close();
        }
    }
}
