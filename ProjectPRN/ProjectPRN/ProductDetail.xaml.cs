using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;

namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : Window
    {
        private Product productdetail;
        private List<Review> reviewList;
        public ProductDetail(int ProductID)
        {
            InitializeComponent();
            this.productdetail = ProjectPrnContext.Ins.Products.Find(ProductID);
            var product = ProjectPrnContext.Ins.Products.Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.PriceBuy,
                p.Quantity,
                p.Brand,
                p.Category,
                p.ProductDescription,
                ProductImage = ProjectPrnContext.Ins.ProductImages
                        .Where(img => img.ProductId == p.ProductId && img.IsMain == true)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault() ?? "default_image.jpg"
            }).FirstOrDefault(p=>p.ProductId==ProductID);
            var productList = ProjectPrnContext.Ins.Products.Where(p => p.Brand.BrandId == product.Brand.BrandId || p.Category.CategoryId == product.Category.CategoryId).
                Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.PriceBuy,
                    p.Quantity,
                    p.Brand,
                    p.Category,
                    p.ProductDescription,
                    ProductImage = ProjectPrnContext.Ins.ProductImages
                        .Where(img => img.ProductId == p.ProductId && img.IsMain == true)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault() ?? "default_image.jpg"
                }).ToList();
            reviewList = ProjectPrnContext.Ins.Reviews.Include(a => a.Account).Where(r => r.Product.ProductId == ProductID).ToList();
            List<ProductImage> productImageList = ProjectPrnContext.Ins.ProductImages.Where(pi => pi.Product.ProductId == ProductID).ToList();
            Detail.DataContext = product;
            RelatedProductList.ItemsSource = productList;
            ReviewList.ItemsSource = reviewList;
            ImageList.ItemsSource = productImageList;
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (productdetail == null)
            {
                MessageBox.Show("Sản phẩm không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int accountId=SessionManager.AccountId;

            var existingCartItem = ProjectPrnContext.Ins.Carts
                .FirstOrDefault(c => c.ProductId == productdetail.ProductId && c.AccountId == accountId);

            if (existingCartItem != null)
            {
                MessageBox.Show("Sản phẩm đã có trong giỏ hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                Cart newCartItem = new Cart
                {
                    AccountId = accountId,
                    ProductId = productdetail.ProductId,
                    Quantity = 1,
                };
                ProjectPrnContext.Ins.Carts.Add(newCartItem);
                ProjectPrnContext.Ins.SaveChanges();
                MessageBox.Show("Thêm vào giỏ hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

         
            // Quay lại trang Shop
            Shop shopWindow = new Shop();
            shopWindow.Show();
            this.Close(); 
        }


        private void SortReview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            ComboBoxItem cbi = cb.SelectedItem as ComboBoxItem;

        }
    }
}
