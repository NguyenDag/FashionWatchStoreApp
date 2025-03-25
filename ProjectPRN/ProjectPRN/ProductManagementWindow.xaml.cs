using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Interaction logic for ProductManagementWindow.xaml
    /// </summary>
    public partial class ProductManagementWindow : Window
    {
        private readonly ProjectPrnContext context;
        private ObservableCollection<Product> products;
        private ObservableCollection<Category> categories;
        private ObservableCollection<Brand> brands;
        private int currentImageIndex = 0;
        private List<ProductImage> currentProductImages = new List<ProductImage>();
        private Product selectedProduct;
        public ProductManagementWindow()
        {
            context = new ProjectPrnContext();
            selectedProduct = new Product();
            InitializeComponent();
            //InitializeData();
            //SetupEventHandlers();
        }
        private void dgvProducts_Loaded(object sender, RoutedEventArgs e)
        {
            products = new ObservableCollection<Product>();
            categories = new ObservableCollection<Category>();
            brands = new ObservableCollection<Brand>();

            LoadCategories();
            LoadBrands();
            LoadProducts();

            dgvProducts.ItemsSource = products;
            cbxCategory.ItemsSource = categories;
            cbxCategory.DisplayMemberPath = "CategoryName";
            cbxCategory.SelectedValuePath = "CategoryId";
            cbxCategory.SelectedIndex = 0;

            cbxBrand.ItemsSource = brands;
            cbxBrand.DisplayMemberPath = "BrandName";
            cbxBrand.SelectedValuePath = "BrandId";
            cbxBrand.SelectedIndex = 0;
        }
        //private void InitializeData()
        //{
        //    products = new ObservableCollection<Product>();
        //    categories = new ObservableCollection<Category>();
        //    brands = new ObservableCollection<Brand>();

        //    LoadCategories();
        //    LoadBrands();
        //    LoadProducts();

        //    dgvProducts.ItemsSource = products;
        //    cbxCategory.ItemsSource = categories;
        //    cbxCategory.DisplayMemberPath = "CategoryName";
        //    cbxCategory.SelectedValuePath = "CategoryId";
        //    cbxCategory.SelectedIndex = 0;

        //    cbxBrand.ItemsSource = brands;
        //    cbxBrand.DisplayMemberPath = "BrandName";
        //    cbxBrand.SelectedValuePath = "BrandId";
        //    cbxBrand.SelectedIndex = 0;

        //    //ClearForm();
        //}

        private void SetupEventHandlers()
        {
            //btnNew.Click += (s, e) => ClearForm();
            //btnSave.Click += (s, e) => SaveProduct();
            //btnDelete.Click += (s, e) => DeleteProduct();
            //btnClear.Click += (s, e) => ClearForm();
            //dgProducts.SelectionChanged += DgProducts_SelectionChanged;
        }
        private void LoadCategories()
        {
            try
            {
                categories.Clear();
                foreach (var category in context.Categories.ToList())
                {
                    categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải categories: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBrands()
        {
            try
            {
                brands.Clear();
                foreach (var brand in context.Brands.ToList())
                {
                    brands.Add(brand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading brands: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                products.Clear();
                foreach (var product in context.Products.Include(x => x.Category)
                                                .Include(x => x.Brand)
                                                .Include(x => x.ProductImages)
                                                .ToList())
                {
                    products.Add(product);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadProductImages(int productId)
        {
            try
            {
                currentProductImages.Clear();
                currentProductImages = context.ProductImages
            .Where(x => x.ProductId == productId && x.IsMain == true)
            .ToList();

                DisplayCurrentImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading product images: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayCurrentImage()
        {
            if (currentProductImages.Count > 0 && currentImageIndex < currentProductImages.Count)
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(currentProductImages[currentImageIndex].ImageUrl);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imgProductMain.Source = bitmap;
                }
                catch
                {
                    imgProductMain.Source = null;
                }
            }
            else
            {
                imgProductMain.Source = null;
            }
        }

        private void dgvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedProduct = dgvProducts.SelectedItem as Product;
                if (selectedProduct != null)
                {
                    txtProductId.Text = selectedProduct.ProductId.ToString();
                    txtProductName.Text = selectedProduct.ProductName;
                    cbxCategory.SelectedValue = selectedProduct.CategoryId;
                    cbxBrand.SelectedValue = selectedProduct.BrandId;
                    cbxStatus.SelectedValue = selectedProduct.Status;
                    txtQuantity.Text = selectedProduct.Quantity.ToString();
                    txtBuyPrice.Text = ((int)selectedProduct.PriceBuy).ToString();
                    txtSellPrice.Text = ((int)selectedProduct.PriceSell).ToString();
                    txtDescription.Text = selectedProduct.ProductDescription;

                    // Hiển thị ảnh từ đường dẫn
                    if (selectedProduct.ProductImages != null && selectedProduct.ProductImages.Any())
                    {
                        var mainImage = selectedProduct.ProductImages
                            .FirstOrDefault(x => x.ProductId == selectedProduct.ProductId && x.IsMain == true)
                            ?? selectedProduct.ProductImages.FirstOrDefault(); // Lấy ảnh đầu tiên nếu không có ảnh chính

                        if (mainImage != null && !string.IsNullOrEmpty(mainImage.ImageUrl))
                        {
                            string fullPath = System.IO.Path.GetFullPath(mainImage.ImageUrl);
                            MessageBox.Show($"Đường dẫn ảnh: {mainImage.ImageUrl}");

                            if (File.Exists(mainImage.ImageUrl))
                            {
                                imgProductMain.Source = new BitmapImage(new Uri(mainImage.ImageUrl, UriKind.Absolute));
                            }
                            else
                            {
                                MessageBox.Show("File ảnh không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                                imgProductMain.Source = null;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy ảnh hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                            imgProductMain.Source = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sản phẩm này không có danh sách ảnh!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        imgProductMain.Source = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải ảnh: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra context có bị null không
                if (context == null)
                {
                    MessageBox.Show("Database context chưa được khởi tạo!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Kiểm tra combobox không null
                if (cbxCategory.SelectedValue == null || cbxBrand.SelectedValue == null || cbxStatus.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn đầy đủ danh mục, thương hiệu và trạng thái!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra nhập liệu
                if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                    string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                    string.IsNullOrWhiteSpace(txtBuyPrice.Text) ||
                    string.IsNullOrWhiteSpace(txtSellPrice.Text) ||
                    string.IsNullOrWhiteSpace(txtDescription.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra số nguyên hợp lệ
                if (!IsValidInteger(txtQuantity.Text, out int quantity) || quantity < 0 ||
                    !IsValidInteger(txtBuyPrice.Text, out int buyPrice) || buyPrice < 0 ||
                    !IsValidInteger(txtSellPrice.Text, out int sellPrice) || sellPrice < 0)
                {
                    MessageBox.Show("Số lượng, giá nhập, giá bán phải là số nguyên không âm!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra giá bán không nhỏ hơn giá nhập
                if (sellPrice < buyPrice)
                {
                    MessageBox.Show("Giá bán không thể thấp hơn giá nhập!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra trùng tên sản phẩm
                string newProductName = txtProductName.Text.Trim();
                if (selectedProduct.ProductId == 0 || selectedProduct.ProductName != newProductName)
                {
                    if (IsExistProductName(newProductName, selectedProduct.ProductId))
                    {
                        MessageBox.Show("Tên sản phẩm đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Kiểm tra sản phẩm có trong DB chưa
                //Product product = context.Products.FirstOrDefault(p => p.ProductId == selectedProduct.ProductId);
                Product product = selectedProduct.ProductId == 0 ? new Product() : context.Products.FirstOrDefault(p => p.ProductId == selectedProduct.ProductId);

                //if (product == null) // Nếu không có, tạo mới
                //{
                //    product = new Product();
                //    context.Products.Add(product);
                //}
                if (product == null) // Trường hợp không tìm thấy sản phẩm trong DB
                {
                    MessageBox.Show("Sản phẩm không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                // Cập nhật dữ liệu
                product.ProductName = newProductName;
                product.CategoryId = Convert.ToInt32(cbxCategory.SelectedValue);
                product.BrandId = Convert.ToInt32(cbxBrand.SelectedValue);
                product.Status = cbxStatus.SelectedValue != null ? Convert.ToInt32(cbxStatus.SelectedValue) : null;
                product.Quantity = quantity;
                product.PriceBuy = buyPrice;
                product.PriceSell = sellPrice;
                product.ProductDescription = txtDescription.Text.Trim();

                if (selectedProduct.ProductId == 0) // Nếu đang thêm mới
                {
                    context.Products.Add(product);
                }

                // Lưu thay đổi vào DB
                context.SaveChanges();

                // Cập nhật danh sách sản phẩm
                LoadProducts();

                // Hiển thị thông báo
                MessageBox.Show(product.ProductId == 0 ? "Thêm sản phẩm thành công!" : "Cập nhật sản phẩm thành công!",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reset form
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool IsExistProductName(string productName, int productId)
        {

            return context.Products
        .Any(x => x.ProductId != productId &&
                  x.ProductName.Equals(productName));
        }
        // Hàm chuẩn hóa chuỗi: Loại bỏ khoảng trắng dư thừa giữa các từ
        private string NormalizeString(string input)
        {
            return string.Join(" ", input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).Trim();
        }
        private bool IsValidInteger(string input, out int result)
        {
            return int.TryParse(input, out result);
        }
        private bool IsValidPrice(int buyPrice, int sellPrice)
        {
            return sellPrice >= buyPrice;
        }
        private void ClearForm()
        {
            txtProductId.Text = string.Empty;
            txtProductName.Text = string.Empty;
            cbxCategory.SelectedIndex = 0;
            cbxBrand.SelectedIndex = 0;
            txtQuantity.Text = "0";
            txtBuyPrice.Text = "0";
            txtSellPrice.Text = "0";
            txtDescription.Text = string.Empty;
            cbxStatus.SelectedIndex = 0;
            imgProductMain.Source = null;
            currentProductImages.Clear();
            currentImageIndex = 0;
            btnSave.Content = "Lưu";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận
            MessageBoxResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa sản phẩm này?",
                "Xác nhận xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    Product? pro = context.Products.Find(selectedProduct.ProductId);
                    if (pro != null)
                    {
                        context.Products.Remove(pro);
                        context.SaveChanges();
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadProducts(); // Cập nhật danh sách sau khi xóa
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để xóa!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtProductId.Text = string.Empty;
            txtProductName.Text = string.Empty;
            cbxCategory.SelectedIndex = 0;
            cbxBrand.SelectedIndex = 0;
            txtQuantity.Text = "0";
            txtBuyPrice.Text = "0";
            txtSellPrice.Text = "0";
            txtDescription.Text = string.Empty;
            cbxStatus.SelectedIndex = 0;
            imgProductMain.Source = null;
            currentProductImages.Clear();
            currentImageIndex = 0;
        }
    }
}
