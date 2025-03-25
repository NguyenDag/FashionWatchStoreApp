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
    /// Interaction logic for Shop.xaml
    /// </summary>
    public partial class Shop : Window
    {
        List<String> selectedBrands = new List<String>();
        List<String> selectedCategories = new List<String>();
        int PriceFrom = 0;
        int PriceTo = int.MaxValue;
        public Shop()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
            LoadCategory();
            LoadBrand();
        }

        public void LoadData()
        {
            var listProduct = ProjectPrnContext.Ins.Products.Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.PriceBuy,
                p.Quantity,
                ProductImage = ProjectPrnContext.Ins.ProductImages
                        .Where(img => img.ProductId == p.ProductId && img.IsMain == true)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault() ?? "default_image.jpg"
            })
                .ToList();
            dgData.ItemsSource = listProduct;
            dgData.Items.Refresh();
        }

        public void LoadCategory(List<string> selectedIds = null, string id = "All")
        {
            List<Category> listCategory = ProjectPrnContext.Ins.Categories.ToList();
            cbkCategory.Children.Clear();
            CheckBox cbAll = new CheckBox();
            cbAll.Content = "All";
            cbAll.Name = "All";
            cbAll.IsChecked = true;
            cbAll.Click += Category_Click;
            cbkCategory.Children.Add(cbAll);
            foreach (var category in listCategory)
            {
                CheckBox cb = new CheckBox()
                {
                    Name = $"_{category.CategoryId}", // Convert int to string
                    Content = category.CategoryName,
                    IsChecked = selectedIds?.Contains(category.CategoryId.ToString()) == true // Ensure comparison is with string
                };

                cb.Click += Category_Click;
                cbkCategory.Children.Add(cb);
            }

            cbkBrand.Tag = id;
        }

        private void LoadBrand(List<string> selectedIds = null, string id = "All")
        {
            var brandList = ProjectPrnContext.Ins.Brands.ToList();
            cbkBrand.Children.Clear();
            CheckBox cbAll = new CheckBox();
            cbAll.Content = "All";
            cbAll.Name = "All";
            cbAll.IsChecked = true;
            cbAll.Click += Brand_Click;
            cbkBrand.Children.Add(cbAll);

            foreach (var brand in brandList)
            {
                CheckBox cb = new CheckBox()
                {
                    Name = $"_{brand.BrandId}", // Convert int to string
                    Content = brand.BrandName,
                    IsChecked = selectedIds?.Contains(brand.BrandId.ToString()) == true // Ensure comparison is with string
                };

                cb.Click += Brand_Click;
                cbkBrand.Children.Add(cb);
            }

            cbkBrand.Tag = id;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void Brand_Click(object sender, RoutedEventArgs e)
        {
            selectedBrands.Clear();
            if (sender is CheckBox cb && cb.Name == "All")
            {
                selectedBrands.Add("All");
                foreach (var obj in cbkBrand.Children)
                {
                    if (obj is CheckBox chk && chk.Name != "All")
                    {
                        chk.IsChecked = false;
                    }
                }
            }
            else
            {
                foreach (var obj in cbkBrand.Children)
                {
                    if (obj is CheckBox chk && chk.IsChecked == true)
                    {
                        if (chk.Name == "All")
                        {
                            chk.IsChecked = false;
                        }
                        else
                        {
                            selectedBrands.Add(chk.Name.TrimStart('_')); // Lấy CategoryId từ Name
                        }
                    }
                }
            }
            ApplyFilters(); ;
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            selectedCategories.Clear();
            if (sender is CheckBox cb && cb.Name == "All")
            {
                selectedCategories.Add("All");
                foreach (var obj in cbkCategory.Children)
                {
                    if (obj is CheckBox chk && chk.Name != "All")
                    {
                        chk.IsChecked = false;
                    }
                }
            }
            else
            {
                foreach (var obj in cbkCategory.Children)
                {
                    if (obj is CheckBox chk && chk.IsChecked == true)
                    {
                        if (chk.Name == "All")
                        {
                            chk.IsChecked = false;
                        }
                        else
                        {
                            selectedCategories.Add(chk.Name.TrimStart('_'));
                        }
                    }
                }
            }
            ApplyFilters();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string PriceFromRaw = txtPriceFrom?.Text?.Trim().ToLower();
            string PriceToRaw = txtPriceTo?.Text?.Trim().ToLower();

            if (!int.TryParse(PriceFromRaw, out PriceFrom))
            {
                PriceFrom = 0;
            }
            if (!int.TryParse(PriceToRaw, out PriceTo))
            {
                PriceTo = int.MaxValue;
            }
            if (PriceFrom > PriceTo)
            {
                MessageBox.Show("Khoảng giá không hợp lệ!");
                return;
            }
            ApplyFilters();
        }

        private bool IsValidMoneyInput(string input, TextBox textBox)
        {
            if (input.All(char.IsDigit)) return true;
            return false; // Chặn tất cả ký tự khác
        }

        private void txtPriceFrom_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsValidMoneyInput(e.Text, (TextBox)sender);
        }

        private void ApplyFilters()
        {
            // Lấy danh sách sản phẩm gốc
            var products = ProjectPrnContext.Ins.Products.AsQueryable();

            // Lọc theo Brand
            var selectedBrandsForFilter = selectedBrands.Where(id => id != "All").ToList();
            if (selectedBrandsForFilter.Any())
            {
                products = products.Where(p => selectedBrandsForFilter.Contains(p.BrandId.ToString()));
            }

            // Lọc theo Category
            var selectedCategoriesForFilter = selectedCategories.Where(id => id != "All").ToList();
            if (selectedCategoriesForFilter.Any())
            {
                products = products.Where(p => selectedCategoriesForFilter.Contains(p.CategoryId.ToString()));
            }

            // Lọc theo tên (nếu có)
            string searchText = txtSearch?.Text?.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(searchText));
            }

            products = products.Where(p => p.PriceBuy >= PriceFrom && p.PriceBuy <= PriceTo);

            // Cập nhật danh sách sản phẩm
            var filteredProducts = products.ToList();
            dgData.ItemsSource = filteredProducts;
            dgData.Items.Refresh();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewDetail_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && button.Tag is int productID)
            {
                ProductDetail productDetail = new ProductDetail(productID);
                this.Close();
                productDetail.ShowDialog();
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null && border.Tag is int productID)
            {
                OpenProductDetail(productID);
            }

        }

        public void OpenProductDetail(int ProductID)
        {
            ProductDetail productDetail = new ProductDetail(ProductID);
            this.Close();
            productDetail.ShowDialog();
        }
    }
}
