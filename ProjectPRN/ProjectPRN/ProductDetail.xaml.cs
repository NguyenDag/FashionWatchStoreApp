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
    /// Interaction logic for ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : Window
    {
        public ProductDetail(int ProductID)
        {
            InitializeComponent();
            Product? product = ProjectPrnContext.Ins.Products.FirstOrDefault(p => p.ProductId == ProductID);
            List<Product> productList = ProjectPrnContext.Ins.Products.Where(p => p.Brand.BrandId == product.Brand.BrandId || p.Category.CategoryId == product.Category.CategoryId).ToList();
            List<Review> reviewList = product.Reviews.ToList();
            Detail.DataContext = product;
            RelatedProductList.ItemsSource = productList;
            ReviewList.ItemsSource = reviewList;
            ProductImagePreview.DataContext = "Images/logo.png";
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
