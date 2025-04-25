using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;
namespace ProjectPRN
{
    /// <summary>
    /// Interaction logic for StatisticWindow.xaml
    /// </summary>
    public partial class StatisticWindow : Page
    {
        private readonly ProjectPrnContext _context;
        public int TotalProductCount { get; set; }
        public ObservableCollection<object> ProductCountByCategory { get; set; }
        public ObservableCollection<object> ProductCountByBrand { get; set; }
        public ObservableCollection<object> TopRatedProducts { get; set; }
        public ObservableCollection<object> MostCanceledProducts { get; set; }

        public StatisticWindow()
        {
            InitializeComponent();
            _context = new ProjectPrnContext();
            ProductCountByCategory = new ObservableCollection<object>();
            ProductCountByBrand = new ObservableCollection<object>();
            TopRatedProducts = new ObservableCollection<object>();
            MostCanceledProducts = new ObservableCollection<object>();
            LoadStatistics();
            DataContext = this;
        }

        private void LoadStatistics()
        {
            try
            {
                TotalProductCount = _context.Products.Count();

                ProductCountByCategory.Clear();
                foreach (var item in _context.Products.GroupBy(p => p.Category.CategoryName)
                    .Select(g => new { CategoryName = g.Key, Count = g.Count() }))
                {
                    ProductCountByCategory.Add(item);
                }

                ProductCountByBrand.Clear();
                foreach (var item in _context.Products.GroupBy(p => p.Brand.BrandName)
                    .Select(g => new { BrandName = g.Key, Count = g.Count() }))
                {
                    ProductCountByBrand.Add(item);
                }

                TopRatedProducts.Clear();
                foreach (var item in _context.Reviews
                    .GroupBy(r => new { r.Product.ProductId, r.Product.ProductName })
                    .Select(g => new
                    {
                        ProductName = g.Key.ProductName,
                        AverageRating = Math.Round(g.Average(r => (decimal)r.ReviewRating), 2) // Làm tròn 2 chữ số
                    })
                    .OrderByDescending(p => p.AverageRating)
                    .Take(5))
                {
                    TopRatedProducts.Add(item);
                }

                MostCanceledProducts.Clear();
                foreach (var item in _context.OrderDetails
                    .Include(od => od.Order)
                    .Include(od => od.Product)
                    .Where(od => od.Order.Status == "Canceled")
                    .GroupBy(od => new { od.Product.ProductId, od.Product.ProductName })
                    .Select(g => new
                    {
                        ProductName = g.Key.ProductName,
                        CanceledQuantity = g.Sum(od => od.Quantity)
                    })
                    .OrderByDescending(p => p.CanceledQuantity)
                    .Take(5))
                {
                    MostCanceledProducts.Add(item);
                }

                // Update bindings
                DataContext = null;
                DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thống kê: " + ex.Message);
            }
        }
    }
}
